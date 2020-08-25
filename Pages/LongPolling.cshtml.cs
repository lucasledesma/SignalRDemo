using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SignalRDemo.Pages
{
    public class LongPollingModel : PageModel
    {
        private readonly ILogger<LongPollingModel> _logger;

        public LongPollingModel(ILogger<LongPollingModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
