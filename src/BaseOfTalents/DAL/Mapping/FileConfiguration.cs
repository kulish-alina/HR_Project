using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Mapping
{
    public class FileConfiguration : BaseEntityConfiguration<File>
    {
        public FileConfiguration()
        {
            Property(f => f.FilePath).IsRequired();
            Property(f => f.Description).IsOptional();
        }
    }
}