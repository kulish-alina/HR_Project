using System.Collections.Generic;

namespace Domain.DTO.DTOModels
{
    public class RoleDTO : BaseEntityDTO
    {
        public string Title { get; set; }
        public IEnumerable<int> PermissionIds { get; set; }
    }
}
