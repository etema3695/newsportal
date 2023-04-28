using NewsPortal.BLL.Services;
using NewsPortal.Mapping;
using NewsPortal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewsPOrtal.DAL.ViewModels;
using CategoryViewModel = NewsPOrtal.DAL.ViewModels.CategoryViewModel;
using CategoryItem = NewsPOrtal.DAL.ViewModels.CategoryItem;
using NewsPOrtal.DAL.Models;

namespace NewsPortal.Controllers
{
    public class NewsPortalController : BaseController
    {
       
        private ArticleService articleService = new ArticleService();
        private CategoryService categoryService = new CategoryService();
        private CommentService commentService = new CommentService();

        // GET: NewsPortal
        public ActionResult Index(int page=0)
        {
            
            const int PageSize = 4;
            var pubArticles = articleService.GetArticlesOfNewsPortal();
            var count = articleService.CountArticles();

            this.ViewBag.MaxPage = articleService.GetMaxPageForArticles(count, PageSize);
            this.ViewBag.CurrentPage = page ;
            this.ViewBag.Page = page ;

            if (page > this.ViewBag.MaxPage)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadGateway);
            }

            return View(categoryService.GetCategoriesAndArticles(page,pubArticles.ToList()));
        }

        public ActionResult Category(int? id,int page=0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var categories = categoryService.GetAllCategories();

            var checkIsEgixt = categories.FirstOrDefault(x => x.Id == id);
            if (checkIsEgixt == null)
            {
                return HttpNotFound();
            }

            const int PageSize = 6;
            var pubArticlesByCat = articleService.GetArticlesOfSameCategoryToNewsPortal(id);
            var count =pubArticlesByCat.Count();

            this.ViewBag.MaxPage = articleService.GetMaxPageForArticles(count, PageSize);
            this.ViewBag.CurrentPage = page;
            this.ViewBag.Page = page;
            if (page > this.ViewBag.MaxPage)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadGateway);
            }

            
         

            return View("Category", categoryService.GetCategoriesAndArticlesOfSameCategory(id,page,pubArticlesByCat.ToList()));

        }

        public ActionResult Article(int id)
        {

           
            return View("Article",articleService.GetArticleOfNewsPortal(id));
        }

        public ActionResult CreateComment(CategoryViewModel model)
        {
            if (Request.IsAjaxRequest())
            {
                commentService.AddComment(model);
                return PartialView("_SingleComment",model);
            } else
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            
        }
        
        [Authorize(Roles ="SuperAdmin")]
        public ActionResult DeleteComment(int id)
        {

            Comment comment = commentService.FindCommentById(id);
            commentService.RemoveComment(comment);

            return RedirectToAction("Article",new { id = comment.Article_Id });
        }

        public ActionResult Contact()
        {
            var model = new CategoryViewModel();
            var categories = categoryService.GetAllCategories();

            categoryService.AddCategoriesToViewModel(model);

            return View(model);
        }

        public ActionResult About()
        {
            var model = new CategoryViewModel();
            var categories = categoryService.GetAllCategories();

            categoryService.AddCategoriesToViewModel(model);

            return View(model);
        }
    }

}