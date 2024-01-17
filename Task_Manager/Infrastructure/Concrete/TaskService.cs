using System.Linq.Expressions;
using Task_Manager.Core.Abstract.Services;

namespace Task_Manager.Infrastructure.Concrete
{
    public class TaskService : Repository<Infrastructure.Models.Task>, ITaskService
    {
        public TaskService(AppDbContext context) : base(context)
        {
        }
    }
}
