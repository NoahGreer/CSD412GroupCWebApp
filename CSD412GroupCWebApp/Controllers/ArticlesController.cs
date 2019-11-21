using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSD412GroupCWebApp.Data;
using CSD412GroupCWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CSD412GroupCWebApp
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public ArticlesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Articles
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Article.Include(a => a.Author).Include(a => a.Categories);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Articles/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Author)
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public async Task<IActionResult> Create()
        {
            var categoryOptions = await _context.Category.ToListAsync();
            ArticleViewModel articleViewModel = new ArticleViewModel
            {
                Article = new Article(),
                CategoryOptions = categoryOptions
            };

            return View(articleViewModel);
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleViewModel articleViewModel)
        {
            if (ModelState.IsValid)
            {
                articleViewModel.Article.AuthorId = _userManager.GetUserId(User);
                articleViewModel.Article.Categories = _context.Category.Where(c => articleViewModel.SelectedCategoryIds.Contains(c.Id)).ToList();
                _context.Add(articleViewModel.Article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(articleViewModel);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            var selectedCategoryIds = article.Categories.Select(x => x.Id).ToArray();

            var categoryOptions = await _context.Category.ToListAsync();
            ArticleViewModel articleViewModel = new ArticleViewModel
            {
                Article = article,
                SelectedCategoryIds = selectedCategoryIds,
                CategoryOptions = categoryOptions
            };

            return View(articleViewModel);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ArticleViewModel articleViewModel)
        {
            if (id != articleViewModel.Article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                articleViewModel.Article.Categories = _context.Category.Where(c => articleViewModel.SelectedCategoryIds.Contains(c.Id)).ToList();

                try
                {
                    _context.Update(articleViewModel.Article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(articleViewModel.Article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(articleViewModel);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var article = await _context.Article.FindAsync(id);
            _context.Article.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(long id)
        {
            return _context.Article.Any(e => e.Id == id);
        }
    }
}
