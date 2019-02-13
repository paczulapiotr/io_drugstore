using AutoMapper;
using Drugstore.Core;
using Drugstore.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Mapper
{
    public class PrescriptionMapperProfiler :Profile
    {
        public PrescriptionMapperProfiler()
        {
            CreateMap<MedicalPrescription, PrescriptionViewModel>()
                .ForMember(dest => dest.VerificationState, opt => opt.MapFrom(src => src.VerificationState))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.Medicines.Sum(m => m.PricePerOne * m.AssignedQuantity)))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Patient.ID))
                .ForMember(dest => dest.AssignedMedicines, opt => opt.MapFrom(src => src
                    .Medicines.Select(m=>AutoMapper.Mapper.Map<MedicineViewModel>(m)).ToList()))
                .ForAllOtherMembers(opt => opt.Ignore());
        }

    }
}
