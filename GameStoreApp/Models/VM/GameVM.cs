using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStoreApp.Models.VM
{
    public class GameVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public PegiRating PegiRating { get; set; }
        public int Count { get; set; }

        public ICollection<Genre> Genres { get; set; }
    }

    public class GameCreateOrUpdateVM
    {
        public GameCreateOrUpdateVM()
        {
            ReleaseDate = DateTime.Now;
        }
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public PegiRating PegiRating { get; set; }
        [Required]
        public int Count { get; set; }
        public string ImagePath { get; set; }
        [Required]
        public string GenreId { get; set; }
        public IEnumerable<SelectListItem> Genres { get; set; }
    }

    public enum PegiRating
    {
        three=3,
        seven=7,
        twelve=12,
        sixteen=16,
        eighteen=18
    }
}