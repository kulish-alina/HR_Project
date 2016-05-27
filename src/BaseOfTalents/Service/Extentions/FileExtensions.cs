using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Extentions
{ 
    public static class FileExtensions
    {
        public static void Update(this File destination, FileDTO source)
        {
            destination.Description = source.Description;
        }
    }
}
