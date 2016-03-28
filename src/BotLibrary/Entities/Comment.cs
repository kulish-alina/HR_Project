using BotLibrary.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class Comment : BaseEntity
    {
        public string Message { get; set; }
        public CommentType CommentType { get; set; }
    }
}
