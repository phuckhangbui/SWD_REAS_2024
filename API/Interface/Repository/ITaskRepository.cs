using API.DTOs;
using API.Helper;

namespace API.Interfaces
{
    public interface ITaskRepository : IBaseRepository<Entity.Task>
    {
        //Admin
        Task<PageList<TaskDto>> GetTasksAsync(TaskParam taskParam, int adminId);
        Task<TaskDto> CreateTaskAsync(CreateUpdateTaskDto taskDto, int adminId);
        Task<TaskDto> EditTaskAsync(CreateUpdateTaskDto taskDto, int adminId, int taskId);
        Task<bool> CheckAccountCreateValid(int accountId);
        Task<bool> CheckAccountAssignedValid(int accountId);
        Task<TaskDto?> GetTaskAsync(int adminId, int taskId);
        
        //Staff
        Task<PageList<TaskDto>> GetTasksStaffRoleAsync(TaskParam taskParam, int staffId);
        Task<TaskDto?> GetTaskStaffRoleAsync(int staffId, int taskId);

        //Admin and staff
        Task<TaskDto> UpdateTaskStatus(int taskId, int taskStatus);
    }
}
