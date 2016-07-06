using Domain.Entities;

namespace DAL.Mapping
{
    public class NoteConfiguration : BaseEntityConfiguration<Note>
    {
        public NoteConfiguration()
        {
            HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
