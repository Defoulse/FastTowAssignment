using FastTowAssignment.Data;
using FastTowAssignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastTowAssignment.Controllers
{
    public class ArticleController : Controller
    {
        private readonly FastTowAssignmentContext _ft;

        public ArticleController(FastTowAssignmentContext ft)
        {
            _ft = ft;
        }

        public IActionResult Index()
        {
            var articles = _ft.Articles.OrderByDescending(a => a.CreationDate);
            return View(articles);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {            
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] Article articleData)
        {
            Article article = new Article();
            if (ModelState.IsValid)
            {
                try
                {
                    article.Title = articleData.Title;
                    article.Text = articleData.Text;
                    article.CreationDate = DateTime.Now;

                    _ft.Add(article);
                    await _ft.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(articleData);
        }
    }
}
