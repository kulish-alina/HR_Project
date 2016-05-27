using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Extentions
{
    public static class PhoneNumberExtension
    {
        public static void Update(this PhoneNumber destination, PhoneNumberDTO source)
        {
            destination.Number = source.Number;
            destination.State = source.State;
        }
    }
}
