using System.Net;
using System.Web;
using System.Web.Mvc;
using NewsPortal.Models;
using System.IO;
using NewsPortal.BLL.Services;
using NewsPortal.Common.Models.Requests.Article;
using NewsPOrtal.DAL.Models;
using NewsPOrtal.DAL.Mapping;
using System.ComponentModel.DataAnnotations;
using System;
using NewsPortal.Common.Models;
using System.Collections.Generic;

namespace NewsPortal.Controllers
{
    [Authorize(Roles = "SuperAdmin,Journalist")]
    public class ArticlesController : BaseController
    {
        private readonly ArticleService articleService = new ArticleService();
        private readonly CategoryService categoryService = new CategoryService();
        
        
        

        public ArticlesController()
        {

        }
        

        // GET: Articles
        public ActionResult Index()
        {
            var article = articleService.GetAllArticles(User);
            if (!article.IsSuccessfull)
            {

                throw new ArgumentNullException();
            }
            return View(article.Result);
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var article = articleService.GetArticle(id);
           
            if (!article.IsSuccessfull)
            {
                throw new ArgumentNullException();
            }
            

            if (articleService.CheckIfArticleExistsForUser(article.Result, User) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(article.Result);
        }

        // GET: Articles/Create
        public ActionResult Create()
        { 
            ViewBag.Category_Id = new SelectList(categoryService.GetAllCategories(), "Id", "Code");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,Body,ImageTitle,Category_Id")] AddArticle article)
        {

            var addArticle = articleService.AddArticleCustomValidation(article, User);
            if (!addArticle.IsSuccessfull)
            {
                AddCustomValidationToModelState(addArticle);
            }

            //if (ModelState.IsValid)
            //{
            //    articleService.AddArticle(article, User);
            //    return RedirectToAction("Index");
            //}

            ViewBag.Category_Id = new SelectList(categoryService.GetAllCategories(), "Id", "Code", article.Category_Id);
            return View();
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = articleService.FindArticleById(id);

            if (articleService.CheckIfArticleExistsForUser(article, User) == false)
            {
                return HttpNotFound();
            }

            if (article.IsPublished == true)
            {
                if (articleService.CheckIfUserHasAcces(article,User)==false)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }
            ViewBag.Category_Id = new SelectList(categoryService.GetAllCategories(), "Id", "Code", article.Category_Id);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Body,CreatedOn,CreatedBy,Category_Id,IsPublished,ImagePath,ImageTitle")] NewsPOrtal.DAL.Models.Article article , HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                

                articleService.EditArticle(article, file, Server);
                return RedirectToAction("Index");

            }
            ViewBag.Category_Id = new SelectList(categoryService.GetAllCategories(), "Id", "Code", article.Category_Id);
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = articleService.GetArticle(id);
           

            if (articleService.CheckIfArticleExistsForUser(article.Result, User) == false)
            {
                return HttpNotFound();
            }

            if (article.Result.IsPublished == true)
            {
                if (articleService.CheckIfUserHasAcces(article.Result, User) == false)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }
            return View(article.Result);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            

            var article=articleService.FindArticleById(id);
            
            articleService.RemoveArticle(article);
           
            return RedirectToAction("Index");
        }

       
        [Authorize(Roles ="SuperAdmin")]
        public ActionResult Publish(int id)
        {

            articleService.PublishArticle(id);

            return RedirectToAction("Index");
        }



    }
}
