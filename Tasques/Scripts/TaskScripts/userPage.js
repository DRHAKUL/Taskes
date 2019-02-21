$(function () {
    buildTaskSelector();
});


function buildTaskSelector(){
    $.ajax({
        type: "GET",
        url: "@Url.Action("getUserTasks")",
        content: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (d) {
            if (d.success == true)
                alert('Has arribat!!');
            else { }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Error!!');
        }
    });
}
