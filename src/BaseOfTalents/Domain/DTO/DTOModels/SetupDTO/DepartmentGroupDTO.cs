using System.Collections.Generic;

namespace Domain.DTO.DTOModels.SetupDTO
{
    public class DepartmentGroupDTO : BaseEntityDTO
    {
        public string Title { get; set; }
        public virtual ICollection<DepartmentDTO> Departments { get; set; }
    }
}
