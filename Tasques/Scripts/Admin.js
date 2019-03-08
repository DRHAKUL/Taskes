$(document).ready(function () {
    var index = 1;
    $('#newTask').click(function (event) {
        event.preventDefault();
        index++;
        $('#taskList').append('<tr><td><input type="text" id="part' + index + '"></td></tr>');
    });
   
});