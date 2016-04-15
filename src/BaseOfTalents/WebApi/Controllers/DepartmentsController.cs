using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Controllers
{
    public class DepartmentsController : BoTController<Department, Department>
    {
        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _repo = departmentRepository;
        }
    }
}