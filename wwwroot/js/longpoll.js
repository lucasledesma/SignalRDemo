pollWithTimeout = (url, options, timeout = 10000) => {
    return Promise.race([
        fetch(url, options),
        new Promise((_, reject) =>
            setTimeout(() => reject(new Error('timeout')), timeout)
        )
    ]);
}

poll = (itemId) => {
    pollWithTimeout(`/api/LongPoll/${itemId}`)
        .then(response => {
            if (response.status === 200) {
                const statusDiv = document.getElementById("status");                    
                response.json().then(j => {
                    statusDiv.innerHTML = j.update;
                    if (!j.finished)
                        poll(itemId);
                });
            }
            if (response.status === 204) {
                poll(itemId);
            }
        }
        )
        .catch(response => poll(itemId));
}

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    const statusDiv = document.getElementById("status").innerHTML = "Starting...";
    fetch("/api/LongPoll",
        {
            method: "POST",
            body: {}
        })
        .then(response => response.text())
        .then(text => poll(text));
})