using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages
{
    public class WorkflowModel : PageModel
    {
        private readonly ILogger<WorkflowModel> _logger;

        public WorkflowModel(ILogger<WorkflowModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
