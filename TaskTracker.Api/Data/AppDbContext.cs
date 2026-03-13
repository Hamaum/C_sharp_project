using Microsoft.EntityFrameworkCore;
using TaskTracker.Api.Models; // Подключаем наши модели

namespace TaskTracker.Api.Data
{
    // Наследуемся от встроенного класса DbContext
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Это свойство будет представлять нашу таблицу с задачами в базе данных
        public DbSet<TodoTask> Tasks { get; set; }
    }
}