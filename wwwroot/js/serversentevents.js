listen = (itemId) => {
    var eventSource = new EventSource(`/api/ServerSentEvents/${itemId}`);
    eventSource.onmessage = (event) => {
        const statusDiv = document.getElementById("status");                    
        statusDiv.innerHTML = event.data;
        if (event.data == 'Done!') eventSource.close();
    }
}

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    const statusDiv = document.getElementById("status").innerHTML = "Starting...";
    fetch("/api/ServerSentEvents",
        {
            method: "POST",
            body: {}
        })
        .then(response => response.text())
        .then(text => listen(text));
})  