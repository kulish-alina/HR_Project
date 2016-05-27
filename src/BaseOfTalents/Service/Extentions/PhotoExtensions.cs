using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Extentions
{
    public static class PhotoExtensions
    {
        public static void Update(this Photo destination, PhotoDTO source)
        {
            destination.Description = source.Description;
            destination.ImagePath = source.ImagePath;
            destination.State = source.State;
        }
    }
}
