//var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

////Disable send button until connection is established
//$("#sendButton").disabled = true;

//connection.on("ReceiveMessage", function (user, message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var encodedMsg = user + " says " + msg;
//    var li = document.createElement("li");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});

//connection.start().then(function () {
//    document.getElementById("sendButton").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

$('.journeyOption').on('click', function () {
    $('.journeyOption-data').removeClass('journeyOption-active');
    $(this).children('.journeyOption-data').addClass('journeyOption-active');
    let journeyId = $(this).attr('data-id');

    loadContacts(journeyId);
});

function loadContacts (journeyId) {
    $.ajax({
        type: 'get',
        url: '/Chat/GetContacts',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { "journeyId": journeyId },
        success: function (result) {
            console.log(result);
        },
        error: function (ex) {
            console.log(ex);
            alert('Съжаляваме, възникна грешка.');
        }
    });
}