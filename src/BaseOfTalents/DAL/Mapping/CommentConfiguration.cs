using Domain.Entities;

namespace DAL.Mapping
{
    public class CommentConfiguration : BaseEntityConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            Property(c => c.Message).IsOptional();
            HasRequired(x => x.Author).WithMany(x => x.UserComments).HasForeignKey(x => x.AuthorId);
        }
    }
}