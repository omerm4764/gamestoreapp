using GameStoreApp.Models;
using GameStoreApp.Models.VM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStoreApp.Controllers
{
    [Authorize(Roles = "Administrator,Member")]
    public class GameController : Controller
    {
        private ApplicationDbContext _db { get; set; }
        private const string FILE_JPG = ".jpg";
        private const string IMAGE_FILES_PATH = "~/Content/images/";
        public GameController()
        {
            _db = new ApplicationDbContext();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var result = _db.Games.ToList();

            return View(result);
        }
        //[AllowAnonymous]
        //public ActionResult List()
        //{

        //    return View();
        //}
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var model = new GameCreateOrUpdateVM();
            model.Genres = _db.Genres.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id
            });
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(GameCreateOrUpdateVM model)
        {

            if (!ModelState.IsValid)
            {


                return View(model);
            }
            try
            {
                var newGame = new Game
                {
                    Title = model.Title,
                    Count = model.Count,
                    Description = model.Description,
                    GenreId = model.GenreId,
                    PegiRating = model.PegiRating,
                    Price = model.Price,
                    ReleaseDate = model.ReleaseDate,
                    ApplicationUserId = _db.Users.Single(x => x.UserName.Equals(User.Identity.Name)).Id
                };

                if (Request.Files != null && Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    var file = Request.Files[0];

                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath(IMAGE_FILES_PATH), fileName);
                    file.SaveAs(path);
                    newGame.ImagePath = fileName;
                }
                _db.Games.Add(newGame);
                _db.SaveChanges();

                ViewBag.Alert = new AlertInformation
                {
                    Title = "Succeed",
                    Message = string.Format("The {0} game created successfully.", newGame.Title),
                    AlertType = AlertType.success
                };
                return RedirectToAction("Index", "Game");
            }
            catch (Exception ex)
            {
                ViewBag.Alert = new AlertInformation
                {
                    Title = "Error",
                    Message = string.Format("There was an error while creating {0} game .", model.Title),
                    AlertType = AlertType.danger
                };
                return View(model);
            }
            finally
            {
                model.Genres = _db.Genres.Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.Id
                });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string Id)
        {
            var model = _db.Games.Single(x => x.Id == Id);

            var result = new GameCreateOrUpdateVM
            {
                Title = model.Title,
                Count = model.Count,
                Description = model.Description,
                GenreId = model.GenreId,
                PegiRating = model.PegiRating,
                Price = model.Price,
                ReleaseDate = model.ReleaseDate,
                Id = model.Id,
                ImagePath = model.ImagePath,
                Genres = _db.Genres.Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.Id
                }),
            };
            result.Genres = _db.Genres.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id
            });
            return View(result);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(GameCreateOrUpdateVM model)
        {
            model.Genres = _db.Genres.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id
            });
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var currentModel = _db.Games.Find(model.Id);
                var updated = new Game
                {
                    Title = model.Title,
                    Count = model.Count,
                    Description = model.Description,
                    GenreId = model.GenreId,
                    PegiRating = model.PegiRating,
                    Price = model.Price,
                    ReleaseDate = model.ReleaseDate,
                    ApplicationUserId = _db.Users.Single(x => x.UserName.Equals(User.Identity.Name)).Id,
                    UpdatedAt = DateTime.Now,
                    Id = model.Id,
                    ImagePath = currentModel.ImagePath
                };

                if (Request.Files != null && Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    var file = Request.Files[0];
                    if (!string.IsNullOrEmpty(updated.ImagePath))
                    {
                        var deleted = Path.Combine(Server.MapPath(IMAGE_FILES_PATH), updated.ImagePath);
                        System.IO.File.Delete(deleted);
                    }

                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath(IMAGE_FILES_PATH), fileName);
                        file.SaveAs(path);
                        updated.ImagePath = fileName;
                    }
                }

                _db.Entry(currentModel).CurrentValues.SetValues(updated);
                _db.SaveChanges();
                ViewBag.Alert = new AlertInformation
                {
                    Title = "Succeed",
                    Message = string.Format("The {0} game updated successfully.", model.Title),
                    AlertType = AlertType.success
                };
                return View("Index", _db.Games.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.Alert = new AlertInformation
                {
                    Title = "Error",
                    Message = string.Format("There was an error while updating {0} game .", model.Title),
                    AlertType = AlertType.success
                };
                return View(model);
            }
        }

        public ActionResult RentalGames()
        {
            return View(_db.RentalGames.ToList());
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Detail(string Id)
        {
            var result = _db.Games.Find(Id);

            return View(result);
        }
        public ActionResult RentTheGame(string Id)
        {
            var result = _db.Games.Find(Id);
            var currentUserAge = _db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Age;
            if (result.Count > 0)
            {
                if ((int)result.PegiRating > currentUserAge)
                {
                    ViewBag.Alert = new AlertInformation
                    {
                        Title = "Warning",
                        Message = string.Format("Your age is not enough to rent the game.If you want to rent the game you have to be over than or equal {0} ages.", result.PegiRating),
                        AlertType = AlertType.warning
                    };
                    return View("Detail", result);
                }
                result.Count -= 1;

                var rentedGame = new RentalGame
                {
                    RentedDate = DateTime.Now,
                    GameId = Id,
                    ApplicationUserId = _db.Users.Single(x => x.UserName.Equals(User.Identity.Name)).Id,
                };
                ViewBag.Alert = new AlertInformation
                {
                    Title = "Succeed",
                    Message = string.Format("Thanks for rented {0} game", result.Title),
                    AlertType = AlertType.success
                };

                _db.RentalGames.Add(rentedGame);
                _db.SaveChanges();
                return RedirectToAction("RentalGames", _db.Games.ToList());
            }

            ViewBag.Alert = new AlertInformation
            {
                Title = "Error",
                Message = string.Format("There is not enough stock item for {0} game", result.Title),
                AlertType = AlertType.danger
            };

            return View("Detail", result);
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string Id)
        {
            var deleted = _db.Games.Find(Id);

            try
            {
                var deletedRentedGames = _db.RentalGames.Where(x => x.GameId == deleted.Id).ToList();
                if (deletedRentedGames != null)
                {
                    foreach (var item in deletedRentedGames)
                    {
                        _db.RentalGames.Remove(item);
                    }
                }
                _db.Games.Remove(deleted);
                _db.SaveChanges();
                var deletedFile = Path.Combine(Server.MapPath(IMAGE_FILES_PATH), deleted.ImagePath);
                if (System.IO.File.Exists(deletedFile))
                    System.IO.File.Delete(deletedFile);
                ViewBag.Alert = new AlertInformation
                {
                    Title = "Succeed",
                    Message = string.Format("The {0} game deleted successfully.", deleted.Title),
                    AlertType = AlertType.success
                };
            }
            catch (Exception)
            {
                ViewBag.Alert = new AlertInformation
                {
                    Title = "Error",
                    Message = string.Format("There was an error while deleting {0} game .", deleted.Title),
                    AlertType = AlertType.success
                };
            }

            return View("Index", _db.Games.ToList());
        }
        [AllowAnonymous]
        public ActionResult Search(string keyword)
        {
            var result = _db.Games.Where(x => x.Title.ToLower().Contains(keyword.ToLower())).ToList();
            if (result == null)
            {
                ViewBag.Alert = new AlertInformation
                {
                    Title = "Error",
                    Message = string.Format("There is not a game equals with {0} keyword", keyword),
                    AlertType = AlertType.success
                };
            }
            return View("Index", result);
        }
    }
}