using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SignalRDemo.Helpers;
using SignalRDemo.Models;
using Microsoft.AspNetCore.Http;


namespace SignalRDemo.Controllers
{
    [Route("api/[controller]")]
    public class WebSocketsController : Controller
    {
        private static readonly StateChecker _stateChecker =
                   new StateChecker(new Random());

        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebSocketsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public IActionResult NewItem(Item item)
        {
            return Accepted(_stateChecker.GetNewItem());
        }

        [HttpGet("{itemNo}")]
        public async void GetUpdateForItem(int itemNo)
        {
            var item = new Item();
            item.Id = itemNo;

            var context = _httpContextAccessor.HttpContext;
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await SendEvents(webSocket, item);
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private async Task SendEvents(WebSocket webSocket, Item item)
        {
            UpdateInfo result;
            do
            {
                result = _stateChecker.GetUpdate(item);
                Thread.Sleep(2000);
                if (result.New)
                {
                    var jsonMessage = $"\"{result.Update}\"";
                    await webSocket.SendAsync(buffer: new ArraySegment<byte>(
                        array: Encoding.ASCII.GetBytes(jsonMessage),
                        offset: 0,
                        count: jsonMessage.Length),
                        messageType: WebSocketMessageType.Text,
                        endOfMessage: true,
                        cancellationToken: CancellationToken.None);
                }
            } while (!result.Finished);
           
        }
    }
}