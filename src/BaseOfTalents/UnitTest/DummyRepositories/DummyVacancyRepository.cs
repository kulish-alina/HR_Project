using Data.Migrations;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.DummyRepositories
{
    public class DummyVacancyRepository : DummyRepository<Vacancy>
    {
        public DummyVacancyRepository()
        {
            Collection = DummyData.Vacancies;
        }
    }
}
