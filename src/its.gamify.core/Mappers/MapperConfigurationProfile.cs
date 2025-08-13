using AutoMapper;
using its.gamify.core.Models.Categories;
using its.gamify.core.Models.Courses;
using its.gamify.core.Models.CourseSections;
using its.gamify.core.Models.Departments;
using its.gamify.core.Models.DifficultyLevels;
using its.gamify.core.Models.Quarters;
using its.gamify.core.Models.Questions;
using its.gamify.core.Models.QuizAnswers;
using its.gamify.core.Models.Quizes;
using its.gamify.core.Models.QuizResults;
using its.gamify.core.Models.Users;
using its.gamify.domains.Entities;

namespace its.gamify.core.Mappers;
public class MapperConfigurationProfile : Profile
{
    public MapperConfigurationProfile()
    {
        #region Users
        CreateMap<User, UserViewModel>()
            .ForMember(x => x.DeptName, cfg => cfg.MapFrom(x => (x.Department!.Name ?? string.Empty)))
            .ForMember(x => x.FullName, cfg => cfg.MapFrom(x => $"{x.LastName} {x.FirstName}"))
            .ForMember(x => x.RoleName, cfg => cfg.MapFrom(x => x.Role!.Name))
            .ReverseMap();
        CreateMap<User, UserUpdateModel>().ReverseMap();
        CreateMap<User, UserCreateModel>().ForMember(x => x.DeptId, cfg => cfg.MapFrom(x => x.DepartmentId))
            .ReverseMap();

        #endregion

        #region Course
        CreateMap<Course, CourseViewModel>().ReverseMap();
        CreateMap<Course, CourseCreateModels>().ReverseMap();
        CreateMap<Course, CourseUpdateModel>().ReverseMap();
        #endregion

        #region Department
        CreateMap<Department, DepartmentViewModel>().ForMember(x => x.EmployeeCount, cfg => cfg.MapFrom(c => c.Users!.Count))
                .ReverseMap();
        CreateMap<Department, DepartmentCreateModel>().ReverseMap();
        CreateMap<Department, DepartmentUpdateModel>().ReverseMap();
        #endregion

        CreateMap<Category, CategoryCreateModel>().ReverseMap();
        CreateMap<Category, CategoryViewModel>().ReverseMap();
        CreateMap<Category, CategoryUpdateModel>().ReverseMap();

        CreateMap<Quarter, QuarterCreateModel>().ReverseMap();
        CreateMap<CourseSection, CourseSectionCreateModel>().ReverseMap();
        CreateMap<Lesson, LessonCreateModel>().ReverseMap();
        CreateMap<Difficulty, DifficultyCreateModel>().ReverseMap();
        CreateMap<Quiz, QuizCreateModel>().ReverseMap();
        CreateMap<Quiz, QuizUpdateModel>().ReverseMap();
        CreateMap<Quiz, QuizViewModel>().ReverseMap();

        CreateMap<QuizResult, QuizResultCreateModel>().ReverseMap();
        CreateMap<QuizResult, QuizResultUpdateModel>().ReverseMap();
        CreateMap<QuizResult, QuizResultViewModel>().ReverseMap();
        CreateMap<Question, QuestionUpsertModel>().ReverseMap();

        CreateMap<QuizAnswer, QuizAnswerCreateModel>().ReverseMap();
        CreateMap<QuizAnswer, QuizAnswerUpdateModel>().ReverseMap();
        CreateMap<QuizAnswer, QuizAnswerViewModel>().ReverseMap();


    }
}
