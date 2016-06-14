using BaseOfTalents.Domain.Entities;
using Domain.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Extensions
{
    public static class CommentExtension
    {
        public static void Update(this Comment destination, CommentDTO source)
        {
            destination.Message = source.Message;
            destination.State = source.State;
        }
    }
}
