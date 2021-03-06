using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WebApp.Helpers;
using WebApp.Models;
using System.Threading;
using NLog.Extensions.Logging;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;

namespace WebApp.Hubs
{
    public class SignalRHub : Hub
    {

        private readonly StateChecker _stateChecker;

        public SignalRHub(StateChecker stateChecker)
        {
            _stateChecker = stateChecker;
        }

        public async Task GetUpdateForItem(int itemNo)
        {
            var item = new Item();
            item.Id = itemNo;
            UpdateInfo result;
            do
            {
                result = _stateChecker.GetUpdate(item);
                Thread.Sleep(1000);
                if (result.New)
                {
                    await Clients.Caller.SendAsync("ReceiveItemUpdate", result.Update);
                }
            } while (!result.Finished);
            await Clients.Caller.SendAsync("Finished");

        }

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            await Clients.All.SendAsync("NewClientConnected");
        }

        public async Task NewItem()
        {
            Item item = new Item();
            item.Id = _stateChecker.GetNewItem();
            await Clients.Caller.SendAsync("NewItemFromMe", item);
            await Clients.Others.SendAsync("NewItemFromOthers", item);
        }

        public async Task NewWorkflowInstance()
        {
            Item item = new Item();
            item.Id = _stateChecker.GetNewItem();

            var client = ZeebeClient.Builder()
                .UseLoggerFactory(new NLogLoggerFactory())
                .UseGatewayAddress("localhost:26500")
                .UsePlainText()
                .Build();

            await client
                     .NewCreateWorkflowInstanceCommand()
                     .BpmnProcessId("signalrdemo")
                     .LatestVersion()
                     .Variables("{\"connectionid\":\""+ Context.ConnectionId + "\"}")
                     .Send();

            await Clients.Caller.SendAsync("NewWorkflowFromMe", item);
            await Clients.Others.SendAsync("NewWorkflowFromOthers", item);
        }

        public async Task WorkflowStepDone(string workflowid, string connectionid, string stepDone)
        {
            await Clients.Client(connectionid).SendAsync("WorkflowStepDone",stepDone);
        }

    }
}