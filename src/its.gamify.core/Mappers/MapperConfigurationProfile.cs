using AutoMapper;

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
    }
}
