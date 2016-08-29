using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class LanguageConfiguration : BaseEntityConfiguration<Language>
    {
        public LanguageConfiguration()
        {
            Property(x => x.Title).IsRequired();
        }
    }
}
