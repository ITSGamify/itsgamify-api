using its.gamify.core.Models.Departments;

namespace its.gamify.core.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentViewModel>> GetAll(int page, int limit, string q);
        Task<DepartmentViewModel> GetDepartment(Guid id);
        Task<DepartmentViewModel> Create(DepartmentCreateModel item);
        Task<bool> Update(DepartmentUpdateModel item);
        Task<bool> Delete(Guid Id);
    }
}
