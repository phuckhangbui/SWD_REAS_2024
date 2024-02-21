using API.DTOs;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TaskController : BaseApiController
    {
        private readonly ITaskRepository _taskRepository;

        private const string AdminBaseUri = "/api/admin/{adminId}/tasks";
        private const string AdminDetailUri = AdminBaseUri + "/{taskId}";
        private const string StaffBaseUri = "/api/staff/{staffId}/tasks";
        private const string StaffDetailUri = StaffBaseUri + "/{taskId}";
        private const string UpdateStatusUri = "/api/{taskId}/{status}";

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet(StaffBaseUri)]
        [Authorize(policy: "Staff")]
        public async Task<IActionResult> GetStaffTasks([FromQuery] TaskParam taskParam, int staffId)
        {
            if (taskParam == null)
            {
                return BadRequest(new ApiResponse(400));
            }

            var tasks = await _taskRepository.GetTasksStaffRoleAsync(taskParam, staffId);
            if (tasks == null)
            {
                return BadRequest(new ApiResponse(404, "This account with staff role not found"));
            }
            else
            {
                Response.AddPaginationHeader(new PaginationHeader(tasks.CurrentPage, tasks.PageSize,
            tasks.TotalCount, tasks.TotalPages));

                return Ok(tasks);
            }
        }

        [HttpGet(StaffDetailUri)]
        [Authorize(policy: "Staff")]
        public async Task<IActionResult> GetStaffTaskDetail(int staffId, int taskId)
        {
            var task = await _taskRepository.GetTaskStaffRoleAsync(staffId, taskId);
            if (task == null)
            {
                return BadRequest(new ApiResponse(404, "Task or staff not found"));
            }

            return Ok(task);
        }

        [HttpPost(UpdateStatusUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, int status)
        {
            var task = await _taskRepository.UpdateTaskStatus(taskId, status);
            if (task == null)
            {
                return BadRequest(new ApiResponse(404, "Task not found"));
            }
            else
            {
                return Ok(task);
            }
        }

        [HttpGet(AdminBaseUri)]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> GetTasksInAdminRole([FromQuery] TaskParam taskParam, int adminId)
        {
            var isAdmin = await _taskRepository.CheckAccountCreateValid(adminId);
            if (!isAdmin)
            {
                return BadRequest(new ApiResponse(400, "Invalid admin account"));
            }

            var tasks = await _taskRepository.GetTasksAsync(taskParam, adminId);

            Response.AddPaginationHeader(new PaginationHeader(tasks.CurrentPage, tasks.PageSize,
            tasks.TotalCount, tasks.TotalPages));

            return Ok(tasks);
        }

        [HttpGet(AdminDetailUri)]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> GetTaskDetailInAdminRole(int adminId, int taskId)
        {
            var isAdmin = await _taskRepository.CheckAccountCreateValid(adminId);
            if (!isAdmin)
            {
                return BadRequest(new ApiResponse(400, "Invalid admin account"));
            }

            var task = await _taskRepository.GetTaskAsync(adminId, taskId);
            if (task == null)
            {
                return BadRequest(new ApiResponse(404, "Task not found"));
            }
            else
            {
                return Ok(task);
            }
        }

        [HttpPost(AdminBaseUri)]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> AddTask(CreateUpdateTaskDto taskDto, int adminId)
        {
            var isAdmin = await _taskRepository.CheckAccountCreateValid(adminId);
            if (!isAdmin)
            {
                return BadRequest(new ApiResponse(400, "Invalid admin account"));
            }

            var isValidAccountAssigned = await _taskRepository.CheckAccountAssignedValid(taskDto.AccountAssignedId);
            if (!isValidAccountAssigned)
            {
                return BadRequest(new ApiResponse(400, "Invalid account assigned"));
            }

            var newTask = await _taskRepository.CreateTaskAsync(taskDto, adminId);

            return Ok(newTask);
        }

        [HttpPost(AdminDetailUri)]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> UpdateTask(CreateUpdateTaskDto taskDto, int adminId, int taskId)
        {
            var task = await _taskRepository.GetTaskAsync(adminId, taskId);
            if (task == null)
            {
                return BadRequest(new ApiResponse(404, "Task not found"));
            }

            var isValidAccountAssigned = await _taskRepository.CheckAccountAssignedValid(taskDto.AccountAssignedId);
            if (!isValidAccountAssigned)
            {
                return BadRequest(new ApiResponse(400, "Invalid account assigned"));
            }

            var newTask = await _taskRepository.EditTaskAsync(taskDto, adminId, taskId);

            return Ok(newTask);
        }
    }
}
