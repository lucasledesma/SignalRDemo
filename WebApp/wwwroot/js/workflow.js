setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalrhub")
        .build();

    connection.on("WorkflowStepDone", (update) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = update;
    });

    connection.on("NewWorkflowFromMe", (item) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = "Starting a new workflow for item " + item.id + "...";        
    });

    connection.on("NewWorkflowFromOthers", (item) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = "Someone else started a workflow...";        
    });

    connection.start()
        .catch(err => console.error(err.toString()));
};

setupConnection();

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    connection.invoke("NewWorkflowInstance");
});