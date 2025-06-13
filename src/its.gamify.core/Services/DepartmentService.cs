using AutoMapper;
using its.gamify.core.Models.Departments;
using its.gamify.core.Services.Interfaces;
using its.gamify.domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace its.gamify.core.Services;
    public class DepartmentService(IMapper mapper, IUnitOfWork unitOfWork, IClaimsService claimsService) : IDepartmentService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IClaimsService _claimsService = claimsService;

    public async Task<List<DepartmentViewModel>> GetAll(int page, int limit, string q)
    {
        var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
        if (departments.Count > 0)
        {
            // Sắp xếp danh sách phòng ban theo orderBy
            /*if (orderBy != null && orderBy.Any())
            {
                foreach (var order in orderBy)
                {
                    if (order.OrderColumn.ToLower() == "name")
                    {
                        departments = order.OrderDir.ToUpper() == "ASC" ? departments.OrderBy(d => d.Name).ToList() : departments.OrderByDescending(d => d.Name).ToList();
                    }
                }
            }*/

            // Phân trang
            var pagedDepartments = departments.Skip(page * limit).Take(limit).ToList();
            var departmentsList = _mapper.Map<List<DepartmentViewModel>>(pagedDepartments);

            return departmentsList;
        }
        else throw new Exception("Not have any department");
    }
    public async Task<DepartmentViewModel> GetDepartment(Guid id)
    {
        var result = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
        if (result is not null) return _mapper.Map<DepartmentViewModel>(result);
        else throw new Exception("Not found");
    }
    public async Task<DepartmentViewModel> Create(DepartmentCreateModel item)
    {
        if (item == null) throw new Exception("No data to create");
        var createItem = _mapper.Map<Department>(item);
        await _unitOfWork.DepartmentRepository.AddAsync(createItem);
        if (await _unitOfWork.SaveChangesAsync()) return _mapper.Map<DepartmentViewModel>(createItem);
        else throw new Exception("Create failed");
    }
    public async Task<bool> Update(DepartmentUpdateModel item)
    {
        var updatedItem = await _unitOfWork.DepartmentRepository.GetByIdAsync(item.Id);
        if (updatedItem != null)
        {
            updatedItem = (Department)_mapper.Map(item, typeof(DepartmentUpdateModel), typeof(Department));
            _unitOfWork.DepartmentRepository.Update(updatedItem);
            if (await _unitOfWork.SaveChangesAsync()) return true;
            else throw new Exception("Save change failed!");
        }
        else throw new Exception("Not found");
    }
    public async Task<bool> Delete(Guid Id)
    {
        var deletedItem = await _unitOfWork.DepartmentRepository.FirstOrDefaultAsync(x => x.Id == Id);
        if (deletedItem != null)
        {
            _unitOfWork.DepartmentRepository.SoftRemove(deletedItem);
            if (!await _unitOfWork.SaveChangesAsync())
            {
                throw new Exception("Delete failed!");
            }
            return true;
        }
        else throw new Exception("Not found");
    }
}
