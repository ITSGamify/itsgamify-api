using AutoMapper;
using its.gamify.core.Models.Courses;
using its.gamify.core.Models.Departments;
using its.gamify.domains.Entities;

namespace its.gamify.core.Mappers;
public class MapperConfigurationProfile : Profile
{
    public MapperConfigurationProfile()
    {
        #region Customer
        //CreateMap<Admin, AdminCreateModel>().ReverseMap();
        //CreateMap<Customer, CustomerViewModel>().ReverseMap();
        //CreateMap<CustomerCreateModel, Customer>().ReverseMap();
        //CreateMap<CustomerUpdateModel, Customer>().ReverseMap();
        #endregion

        #region BloodUnit
        //CreateMap<BloodUnitCreateModel, BloodUnit>().ReverseMap();
        //CreateMap<BloodUnitUpdateModel, BloodUnit>().ReverseMap();
        #endregion

        #region Course
        CreateMap<Course, CourseViewModel>().ReverseMap();
        CreateMap<Course, CourseCreateModel>().ReverseMap();
        CreateMap<Course, CourseUpdateModel>().ReverseMap();
        #endregion

        #region Department
        CreateMap<Department, DepartmentViewModel>().ReverseMap();
        CreateMap<Department, DepartmentCreateModel>().ReverseMap();
        CreateMap<Department, DepartmentUpdateModel>().ReverseMap();
        #endregion
    }
}
