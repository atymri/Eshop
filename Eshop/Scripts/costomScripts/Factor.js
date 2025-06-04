
function Command(id, productCount) {

    $.get("/ShopCart/FactorListCommands?productId=" + id + "&count=" + productCount,
        function(res) {
            $("#factorList").html(res);
        });

}