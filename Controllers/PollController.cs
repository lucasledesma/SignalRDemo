using System;
using Microsoft.AspNetCore.Mvc;
using SignalRChat.Helpers;
using SignalRChat.Models;

namespace SignalRChat.Controllers
{
    [Route("api/[controller]")]
    public class PollController : Controller
    {
        private static readonly OrderChecker _orderChecker =
                   new OrderChecker(new Random());
        public PollController()
        {
        }

        [HttpPost]
        public IActionResult OrderCoffee(Order order)
        {
            return Accepted(1);
        }

        [HttpGet("{orderNo}")]
        public IActionResult GetUpdateForOrder(int orderNo)
        {
            var order = new Order();
            order.Id = orderNo;
            var result = _orderChecker.GetUpdate(order);
            if (result.New)
                return new ObjectResult(result);
            return NoContent();
        }
    }
}