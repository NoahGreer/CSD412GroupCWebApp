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
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace CSD412GroupCWebApp
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<ArticlesController> _logger;
        public ArticlesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<ArticlesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Articles
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Article.Include(a => a.Author).Include(a => a.Categories);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: api/Articles
        [HttpGet]
        [Route("api/[controller]")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticle()
        {
            var articles = _context.Article    
                .Include(a => a.Author)
                .Include(a => a.Categories)
                .Where(a => a.IsPublished) 
                .AsQueryable();

            var articlesList = await articles.ToListAsync();

            //Reassign article author with only the name field populated to hide sensitive information 
            foreach (var article in articlesList)
            {
                article.Author = new ApplicationUser()
                {
                    Name = article.Author.Name
                };
            }
            return articlesList;
        }

        // GET: Articles/Details/5
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

        // GET: Articles/Display/article-url-slug
        [AllowAnonymous]
        public async Task<IActionResult> Display(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Author)
                .Include(a => a.Categories)
                .Where(a => a.IsPublished)
                .FirstOrDefaultAsync(a => a.UrlSlug == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public async Task<IActionResult> Create()
        {
            var article = new Article()
            {
                AuthorId = _userManager.GetUserId(User),
                Title = "Draft " + DateTime.Now
            };

            _context.Article.Add(article);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = article.Id });
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

            if (UserIsAdminOrArticleOwner(article))
            {
                return Forbid();
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

            var article = await _context.Article
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            if (UserIsAdminOrArticleOwner(article))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                article.Categories = _context.Category.Where(c => articleViewModel.SelectedCategoryIds.Contains(c.Id)).ToList();

                var editedArticle = articleViewModel.Article;
                article.Title = editedArticle.Title;
                article.Content = editedArticle.Content;

                if (!article.IsPublished && editedArticle.IsPublished)
                {
                    article.UrlSlug = Slugify(article.Title);
                    article.DatePosted = DateTime.Now;

                    var link = Url.RouteUrl(new { controller = "Articles", action = "Display", id = article.UrlSlug });
                    string baseUrl = "https://csd412groupcwebapp.azurewebsites.net";
                    link = baseUrl + link;

                    if (Startup.TwitterCredentials != null)
                    {
                        var twitterClient = new TwitterClient(Startup.TwitterCredentials);
                        string message = "Come see our latest post at:";

                        twitterClient.SetMessage(message);
                        twitterClient.SetLink(link);

                        await twitterClient.SendTweet();
                    } 
                    else
                    {
                        _logger.LogError("Twitter credentials not available. Could not send tweet");   
                    }
                }
                else if (article.IsPublished && !editedArticle.IsPublished)
                {
                    article.UrlSlug = null;
                    article.DatePosted = null;
                }

                article.IsPublished = editedArticle.IsPublished;

                try
                {
                    _context.Update(article);
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

            if (UserIsAdminOrArticleOwner(article))
            {
                return Forbid();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var article = await _context.Article
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (UserIsAdminOrArticleOwner(article))
            {
                return Forbid();
            }

            article.Categories.Clear();
            _context.Update(article);

            _context.Article.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(long id)
        {
            return _context.Article.Any(e => e.Id == id);
        }

        private bool UserIsAdminOrArticleOwner(Article article)
        {
            var isAuthorized = User.IsInRole(Constants.OwnerRole) ||
                               User.IsInRole(Constants.AdministratorRole);

            var currentUserId = _userManager.GetUserId(User);

            return !isAuthorized && currentUserId != article.AuthorId;
        }

        // Borrowed slugification logic from https://webmasters.stackexchange.com/a/1194
        private static string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        // Borrowed slugification logic from https://webmasters.stackexchange.com/a/1194
        private static string Slugify(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Remove all non valid chars          
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            str = Regex.Replace(str, @"\s", "-"); // //Replace spaces by dashes
            return str;
        }
    }
}
