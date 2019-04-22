// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/fuelCartHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

var cartStatusStore;

$(function () {
   
    connection.on("loadCartStatus", function (carts) {
        window.console.log("status....");
        cartStatusStore = new DevExpress.data.CustomStore({
            key: "cartId",
            load: function () {
                return carts;
            }
        });
        $("#cartStatus").dxDataGrid({
            dataSource: cartStatusStore
        });
    });

    connection.on("updateCartStatus", function (cartStatus) {

        cartStatusStore.push([{ type: "update", key: cartStatus.cartId, data: cartStatus }]);
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });
});

function LoadCartModel() {
    $.ajax({
        type: "POST",
        url: "/LoadCartStatus",
        success: function (result) {
            window.console.log("success");
            //cartStatusStore = new DevExpress.data.CustomStore({
            //    key: "cartId",
            //    load: function () {
            //        return result;
            //    }
            //});
            //$("#cartStatus").dxDataGrid({
            //    dataSource: cartStatusStore
            //})
        }
    })

};
