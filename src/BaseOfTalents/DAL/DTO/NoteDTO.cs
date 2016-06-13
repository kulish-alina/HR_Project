using Domain.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class NoteDTO : BaseEntityDTO
    {
        public string Message { get; set; }
        public int UserId { get; set; }
    }
}
