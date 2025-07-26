using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Only Admin can create tasks
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return Ok("Task Created Successfully by Admin");
        }

        // ✅ Both Admin and User can view tasks
        [HttpGet("get-all")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetTasks([FromQuery] string search = "", [FromQuery] string titleFilter = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var query = _context.Tasks.AsQueryable();  // Start with all tasks

            // Search functionality
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
            }

            // Title Filter functionality
            if (!string.IsNullOrEmpty(titleFilter))
            {
                query = query.Where(t => t.Title.Contains(titleFilter));
            }

            // For Users, show only their tasks
            if (role != "Admin")
            {
                query = query.Where(t => t.UserId == userId);
            }

            // Pagination functionality (skip and take)
            var tasks = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(tasks);
        
        }
        // ✅ Update Task (Admin or Owner User)
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult UpdateTask(int id, TaskItem updatedTask)
        {
            var task = _context.Tasks.Find(id);
            if (task == null) return NotFound("Task not found");

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (role != "Admin" && task.UserId != userId)
                return Forbid("You cannot edit this task");

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            _context.SaveChanges();

            return Ok("Task Updated Successfully");
        }



        // ✅ Only Admin can delete tasks
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null) return NotFound("Task not found");
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return Ok("Task Deleted Successfully by Admin");
        }
    }
}

