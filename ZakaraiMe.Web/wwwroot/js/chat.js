var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

//Disable send button until connection is established
$("#sendMessaegBtn").prop('disabled', true);

connection.on("ReceiveMessage", function (userId, message) {
    let msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    console.log(userId);
    createIncomingMessageHtml(userId, msg);
});

connection.start().then(function () {
    $("#sendMessageBtn").prop('disabled', false);
}).catch(function (err) {
    return console.error(err.toString());
});

$("#sendMessageBtn").on("click", function (event) {
    let receiverId = $('.msg_history:visible').attr('data-contactid');

    if (receiverId !== undefined) {
        let message = $("#messageInput").val();

        if (message !== '') {
            connection.invoke("SendMessage", receiverId, message).catch(function (err) {
                return console.error(err.toString());
            });

            createOutgoingMessageHtml(receiverId, message);

            $('#messageInput').val('');
        }
    }
    event.preventDefault();
});

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
            createContactHtml(contact);
        });
    }
    else {
        createContactHtml(result);
    }

    function createContactHtml(contact) {
        // create the contact element
        let contactName = $('<h5>' + contact.firstName + ' ' + contact.lastName + '</h5>');
        let chatIb = $('<div>').addClass('chat_ib')
            .append(contactName);
        let chatImg = $('<img>').attr('src', '/images/database/' + contact.profilePictureFileName + '.jpg');
        let chatImgDiv = $('<div>').addClass('chat_img')
            .append(chatImg);
        let chatPeople = $('<div>').addClass('chat_people')
            .append(chatImgDiv)
            .append(chatIb);
        let chatList = $('<div>').addClass('chat_list')
            .attr('data-journeyid', journeyId)
            .attr('data-contactid', contact.id)
            .append(chatPeople);

        $('.inbox_chat').append(chatList);

        // get the message box for that contact
        let msgHistory = getMsgHistory(contact.id);

        $(chatList).on('click', function () {
            $('.chat_list').removeClass('active_chat');
            $(this).addClass('active_chat');

            // hide all message boxes
            $('.msg_history').hide();
            // show only the clicked contact's box
            $(msgHistory).show();
        });
    }
}

function createIncomingMessageHtml(receiverId, message) {
    let msgText = $('<p>').text(message);
    let msgDiv = $('<div>').addClass('received_withd_msg')
        .append(msgText);
    let senderImg = $('<img>').attr('src', 'https://ptetutorials.com/images/user-profile.png'); //TODO: Change source; get it from other html elements or from database or send it from signalR
    let senderImgDiv = $('<div>').addClass('incoming_msg_img')
        .append(senderImg);
    let wrapperDiv = $('<div>').addClass('incoming_msg')
        .append(senderImgDiv)
        .append(msgDiv);

    // get the message box for that contact
    let msgHistory = getMsgHistory(receiverId);

    msgHistory.append(wrapperDiv);
}

function createOutgoingMessageHtml(receiverId, message) {
    let msgText = $('<p>').text(message);
    let msgDiv = $('<div>').addClass('sent_msg')
        .append(msgText);
    let wrapperDiv = $('<div>').addClass('outgoing_msg')
        .append(msgDiv);

    let msgHistory = getMsgHistory(receiverId);

    msgHistory.append(wrapperDiv);
}

function getMsgHistory(userId) {
    // get the message box for that contact
    let msgHistory = $('.msg_history[data-contactid="' + userId + '"]');

    // if the message box with this contact doesn't exist
    if (msgHistory.length === 0) {
        msgHistory = $('<div>').addClass('msg_history')
            .attr('data-contactid', userId)
            .hide();
        $('.mesgs').prepend(msgHistory);
    }

    return msgHistory;
}