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

    // gets the id of the selected journey
    let journeyId = $(this).attr('data-id');

    // hides all contacts
    $('.chat_list').hide();

    // selects contacts of a journey with the selected id
    let contactsOfJourney = $('.chat_list[data-journeyid="' + journeyId + '"]');

    if (contactsOfJourney.length === 0) { // if the journey hasn't been selected yet
        loadContacts(journeyId); // loads the contacts from the database
    }
    else {
        $(contactsOfJourney).show(); // shows the wanted contacts without using the database
    }

});

function loadContacts(journeyId) {
    $.ajax({
        type: 'get',
        url: '/Chat/GetContacts',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { "journeyId": journeyId },
        success: function (result) {
            displayContacts(result, journeyId);
        },
        error: function (ex) {
            alert('Съжаляваме, възникна грешка.');
        }
    });
}

function displayContacts(result, journeyId) {
    if (result.constructor === Array) {
        result.forEach(function (contact) {
            createElements(contact);
        });
    }
    else {
        createElements(result);
    }

    function createElements(contact) {
        let contactName = $('<h5>' + contact.firstName + ' ' + contact.lastName + '</h5>');
        let chatIb = $('<div>').addClass('chat_ib').append(contactName);
        let chatImg = $('<img>').attr('src', '/images/database/' + contact.profilePictureFileName + '.jpg');
        let chatImgDiv = $('<div>').addClass('chat_img').append(chatImg);
        let chatPeople = $('<div>').addClass('chat_people').append(chatImgDiv).append(chatIb);
        let chatList = $('<div>').addClass('chat_list').attr('data-journeyid', journeyId).append(chatPeople);

        $('.inbox_chat').append(chatList);
    }
}