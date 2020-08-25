using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SignalRDemo.Helpers;
using SignalRDemo.Models;
using Microsoft.AspNetCore.Http;


namespace SignalRDemo.Controllers
{
    [Route("api/[controller]")]
    public class ServerSentEventsController : Controller
    {
        private static readonly StateChecker _stateChecker =
                   new StateChecker(new Random());

        [HttpPost]
        public IActionResult NewItem(Item item)
        {
            return Accepted(_stateChecker.GetNewItem());
        }

        [HttpGet("{itemNo}")]
        public async void GetUpdateForItem(int itemNo)
        {
            Response.ContentType = "text/event-stream";

            var item = new Item();
            item.Id = itemNo;

            UpdateInfo result;

            do
            {
                result = _stateChecker.GetUpdate(item);
                Thread.Sleep(3000);
                if (!result.New) continue;

                await HttpContext.Response.WriteAsync("event: message\n");
                await HttpContext.Response.WriteAsync("data:" + result.Update + "\n\n");
                await HttpContext.Response.Body.FlushAsync();

            } while (!result.Finished);

            Response.Body.Close();
        }
    }
}