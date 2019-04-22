using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Components
{
    public class CartTransactions : ViewComponent
    {
        private readonly FuelCartDBContext _context;
        
        public CartTransactions(FuelCartDBContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var transactions = await GetCartTransactions();
            return await Task.FromResult((IViewComponentResult)View("Default", transactions));
        }

        private Task<IEnumerable<CartTransactionViewModel>> GetCartTransactions()
        {
            var cartTransactions = _context.Transactions;
            var transactionModel = cartTransactions
                .Select(x => new
                {
                    x.CartId,
                    x.FuelCart.CartName,
                    x.TransactionId,
                    x.OrderId,
                    x.SequenceId,
                    x.OperatorId,
                    x.StartDateTime,
                    x.EndDateTime,
                    x.IVVolume,
                    x.GVVolume,
                    x.GSTVolume,
                    x.GSVVolume,
                    x.IVTotalizer,
                    x.GVTotalizer,
                    x.GSTTotalizer,
                    x.GSVTotalizer,
                    x.AvgMeterFactor,
                    x.AvgTemperature,
                    x.AvgPressure,
                    x.AvgDensity,
                    x.CTL,
                    x.CPL
                })
                .ToList()
                .Select(y => new CartTransactionViewModel()
                {
                    CartId = y.CartId,
                    CartName = y.CartName,
                    TransactionId = y.TransactionId,
                    OrderId = Convert.ToInt32(y.OrderId),
                    SequenceId = Convert.ToInt32(y.SequenceId),
                    OperatorId = Convert.ToInt32(y.OperatorId),
                    StartDateTime = y.StartDateTime,
                    EndDateTime = y.EndDateTime,
                    IVVolume = y.IVVolume,
                    GVVolume = y.GVVolume,
                    GSTVolume = y.GSTVolume,
                    GSVVolume = y.GSVVolume,
                    IVTotalizer = y.IVTotalizer,
                    GVTotalizer = y.GVTotalizer,
                    GSTTotalizer = y.GSTTotalizer,
                    GSVTotalizer = y.GSVTotalizer,
                    AvgMeterFactor = y.AvgMeterFactor,
                    AvgDensity = y.AvgDensity,
                    AvgPressure = y.AvgPressure,
                    AvgTemperature = y.AvgTemperature,
                    CPL = y.CPL,
                    CTL = y.CTL
                });
            
            return Task.FromResult(transactionModel);
        }
    }
}
