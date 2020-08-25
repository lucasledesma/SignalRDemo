using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SignalRDemo.Helpers;
using SignalRDemo.Models;
using System.Threading;

namespace SignalRDemo.Hubs
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

    }
}