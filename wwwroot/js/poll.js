let intervalId;

poll = (orderId) => {
    fetch(`/api/Poll/${orderId}`)
        .then(response => {
            if (response.status === 200) {
                response.json().then(j => {
                    const statusDiv = document.getElementById("status");
                    statusDiv.innerHTML = j.update;
                    if (j.finished)
                        clearInterval(intervalId);
                })
            }
        })
}

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    const statusDiv = document.getElementById("status").innerHTML="Starting...";
    fetch("/api/Poll",
        {
            method: "POST",
            body: { }
        })
        .then(response => response.text())
        .then(id => intervalId = setInterval(poll, 1000, id));
})