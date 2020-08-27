using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages
{
    public class PollingModel : PageModel
    {
        private readonly ILogger<PollingModel> _logger;

        public PollingModel(ILogger<PollingModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
