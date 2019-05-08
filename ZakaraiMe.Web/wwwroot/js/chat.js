var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

//Disable send button until connection is established
$("#sendMessageBtn").prop('disabled', true);

connection.on("ReceiveMessage", function (userId, message, messageId) {
    let msg = encodeMessage(message);
    let messageHtml = createIncomingMessageHtml(userId, msg, messageId);

    let msgHistory = getMsgHistory(userId);
    msgHistory.append(messageHtml);
});

connection.start().then(function () {
    $("#sendMessageBtn").prop('disabled', false);
}).catch(function (err) {
    return console.error(err.toString());
    });

// catch enter button and html button clicked in order to send the message
$('#messageInput').on('keypress', function (event) {
    if (event.which === 13) {
        sendMessage(event);
    }
});

$("#sendMessageBtn").on("click", function (event) {
    sendMessage(event);
});

// catch the journey click
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

function sendMessage(event) {
    let receiverId = $('.msg_history:visible').attr('data-contactid');

    if (receiverId !== undefined) {
        let message = $("#messageInput").val();

        if (message !== '') {
            connection.invoke("SendMessage", receiverId, message).catch(function (err) {
                return console.error(err.toString());
            });

            let messageHtml = createOutgoingMessageHtml(message, 0);
            let msgHistory = getMsgHistory(receiverId);

            msgHistory.append(messageHtml);

            $('#messageInput').val('');
            $(msgHistory).scrollTop(function () { return this.scrollHeight; });
        }
    }
    event.preventDefault();
}

function loadContacts(journeyId) {
    $.ajax({
        type: 'get',
        url: '/Chat/GetContacts',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { 'journeyId': journeyId },
        success: function (result) {
            displayContacts(result, journeyId);
        },
        error: function (ex) {
            alert('Съжаляваме, възникна грешка.');
        }
    });
}

function loadHistory(contactId, lastMessageId) {
    $.ajax({
        type: 'get',
        url: '/Chat/GetMessages',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { 'receiverId': contactId, 'lastMessageId': lastMessageId },
        success: function (result) {
            displayHistory(result, contactId);
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

            let oldestMessageIdInChat = $(msgHistory).find('div[data-messageid]').attr('data-messageid');     

            if (oldestMessageIdInChat === undefined) {
                oldestMessageIdInChat = 0;
            }

            loadHistory(contact.id, oldestMessageIdInChat);
            $(msgHistory).scrollTop(function () { return this.scrollHeight; });
        });
    }
}

function displayHistory(result, contactId) {
    let msgHistory = getMsgHistory(contactId);

    result.forEach(function (message) {
        message.text = encodeMessage(message.text);     
        let messageHtml;

        if (contactId === message.senderId) {
            messageHtml = createIncomingMessageHtml(contactId, message.text, message.id);
        }
        else {
            messageHtml = createOutgoingMessageHtml(message.text, message.id);
        }
        
        msgHistory.prepend(messageHtml);
    });
}

function createIncomingMessageHtml(contactId, message, messageId) {
    let msgText = $('<p>').text(message);
    let msgDiv = $('<div>').addClass('received_withd_msg')
        .attr('data-messageid', messageId)
        .append(msgText);
    let contactImg = $('.chat_list[data-contactid="' + contactId + '"]').find('img').clone();
    let contactImgDiv = $('<div>').addClass('incoming_msg_img')
        .append(contactImg);
    let wrapperDiv = $('<div>').addClass('incoming_msg')
        .append(contactImgDiv)
        .append(msgDiv);

    return wrapperDiv;
}

function createOutgoingMessageHtml(message, messageId) {
    let msgText = $('<p>').text(message);
    let msgDiv = $('<div>').addClass('sent_msg')
        .attr('data-messageid', messageId)
        .append(msgText);
    let wrapperDiv = $('<div>').addClass('outgoing_msg')
        .append(msgDiv);

    return wrapperDiv;    
}

function getMsgHistory(contactId) {
    // get the message box for that contact
    let msgHistory = $('.msg_history[data-contactid="' + contactId + '"]');

    // if the message box with this contact doesn't exist
    if (msgHistory.length === 0) {
        msgHistory = $('<div>').addClass('msg_history')
            .attr('data-contactid', contactId)
            .hide();
        $('.mesgs').prepend(msgHistory);

        $(msgHistory).on('scroll', function () {
            var scrollTop = $(this).scrollTop();
            if (scrollTop <= 0) {
                let oldestMessageIdInChat = $(msgHistory).find('div[data-messageid]').attr('data-messageid');

                if (oldestMessageIdInChat === undefined) {
                    oldestMessageIdInChat = 0;
                }

                loadHistory(contactId, oldestMessageIdInChat);
            }
        });
    }

    return msgHistory;
}

function encodeMessage(message) {
    return message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
}