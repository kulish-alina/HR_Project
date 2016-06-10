using BaseOfTalents.Domain.Entities;
using Domain.DTO.DTOModels;

namespace DAL.Extensions
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
