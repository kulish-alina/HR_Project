using Domain.Entities;

namespace DAL.Mapping
{
    public class CommentConfiguration : BaseEntityConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            Property(c => c.Message).IsOptional();
        }
    }
}