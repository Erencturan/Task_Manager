using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task_Manager.Core.Abstract.Services;
using Task_Manager.DTOs;
using Task_Manager.Infrastructure.Models;

namespace Task_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        readonly ITaskService _taskService;
        readonly UserManager<User> _userManager;


        public TaskController(ITaskService taskService, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _taskService = taskService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {

            var getTaskList = await _taskService.GetAllAsync();

            return Ok(getTaskList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreatedTask_VM task,DateTime startDate,DateTime endDate)
        {

            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;

            var userId = await _userManager.FindByNameAsync(userName);


            Infrastructure.Models.Task response = new()
            {
                Title = task.Title,
                Description = task.Description,
                CreatedDate = DateTime.Now,
                Status = task.Status,
                StartDate= startDate,
                EndDate= endDate,
                UserId= userId.Id.ToString()
            };



            await _taskService.AddAsync(response);


            return Ok();
        }
    }
}
