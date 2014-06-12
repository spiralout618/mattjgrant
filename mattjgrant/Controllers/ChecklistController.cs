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
    public class ChecklistController : Controller
    {
        private ChecklistContext context = new ChecklistContext();

        // GET: /Checklist/
        public ActionResult Index()
        {
            return View(context.Checklists.ToList());
        }

        [HttpGet]
        public ActionResult List(int checklistID)
        {
            var viewModel = GetListViewModel(checklistID);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult List(ChecklistViewModel viewModel)
        {
            var checklist = context.Checklists.First(c => c.ChecklistID == viewModel.ChecklistID);
            viewModel.AddMetaData(checklist);
            viewModel.CopyToModel(checklist, context);
            context.SaveChanges();

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
            if (checklist == null)
                throw new Exception("No such checklist");
            checklist.ChecklistItems.Add(new ChecklistItem
            {
                ChecklistID = viewModel.ChecklistID,
                NestedChecklistID = viewModel.NestedChecklistID,
                State = ChecklistState.Unchecked,
                Name = checklist.Name
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
        public ActionResult Create([Bind(Include="ChecklistID,Name,IsActive,CreatedDate,UpdatedDate")] Checklist checklist)
        {
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
        public ActionResult Edit([Bind(Include="ChecklistID,Name,IsActive,CreatedDate,UpdatedDate")] Checklist checklist)
        {
            if (ModelState.IsValid)
            {
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
            context.Checklists.Remove(checklist);
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
