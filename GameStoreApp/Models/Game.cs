using GameStoreApp.Models.VM;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace GameStoreApp.Models
{
    public class Game : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public PegiRating PegiRating { get; set; }
        public int Count { get; set; }
        public string ImagePath { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual string ApplicationUserId { get; set; }
        public virtual string GenreId { get; set; }
    }
}