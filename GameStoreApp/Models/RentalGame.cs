using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStoreApp.Models
{
    /// <summary>
    /// Navigation property Rental Games for User
    /// </summary>
    public class RentalGame : EntityBase
    {
        public DateTime RentedDate { get; set; }
        public virtual Game Game { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual string ApplicationUserId { get; set; }
        public virtual string GameId { get; set; }
    }
}