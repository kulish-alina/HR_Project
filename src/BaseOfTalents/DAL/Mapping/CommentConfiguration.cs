using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Mapping
{
    public class CommentConfiguration : BaseEntityConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            Property(c => c.Message).IsRequired();
        }
    }
}