
function AddToCompareList(id) {
    $.get("/CompareList/AddToList/" + id,
        function(res) {
            $("#Compare").html(res);
        });
}

function DeleteFormCompare(id) {
    $.get("/CompareList/DeleteItem/" + id,
        function(res) {
            $("#Compare").html(res);
        });
}