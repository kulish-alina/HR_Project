using Domain.Entities;

namespace DAL.Mapping
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(u => u.FirstName);
            Property(u => u.MiddleName);
            Property(u => u.LastName);
            Property(u => u.isMale).IsRequired();
            Property(u => u.BirthDate);
            Property(u => u.Email).IsRequired();
            Property(u => u.Skype);
            Property(u => u.Login).IsRequired();

            HasRequired(u => u.Password);

            HasRequired(u => u.Role).WithMany().HasForeignKey(u => u.RoleId);
            HasOptional(u => u.City).WithMany().HasForeignKey(u => u.CityId);

            HasMany(x => x.UserComments).WithMany().Map(x =>
            {
                x.MapRightKey("CommentId");
                x.MapLeftKey("UserId");
                x.ToTable("UserToComment");
            });

            HasMany(u => u.PhoneNumbers).WithMany().Map(x =>
            {
                x.MapRightKey("PhoneNumberId");
                x.MapLeftKey("UserId");
                x.ToTable("UserToPhoneNumber");
            });

            HasOptional(u => u.Photo);
        }
    }
}