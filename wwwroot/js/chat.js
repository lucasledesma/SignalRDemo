"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveMessage2", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = "HOLLLLAAAA";
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", e =>  {
    e.preventDefault();
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;

    fetch("/chathub",
    {
        method:"GET",
        body: JSON.stringify({user,message}),
        headers: {
            'content-type': 'application/json'
        }
    })
    .then(response => response.text())
    .then (id=> {
        const status = document.getElementById("messageInput");
        status.innerHTML = id;
    })

    // connection.invoke("SendMessage", user, message).catch(function (err) {
    //     return console.error(err.toString());
    // });
    // event.preventDefault();

});