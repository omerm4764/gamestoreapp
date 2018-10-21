using GameStoreApp.Models;
using GameStoreApp.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenreStoreApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GenreController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public GenreController()
        {
            _db = new ApplicationDbContext();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var result = _db.Genres.ToList();
            //ICollection<GenreVM> model = result.Select(x => new GenreVM
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Description = x.Description
            //}).ToList();
            return View(result);
        }
        //[AllowAnonymous]
        //public ActionResult List()
        //{

        //    return View();
        //}
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(GenreCreateOrUpdateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var newGenre = new Genre
                {
                    Title = model.Title,
                    Description = model.Description,
                    ApplicationUserId = _db.Users.Single(x => x.UserName.Equals(User.Identity.Name)).Id,
                };
                _db.Genres.Add(newGenre);
                _db.SaveChanges();
                return View("Index", _db.Genres.ToList());
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Edit(string Id)
        {
            var x = _db.Genres.Find(Id);
            var result = new GenreCreateOrUpdateVM
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            };
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(GenreCreateOrUpdateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var currentModel = _db.Genres.Find(model.Id);
                var updated = new Genre
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    ApplicationUserId = _db.Users.Single(x => x.UserName.Equals(User.Identity.Name)).Id,
                };

                _db.Entry(currentModel).CurrentValues.SetValues(updated);
                _db.SaveChanges();
                return View("Index", _db.Genres.ToList());
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        //[HttpPost]
        //public ActionResult Detail(string Id)
        //{
        //    var result = _db.Genres.Find(Id);

        //    return View(result);
        //}
        public ActionResult Delete(string Id)
        {
            var deleted = _db.Genres.Find(Id);
            try
            {
                _db.Genres.Remove(deleted);
                _db.SaveChanges();
                return View("Index", _db.Genres.ToList());
            }
            catch (Exception ex)
            {
                return View("Index", _db.Genres.ToList());
            }
        }
        public ActionResult Search(string keyword)
        {
            var result = _db.Genres.Where(x => x.Title.ToLower().Contains(keyword.ToLower())).Select(x => new GenreVM
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).ToList();

            return View("Index", result);
        }
    }
}