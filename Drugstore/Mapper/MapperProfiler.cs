using AutoMapper;
using Drugstore.Core;
using Drugstore.Models.Seriallization;

namespace Drugstore
{
    public class MapperProfiler : Profile
    {
        public MapperProfiler()
        {
            CreateMap<MedicineOnStock, XmlMedicineModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsRefunded, opt => opt.MapFrom(src => src.IsRefunded))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.MedicineCategory))
                .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.ID))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<XmlMedicineModel, MedicineOnStock>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.IsRefunded, opt => opt.MapFrom(src => src.IsRefunded))
               .ForMember(dest => dest.MedicineCategory, opt => opt.MapFrom(src => src.Category))
               .ForMember(dest => dest.PricePerOne, opt => opt.MapFrom(src => src.PricePerOne))
               .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
               .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.StockId))
               .ForAllOtherMembers(opt => opt.Ignore());
        }

        public static void Run()
        {
            Mapper.Initialize(a =>
            {
                a.AddProfile<MapperProfiler>();
            });
        }
    }
}
