using Microsoft.AspNetCore.Mvc;
using SK.TinyScheduler.Database.Entities;
using SK.TinyScheduler.Database;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly SchedulerDbContext _context;
        private readonly ILogger<JobController> _logger;

        public JobController(SchedulerDbContext context, ILogger<JobController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet, Route("")]
        public IEnumerable<Job> Get()
        {
            return _context.Jobs;
        }
    }
}
