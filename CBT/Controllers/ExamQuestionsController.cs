using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CBT.Entities;

namespace CBT.Controllers
{
    public class ExamQuestionsController : Controller
    {
        private CBTEntities db = new CBTEntities();

        // GET: ExamQuestions
        public ActionResult Index()
        {
            var examQuestions = db.ExamQuestions.Include(e => e.Exam);
            return View(examQuestions.ToList());
        }

        // GET: ExamQuestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion = db.ExamQuestions.Find(id);
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            return View(examQuestion);
        }

        // GET: ExamQuestions/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Exams, "ID", "Name");
            return View();
        }

        // POST: ExamQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ExamId,Question,Answer, Point")] ExamQuestion examQuestion)
        {
            if (ModelState.IsValid)
            {
                db.ExamQuestions.Add(examQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Exams, "ID", "Name", examQuestion.Id);
            return View(examQuestion);
        }

        // GET: ExamQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion = db.ExamQuestions.Find(id);
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Exams, "ID", "Name", examQuestion.Id);
            return View(examQuestion);
        }

        // POST: ExamQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ExamId,Question,Answer")] ExamQuestion examQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Exams, "ID", "Name", examQuestion.Id);
            return View(examQuestion);
        }

        // GET: ExamQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion = db.ExamQuestions.Find(id);
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            return View(examQuestion);
        }

        // POST: ExamQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamQuestion examQuestion = db.ExamQuestions.Find(id);
            db.ExamQuestions.Remove(examQuestion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ExamQuestions(int id)
        {
            var q = db.ExamQuestions.Where(a => a.ExamId == id).ToListAsync();
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
