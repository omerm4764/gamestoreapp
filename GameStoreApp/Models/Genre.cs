using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStoreApp.Models
{
    public class Genre : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Game> Games { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual string ApplicationUserId { get; set; }
    }
}