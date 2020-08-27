using System;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class PollController : Controller
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
            var result = _stateChecker.GetUpdate(item);
            if (result.New)
                return new ObjectResult(result);
            return NoContent();
        }
    }
}