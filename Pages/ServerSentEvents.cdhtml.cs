using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SignalRDemo.Pages
{
    public class ServerSentEventsModel : PageModel
    {
        private readonly ILogger<ServerSentEventsModel> _logger;

        public ServerSentEventsModel(ILogger<ServerSentEventsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
