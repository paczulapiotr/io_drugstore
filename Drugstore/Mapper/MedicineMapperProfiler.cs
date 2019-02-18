using AutoMapper;
using Drugstore.Core;
using Drugstore.Models.Shared;

namespace Drugstore.Mapper
{
    public class MedicineMapperProfiler : Profile
    {
        public MedicineMapperProfiler()
        {
            CreateMap<AssignedMedicine,MedicineViewModel>()
                .ForMember(dest => dest.MedicineCategory, opt => opt.MapFrom(src => src.StockMedicine.MedicineCategory))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockMedicine.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.StockMedicine.Name))
                .ForMember(dest => dest.PricePerOne, opt => opt.MapFrom(src => src.PricePerOne))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.AssignedQuantity))
                .ForMember(dest => dest.Refundation, opt => opt.MapFrom(src => src.StockMedicine.Refundation))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<MedicineOnStock, MedicineViewModel>()
               .ForMember(dest => dest.MedicineCategory, opt => opt.MapFrom(src => src.MedicineCategory))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.ID))
               .ForMember(dest => dest.PricePerOne, opt => opt.MapFrom(src => src.PricePerOne))
               .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
               .ForMember(dest => dest.Refundation, opt => opt.MapFrom(src => src.Refundation))
               .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<MedicineViewModel,AssignedMedicine>()
               .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.PricePerOne, opt => opt.MapFrom(src => src.PricePerOne))
               .ForMember(dest => dest.AssignedQuantity, opt => opt.MapFrom(src => src.Quantity))
               .AfterMap((src,dest)=>
               {
                   dest.StockMedicine = new MedicineOnStock
                   {
                       ID = src.StockId,
                       Name = src.Name,
                       Refundation = src.Refundation,
                       MedicineCategory = src.MedicineCategory
                   };
               })
               .ForAllOtherMembers(opt => opt.Ignore());

        }

    }
}
