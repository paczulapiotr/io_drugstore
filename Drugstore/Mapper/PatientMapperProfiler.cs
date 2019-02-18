using AutoMapper;
using Drugstore.Core;
using Drugstore.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Mapper
{
    public class PatientMapperProfiler:Profile
    {
        public PatientMapperProfiler()
        {
            CreateMap<Patient, PatientViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Department.ID))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
