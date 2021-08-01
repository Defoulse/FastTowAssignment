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
        public IActionResult Panel()
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

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article article = await _ft.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id, Title, Text")] Article article)
        {
            Article oldArticle = await _ft.Articles
                .FirstOrDefaultAsync(m => m.Id == article.Id);
            _ft.Entry(oldArticle).State = EntityState.Detached;

            if (ModelState.IsValid)
            {
                try
                {
                    _ft.Update(article);
                    await _ft.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Panel));
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _ft.Articles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var article = await _ft.Articles.FindAsync(id);
            _ft.Articles.Remove(article);
            await _ft.SaveChangesAsync();
            return RedirectToAction(nameof(Panel));
        }

        private bool ArticleExists(long id)
        {
            return _ft.Articles.Any(e => e.Id == id);
        }
    }
}
