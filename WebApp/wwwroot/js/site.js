// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/fuelCartHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();



$( function () {
    window.console.log(connection);
    var cartStatusStore;

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("loadCartStatus", function (carts) {
        window.console.log("status....");
        cartStatusStore = new DevExpress.data.CustomStore({
            load: function () {
                return carts;
            },
            key: "cartId"
        });
        $("#cartStatus").dxDataGrid({
            dataSource: cartStatusStore
        });
    });

    connection.on("updateCartStatus", function (cartStatus) {

        cartStatusStore.push([{ type: "update", key: cartStatus.cartId, data: cartStatus }]);
    });


    LoadCartModel();
});

function LoadCartModel() {
    $.ajax({
        type: "POST",
        url: "/LoadCartModel",
        success: function (result) {
            window.console.log("success...");
            //window.console.log(result.cartModel);
            //cartStatusStore = new DevExpress.data.CustomStore({
            //    load: function () {
            //        return result;
            //    },
            //    key: "cartId"
            //});
            //$("#cartStatus").dxDataGrid({
            //    dataSource: cartStatusStore
            //})
        }
    })

};