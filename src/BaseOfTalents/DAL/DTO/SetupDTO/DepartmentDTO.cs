using Domain.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.DTOModels.SetupDTO
{
    public class DepartmentDTO : BaseEntityDTO
    {
        public string Title { get; set; }
        public int DepartmentGroupId { get; set; }
    }
}

