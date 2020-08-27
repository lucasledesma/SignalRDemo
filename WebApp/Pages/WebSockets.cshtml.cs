using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages
{
    public class WebSocketsModel : PageModel
    {
        private readonly ILogger<WebSocketsModel> _logger;

        public WebSocketsModel(ILogger<WebSocketsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
