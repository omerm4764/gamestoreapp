using GameStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace GameStoreApp.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public HomeController()
        {
            _db = new ApplicationDbContext();
        }
        public ActionResult Index(int? page)
        {
            //asp.net mvc for pagining
            //https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
            int pageSize = 5; //per page 5 items
            int pageNumber = (page ?? 1);
            var result = _db.Games.ToList();
            return View(result.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Search(string keyword)
        {

            if (string.IsNullOrEmpty(keyword))
                RedirectToAction("Index");

            ViewBag.keyword = keyword;
            var result = _db.Games.Where(x => x.Title.Contains(keyword) || x.Description.Contains(keyword) || x.Genre.Title.Contains(keyword)).ToList();
            return View(result);
        }
    }
}