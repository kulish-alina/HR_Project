using Domain.Entities.Enum;
using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.DTOModels
{
    public class CommentDTO : BaseEntityDTO
    {
        public string Message { get; set; }
    }
}
