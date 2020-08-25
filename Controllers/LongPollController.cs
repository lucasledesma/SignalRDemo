using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SignalRDemo.Helpers;
using SignalRDemo.Models;

namespace SignalRDemo.Controllers
{
    [Route("api/[controller]")]
    public class LongPollController : Controller
    {
        private static readonly StateChecker _stateChecker =
                   new StateChecker(new Random());

        [HttpPost]
        public IActionResult NewItem(Item item)
        {
            return Accepted(_stateChecker.GetNewItem());
        }

        [HttpGet("{itemNo}")]
        public IActionResult GetUpdateForItem(int itemNo)
        {
            var item = new Item();
            item.Id = itemNo;

            UpdateInfo result;

            int i = 0;
            do
            {
                result = _stateChecker.GetUpdate(item);
                Thread.Sleep(3000);
                i++;

            } while (!result.New && i < 3);

            if (result.New)
                return new ObjectResult(result);
            return NoContent();
        }
    }
}