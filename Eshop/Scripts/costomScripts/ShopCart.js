$(function () {
    getTheCount();
});

function getTheCount() {
    $.get("/Api/Cart/",
        function (res) {
            $("#shopChartCount").html(res);

        });
}

function addToCart(id) {
    $.get("/Api/Cart/" + id,
        function (res) {
            $("#shopChartCount").html(res);
            showCart();
        });
}

function showCart() {
    $("#ShowCart").load("/ShopCart/ShowCart");
}