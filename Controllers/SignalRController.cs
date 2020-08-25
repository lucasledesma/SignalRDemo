using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SignalRDemo.Helpers;
using SignalRDemo.Models;
using Microsoft.AspNetCore.Http;
using SignalRDemo.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace SignalRDemo.Controllers
{
    [Route("api/[controller]")]
    public class SignalRController : Controller
    {
        private static readonly StateChecker _stateChecker =
                   new StateChecker(new Random());

        private readonly IHubContext<SignalRHub> _signalRHub;

        public SignalRController(IHubContext<SignalRHub> signalRHub)
        {
            _signalRHub = signalRHub;
        }

        [HttpPost]
        public async Task<IActionResult> NewItem([FromBody] Item item)
        {
            await _signalRHub.Clients.All.SendAsync("NewItem", item);
            return Accepted(_stateChecker.GetNewItem());
        }

   }
}