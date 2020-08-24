using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SignalRChat.Controllers
{
 [Route("api/[controller]")]
    public class ChatController : Controller
    {
        public ChatController()
        {
        }

        [HttpGet]
        public IActionResult  Get()
        {
            return Accepted("Hey buddy!");
        }   
    } 
}