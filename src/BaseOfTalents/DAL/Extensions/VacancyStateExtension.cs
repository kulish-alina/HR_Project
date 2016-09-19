using DAL.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Extensions
{
    public static class VacancyStateExtension
    {
        public static void Update(this VacancyState destination, VacancyStateDTO source)
        {
            destination.VacancyId = source.VacancyId;
            destination.Passed = source.Passed;
        }
    }
}
