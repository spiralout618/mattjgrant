using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mattjgrant.Models;
using mattjgrant.DAL;
using mattjgrant.ViewModels;

namespace mattjgrant.Controllers
{
    [Authorize(Roles="Admin")]
    public class ChecklistController : Controller
    {
        private WebsiteContext context = new WebsiteContext();

        // GET: /Checklist/
        public ActionResult Index()
        {
            return View(context.Checklists.Where(c => c.IsActive).ToList());
        }

        [HttpGet]
        public ActionResult List(int checklistID, int? parentChecklistID)
        {
            var viewModel = GetListViewModel(checklistID);
            viewModel.ParentChecklistID = parentChecklistID;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ArchivedChecklists()
        {
            return View(context.Checklists.Where(c => !c.IsActive).ToList());
        }

        [HttpGet]
        public ActionResult ArchivedChecklist(int checklistID)
        {
            var checklist = context.Checklists.First(c => c.ChecklistID == checklistID);
            return View(new ChecklistViewModel(checklist));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Activate(int ChecklistID)
        {
            var checklist = context.Checklists.First(c => c.ChecklistID == ChecklistID);
            checklist.IsActive = true;
            context.SaveChanges();
            return RedirectToAction("List", new { checklistID = checklist.ChecklistID });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult List(ChecklistViewModel viewModel)
        {
            var checklist = context.Checklists.First(c => c.ChecklistID == viewModel.ChecklistID);
            viewModel.AddMetaData(checklist);
            viewModel.CopyToModel(checklist, context);
            context.SaveChanges();

            TempData["Success"] = "Checklist saved";

            return RedirectToAction("List", viewModel.ChecklistID);
            //return PartialView("List", viewModel);
        }

        [HttpGet]
        public ActionResult NestedChecklist(int checklistID)
        {
            var viewModel = new NestedChecklistViewModel();
            viewModel.AddMetaData(context);
            return View(viewModel);

        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NestedChecklist(NestedChecklistViewModel viewModel)
        {
            var checklist = context.Checklists.FirstOrDefault(c => c.ChecklistID == viewModel.ChecklistID);
            var nestedChecklist = context.Checklists.FirstOrDefault(c => c.ChecklistID == viewModel.NestedChecklistID);
            if (checklist == null || nestedChecklist== null)
                throw new Exception("No such checklist");
            checklist.ChecklistItems.Add(new ChecklistItem
            {
                ChecklistID = checklist.ChecklistID,
                NestedChecklistID = nestedChecklist.ChecklistID,
                State = ChecklistState.Unchecked,
                Name = nestedChecklist.Name
            });
            context.SaveChanges();
            return RedirectToAction("List", viewModel.ChecklistID);
        }

        private ChecklistViewModel GetListViewModel(int checklistID)
        {
            var checklist = context.Checklists.FirstOrDefault(c => c.ChecklistID == checklistID);
            if (checklist == null)
                throw new Exception("No such checklist");
            var viewModel = new ChecklistViewModel(checklist);
            viewModel.AddMetaData(checklist);
            return viewModel;
        }

        [HttpPost]
        public ActionResult Add(ChecklistItemViewModel viewModel)
        {
            //add metadata
            //Validate
            //Save to the database
            //Return list
            return PartialView("List", GetListViewModel(viewModel.NestedChecklistID.Value));
        }

        // GET: /Checklist/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checklist checklist = context.Checklists.Find(id);
            if (checklist == null)
            {
                return HttpNotFound();
            }
            return View(checklist);
        }

        // GET: /Checklist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Checklist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Name")] Checklist checklist)
        {
            checklist.CreatedDate = DateTime.Now;
            checklist.UpdatedDate = DateTime.Now;
            checklist.IsActive = true;
            if (ModelState.IsValid)
            {
                context.Checklists.Add(checklist);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(checklist);
        }

        // GET: /Checklist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checklist checklist = context.Checklists.Find(id);
            if (checklist == null)
            {
                return HttpNotFound();
            }
            return View(checklist);
        }

        // POST: /Checklist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ChecklistID,Name,IsActive,CreatedDate")] Checklist checklist)
        {
            if (ModelState.IsValid)
            {
                checklist.UpdatedDate = DateTime.Now;
                context.Entry(checklist).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(checklist);
        }

        // GET: /Checklist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checklist checklist = context.Checklists.Find(id);
            if (checklist == null)
            {
                return HttpNotFound();
            }
            return View(checklist);
        }

        // POST: /Checklist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Checklist checklist = context.Checklists.Find(id);

            context.ChecklistItems
                .Where(ci => ci.NestedChecklistID == checklist.ChecklistID).ToList()
                .ForEach(ci => { ci.NestedChecklistID = null; ci.Name = checklist.Name; });

            foreach (var checklistItem in checklist.ChecklistItems.ToList())
            {
                context.ChecklistItems.Remove(checklistItem);
            }

            context.Checklists.Remove(checklist);

            context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Copy(int checklistID)
        {
            var checklist = context.Checklists.Find(checklistID);
            return View(new CopyChecklistViewModel 
            { 
                OriginalChecklistID = checklist.ChecklistID, 
                OldName = checklist.Name 
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Copy(CopyChecklistViewModel viewModel)
        {
            var checklist = new Checklist();
            checklist.Name = viewModel.NewName;
            checklist.CreatedDate = DateTime.Now;
            checklist.UpdatedDate = DateTime.Now;
            checklist.IsActive = true;

            var checklistItems = context.ChecklistItems.Where(c => c.ChecklistID == viewModel.OriginalChecklistID).ToList().Select(ci => new ChecklistItem
            {
                Checklist = checklist,
                Name = ci.Name,
                NestedChecklistID = ci.NestedChecklistID,
                Order = ci.Order,
                State = ChecklistState.Unchecked
            });

            foreach (var item in checklistItems)
            {
                context.ChecklistItems.Add(item);
            }

            context.Checklists.Add(checklist);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
