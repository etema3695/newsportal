using System.Net;
using System.Web;
using System.Web.Mvc;
using NewsPortal.BLL.Services;
using NewsPortal.Models;
using NewsPortal.Mapping;
using NewsPortal.ViewModel;
using NewsPortal.Common.Models.Requests.Category;

namespace NewsPortal.Controllers
{
    [Authorize(Roles = "SuperAdmin,Journalist")]
    public class CategoriesController : BaseController
    {
  
        private readonly CategoryService categoryService = new CategoryService();
        private readonly ArticleService articleService = new ArticleService();

        // GET: Categories
        public ActionResult Index()
        {

            return View(categoryService.GetAllCategories());
        }


        // GET: Categories/Create
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create()
        {
            ViewBag.Parent_Id = new SelectList(categoryService.GetAllCategories(), "Id", "Code");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddCategory category)
        {

            var addCategory = categoryService.AddCategoryCustomValidation(category, User);
            if (!addCategory.IsSuccessfull)
            {
                AddCustomValidationToModelState(addCategory);
                ViewBag.Parent_Id = new SelectList(categoryService.GetAllCategories(), "Id", "Code");
                return View();
                

            }

            return RedirectToAction("Index");
        }


        // GET: Categories/SoftDelete/5
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult SoftDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = categoryService.FindCategoryById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/SoftDelete/5
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost, ActionName("SoftDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SoftDelete(int id)
        {
            var category = categoryService.FindCategoryById(id);
           if(categoryService.CheckIfCategoryIsEmpty(category))
            {
                categoryService.RemoveCategory(category);
            }


            else
            {
                return View("Empty");
            }


            return RedirectToAction("Index");
        }


        public ActionResult Code(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var category = categoryService.FindCategoryById(id);

            
            if (category == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Journalist"))
            {

                return View(articleService.GetArticlesOfUser(id,User));

            }

            return View(articleService.GetArticlesOfCategory(id));

        }

    }

}
