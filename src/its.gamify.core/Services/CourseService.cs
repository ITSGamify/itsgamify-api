using AutoMapper;
using its.gamify.core.Models.Courses;
using its.gamify.core.Services.Interfaces;
using its.gamify.domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace its.gamify.core.Services
{
    public class CourseService(IMapper mapper, IUnitOfWork unitOfWork, IClaimsService claimsService) : ICourseService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IClaimsService _claimsService = claimsService;

        public async Task<List<CourseViewModel>> GetAll(int page, int limit, string q)
        {
            var Courses = await _unitOfWork.CourseRepository.GetAllAsync();
            if (Courses.Count > 0)
            {
                // Sắp xếp danh sách phòng ban theo orderBy
                /*if (orderBy != null && orderBy.Any())
                {
                    foreach (var order in orderBy)
                    {
                        if (order.OrderColumn.ToLower() == "name")
                        {
                            Courses = order.OrderDir.ToUpper() == "ASC" ? Courses.OrderBy(d => d.Name).ToList() : Courses.OrderByDescending(d => d.Name).ToList();
                        }
                    }
                }*/

                // Phân trang
                var pagedCourses = Courses.Skip(page * limit).Take(limit).ToList();
                var CoursesList = _mapper.Map<List<CourseViewModel>>(pagedCourses);

                return CoursesList;
            }
            else throw new Exception("Not have any Course");
        }
        public async Task<CourseViewModel> GetCourse(Guid id)
        {
            var result = await _unitOfWork.CourseRepository.GetByIdAsync(id);
            if (result is not null) return _mapper.Map<CourseViewModel>(result);
            else throw new Exception("Not found");
        }
        public async Task<CourseViewModel> Create(CourseCreateModel item)
        {
            if (item == null) throw new Exception("No data to create");

            var createItem = _mapper.Map<Course>(item);
            await _unitOfWork.CourseRepository.AddAsync(createItem);
            if (await _unitOfWork.SaveChangesAsync()) return _mapper.Map<CourseViewModel>(createItem);
            else throw new Exception("Create failed");
        }
        public async Task<bool> Update(CourseUpdateModel item)
        {
            var updatedItem = await _unitOfWork.CourseRepository.GetByIdAsync(item.Id);
            if (updatedItem != null)
            {
                updatedItem = (Course)_mapper.Map(item, typeof(CourseUpdateModel), typeof(Course));
                _unitOfWork.CourseRepository.Update(updatedItem);
                if (await _unitOfWork.SaveChangesAsync()) return true;
                else throw new Exception("Save change failed!");
            }
            else throw new Exception("Not found");
        }
        public async Task<bool> Delete(Guid Id)
        {
            var deletedItem = await _unitOfWork.CourseRepository.FirstOrDefaultAsync(x => x.Id == Id);
            if (deletedItem != null)
            {
                _unitOfWork.CourseRepository.SoftRemove(deletedItem);
                if (!await _unitOfWork.SaveChangesAsync())
                {
                    throw new Exception("Delete failed!");
                }
                return true;
            }
            else throw new Exception("Not found");
        }
    }
}

