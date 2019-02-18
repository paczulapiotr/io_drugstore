using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Mapper
{
    public class MapperDependencyResolver
    {
        public static void Resolve()
        {

            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<XmlMedicineMapperProfiler>();
                a.AddProfile<MedicineMapperProfiler>();
                a.AddProfile<PatientMapperProfiler>();
                a.AddProfile<PrescriptionMapperProfiler>();
            });
        }
    }
}
