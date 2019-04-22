using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Hubs;
using WebApp.Models;
using WebApp.ViewModels;
using static WebApp.ViewModels.OASTagModels;

namespace WebApp.Infrastructure
{
    public class OpenAutomationSoftware : IOpenAutomationSoftware
    {
        private OASData.Data oasData = new OASData.Data();
        private OASConfig.Config oasConfig = new OASConfig.Config();
        internal static string NetworkNode; // Set by the user
        internal static string NetworkPath; // Network path to include for remoting
        internal static bool NetworkNodeChanged;
        internal static bool m_InProcessValues = false;
        internal static bool m_closing = false;
        internal static string DataType = "Grid";


        // A Queue or Hashtable is recommended to receieve your values so they can be processed on a seperate thread.
        // You can also process data directly from the ValuesChangedAll Event, but keep in mind other values will be buffered until you release the Event.
        private Queue m_DataValuesQueue = new Queue(); // This Queue demonstrates a technique for processing a lot of data.
        private Hashtable m_DataValuesHashtable = new Hashtable(); // This hashtable will contain the values for each Tag to access whenever you like.

        private readonly Timer _timer;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(500);
        private IHubContext<FuelCartHub> _hubContext;
        private FuelCartDBContext _dbContext;
        private IConfiguration _configuration;
        private List<CartStatus> carts = new List<CartStatus>();

        public OpenAutomationSoftware(IHubContext<FuelCartHub> fuelCartHub, FuelCartDBContext context, IConfiguration config)
        {
            _hubContext = fuelCartHub;
            _dbContext = context;
            _configuration = config;
            oasData.ValuesChangedAll += OasData_ValuesChangedAll;
            _timer = new Timer(ProcessQueue, null, _updateInterval, _updateInterval);
            var cartList = _dbContext.FuelCarts;
            foreach(var c in cartList)
            {
                carts.Add(new CartStatus { CartId = c.CartId, CartName = c.CartName, OASGroup = c.OASGroupName, Status = "Initializing" });
            }

        }

        private void OasData_ValuesChangedAll(string[] Tags, object[] Values, bool[] Qualities, DateTime[] TimeStamps)
        {
            // High speed version with a lot of data values changing
            lock (m_DataValuesQueue.SyncRoot)
            {
                m_DataValuesQueue.Enqueue(new ClassTagValues(Tags, Values, Qualities, TimeStamps));
            }
        }

        private void ProcessQueue(object state)
        {
            if (m_InProcessValues)
            {
                return;
            }
            if (m_closing)
            {
                return;
            }
            m_InProcessValues = true;

            ClassTagValues[] arrTagValues = new ClassTagValues[0];
            Int32 numberOfTagValues = 0;
            lock (m_DataValuesQueue.SyncRoot)
            {
                if (m_DataValuesQueue.Count < 1)
                {
                    m_InProcessValues = false;
                    return; // There is nothing to do
                }
                numberOfTagValues = m_DataValuesQueue.Count;
                arrTagValues = new ClassTagValues[numberOfTagValues];
                m_DataValuesQueue.CopyTo(arrTagValues, 0);
                m_DataValuesQueue.Clear();
            }

            Int32 allTagValuesIndex = 0;
            ClassTagValues allTagValues = null;

            string[] tagNames = null;
            object[] tagValues = null;
            bool[] tagQualities = null;
            DateTime[] tagTimeStamps = null;
            Int32 tagIndex = 0;
            Int32 numberOfTags = 0;

            System.Text.StringBuilder UpdateString = new System.Text.StringBuilder();
            double ValueDouble = 0;
            int ValueInteger = 0;
            bool ValueBoolean = false;
            string ValueString = null;
            ValueDouble = 0;
            ValueInteger = 0;
            ValueBoolean = false;
            ValueString = "";

            for (allTagValuesIndex = 0; allTagValuesIndex < numberOfTagValues; allTagValuesIndex++)
            {
                // Obtain arrays of tags and values from the tag values class previously stored in the OpcSystemsData_ValuesChangedAll event
                allTagValues = arrTagValues[allTagValuesIndex];
                tagNames = allTagValues.TagNames;
                tagValues = allTagValues.Values;
                tagQualities = allTagValues.Qualities;
                tagTimeStamps = allTagValues.TimeStamps;
                numberOfTags = tagNames.GetLength(0);
                CartStatus cartStatus = new CartStatus();
                string value = null;
                for (tagIndex = 0; tagIndex < numberOfTags; tagIndex++)
                {
                    if (tagQualities[tagIndex])
                    {
                        if (object.ReferenceEquals(tagValues[tagIndex].GetType(), typeof(double)))
                        {
                            value = Convert.ToDouble(tagValues[tagIndex]).ToString();
                        }
                        else if (object.ReferenceEquals(tagValues[tagIndex].GetType(), typeof(int)))
                        {
                            value = Convert.ToInt32(tagValues[tagIndex]).ToString();
                        }
                        else if (object.ReferenceEquals(tagValues[tagIndex].GetType(), typeof(bool)))
                        {
                            value = Convert.ToBoolean(tagValues[tagIndex]).ToString();
                        }
                        else if (object.ReferenceEquals(tagValues[tagIndex].GetType(), typeof(string)))
                        {
                            value = tagValues[tagIndex].ToString();
                        }
                        if (tagNames[tagIndex].Contains("Status"))
                        {
                            UpdateCartStatus();
                            //_hubContext.Clients.All.SendAsync("updateCartStatus", new CartStatus {  })
                            Console.WriteLine("StatusKey");
                        }

                        //if (DataType == "Grid")
                        //{
                        //    _hubContext.Clients.All.SendAsync("updateTagValue", new TagList { TagName = tagNames[tagIndex].Substring(0, tagNames[tagIndex].IndexOf(".")), TimeStamp = tagTimeStamps[tagIndex], Value = value });
                        //}
                        //if (tagNames[tagIndex] == "Random.Value")
                        //{
                        //    _hubContext.Clients.All.SendAsync("insertChartData", new ChartData { TagName = tagNames[tagIndex].Substring(0, tagNames[tagIndex].IndexOf(".")), TimeStamp = tagTimeStamps[tagIndex], Value = Convert.ToDouble(value) });
                        //    _hubContext.Clients.All.SendAsync("updateGaugeValue", Convert.ToDouble(value));
                        //}


                    }
                }

                lock (m_DataValuesHashtable.SyncRoot)
                {
                    for (tagIndex = 0; tagIndex < numberOfTags; tagIndex++)
                    {
                        try
                        {
                            UpdateString.Append(tagTimeStamps[tagIndex].ToString("HH:mm:ss.fff") + " ");
                        }
                        catch (Exception ex)
                        {
                            UpdateString.Append("Unknown Time ");
                        }
                        UpdateString.Append(tagNames[tagIndex] + " = ");

                        if (tagQualities[tagIndex]) // The Tag quality is good
                        {
                            // Store value to a hashtable so you can access it from your own routines
                            if (m_DataValuesHashtable.Contains(tagNames[tagIndex]))
                            {
                                m_DataValuesHashtable[tagNames[tagIndex]] = tagValues[tagIndex];
                            }
                            else
                            {
                                m_DataValuesHashtable.Add(tagNames[tagIndex], tagValues[tagIndex]);
                            }
                            try
                            {
                                if (object.ReferenceEquals(tagValues[tagIndex].GetType(), typeof(double)))
                                {
                                    ValueDouble = Convert.ToDouble(tagValues[tagIndex]);
                                    UpdateString.Append(ValueDouble.ToString());
                                }
                                else if (object.ReferenceEquals(tagValues[tagIndex].GetType(), typeof(int)))
                                {
                                    ValueInteger = Convert.ToInt32(tagValues[tagIndex]);
                                    UpdateString.Append(ValueInteger.ToString());
                                }
                                else if (object.ReferenceEquals(tagValues[tagIndex].GetType(), typeof(bool)))
                                {
                                    ValueBoolean = Convert.ToBoolean(tagValues[tagIndex]);
                                    UpdateString.Append(ValueBoolean.ToString());
                                }
                                else if (object.ReferenceEquals(tagValues[tagIndex].GetType(), typeof(string)))
                                {
                                    ValueString = tagValues[tagIndex].ToString();
                                    UpdateString.Append(ValueString);
                                }
                            }
                            catch (Exception ex)
                            {
                                UpdateString.Append("Error");
                            }
                        }
                        else // The Tag quality is bad
                        {
                            // Remove the value from the hashtable so you know the value is bad from your own routines
                            if (m_DataValuesHashtable.Contains(tagNames[tagIndex]))
                            {
                                m_DataValuesHashtable.Remove(tagNames[tagIndex]);
                            }
                            UpdateString.Append("Bad Quality");

                        }
                        UpdateString.Append("\r" + "\n");
                        try
                        {
                            //foreach(var i in tagListModel)
                            //{
                            //    Debug.WriteLine("TagListModel tagName: " + i.TagName);
                            //}

                            //_hubContext.Clients.All.SendAsync("updateTagValues", tagListModel);

                            //Debug.WriteLine(UpdateString.ToString());
                            UpdateString.Remove(0, UpdateString.Length);
                        }
                        catch (Exception ex)
                        {

                        }
                        
                    }


                }
            }
            
            m_InProcessValues = false;
        }

        private void UpdateCartStatus()
        {
            foreach (var c in carts)
            {
                if (m_DataValuesHashtable.ContainsKey(c.OASGroup + ".Status.Value"))
                {
                    c.Status = m_DataValuesHashtable[c.OASGroup + ".Status.Value"].ToString();                    
                }
                if (m_DataValuesHashtable.ContainsKey(c.OASGroup + ".DDGrossVolume.Value"))
                {
                    c.GrossVolume = Convert.ToDecimal(m_DataValuesHashtable[c.OASGroup + ".DDGrossVolume.Value"]);
                }
                if (m_DataValuesHashtable.ContainsKey(c.OASGroup + ".DDNetVolume.Value"))
                {
                    c.NetVolume = Convert.ToDecimal(m_DataValuesHashtable[c.OASGroup + ".DDNetVolume.Value"]);
                }
                _hubContext.Clients.All.SendAsync("updateCartStatus", c);
            }
        }

        public void LoadCartStatus()
        {
            List<TagList> tagList = new List<TagList>();
            var networkNode = _configuration.GetValue<string>("OASServer");

            foreach (var g in carts)
            {
                var tags = oasConfig.GetTagNames(g.OASGroup, networkNode);
                foreach(var i in tags)
                {
                    tagList.Add(new TagList { TagName = i });
                    AddTags(g.OASGroup, networkNode);
                }


            }
            _hubContext.Clients.All.SendAsync("loadCartStatus", carts);
        }

        public void AddTags(string groupName, string networkNode)
        {

            var tags = oasConfig.GetTagNames(groupName, NetworkNode);
            string[] values = new string[tags.Count()];
            if (groupName == "")
            {
                values = tags
                    .Select(x => x + ".Value")
                    .ToArray();
            }
            else
            {
                values = tags
                    .Select(x => NetworkPath + groupName + "." + x + ".Value")
                    .ToArray();
            }

            oasData.AddTags(values);
            

        }

        public void RemoveAllTags()
        {
            oasData.RemoveAllTags();
        }

        internal class ClassTagValues
        {
            internal string[] TagNames;
            internal object[] Values;
            internal bool[] Qualities;
            internal DateTime[] TimeStamps;

            public ClassTagValues(string[] NewTagNames, object[] NewValues, bool[] NewQualities, DateTime[] NewTimeStamps)
            {
                TagNames = NewTagNames;
                Values = NewValues;
                Qualities = NewQualities;
                TimeStamps = NewTimeStamps;
            }
        }
    }
}
