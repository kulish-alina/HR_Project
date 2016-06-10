using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Mapping
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(u => u.FirstName).IsRequired();
            Property(u => u.MiddleName).IsRequired();
            Property(u => u.LastName).IsRequired();
            Property(u => u.isMale).IsRequired();
            Property(u => u.BirthDate);
            Property(u => u.Email).IsRequired();
            Property(u => u.Skype);
            Property(u => u.Login).IsRequired();
            Property(u => u.Password).IsRequired();

            HasRequired(u => u.Role).WithMany().HasForeignKey(u => u.RoleId);
            HasRequired(u => u.Location).WithMany().HasForeignKey(u => u.LocationId);

            HasMany(u => u.PhoneNumbers).WithMany().Map(x =>
            {
                x.MapRightKey("PhoneNumber_Id");
                x.MapLeftKey("User_Id");
                x.ToTable("UserPhoneNumber");
            });

            HasOptional(u => u.Photo).WithOptionalDependent();
        }
    }
}