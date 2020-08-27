listen = (itemId) => {
    const socket = new WebSocket(`wss://localhost:5001/api/WebSockets/${itemId}`);

    socket.onmessage = (event) => {
        const statusDiv = document.getElementById("status");                    
        statusDiv.innerHTML = event.data;
        if (event.data == 'Done!') socket.close();
    }
}

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    const statusDiv = document.getElementById("status").innerHTML = "Starting...";
    fetch("/api/WebSockets",
        {
            method: "POST",
            body: {}
        })
        .then(response => response.text())
        .then(text => listen(text));
})  