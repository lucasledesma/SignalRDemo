// WebSocket = undefined;
// EventSource = undefined;

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalrhub")
        .build();

    connection.on("ReceiveItemUpdate", (update) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = update;
    });

    connection.on("NewItemFromOthers", (item) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = "Someone else created item " + item.id;
    });

    connection.on("NewItemFromMe", (item) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = "Starting item " + item.id + " waiting for server updates...";        
        connection.invoke("GetUpdateForItem",parseInt(item.id))
    });


    connection.on("Finished", () => {
        //    connection.stop();
    });

    connection.start()
        .catch(err => console.error(err.toString()));
};

setupConnection();

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    connection.invoke("NewItem");
});