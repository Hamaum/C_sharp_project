using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Api.Data;
using TaskTracker.Api.Models;

namespace TaskTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/Tasks (Получить все)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoTask>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // 2. POST: api/Tasks (Создать новую)
        [HttpPost]
        public async Task<ActionResult<TodoTask>> CreateTask(TodoTask newTask)
        {
            newTask.CreatedAt = DateTime.Now;
            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTasks), new { id = newTask.Id }, newTask);
        }

        // 3. PUT: api/Tasks/5 (Обновить существующую)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TodoTask updatedTask)
        {
            if (id != updatedTask.Id)
            {
                return BadRequest("ID в пути и в теле запроса не совпадают");
            }

            var taskFromDb = await _context.Tasks.FindAsync(id);
            if (taskFromDb == null)
            {
                return NotFound("Задача не найдена");
            }

            taskFromDb.Title = updatedTask.Title;
            taskFromDb.Description = updatedTask.Description;
            taskFromDb.IsCompleted = updatedTask.IsCompleted;

            await _context.SaveChangesAsync();
            return NoContent(); // Возвращает статус 204 (Успешно, без контента)
        }

        // 4. DELETE: api/Tasks/5 (Удалить задачу)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}