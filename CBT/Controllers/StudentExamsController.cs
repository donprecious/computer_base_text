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
    public class StudentExamsController : Controller
    {
        private CBTEntities db = new CBTEntities();

        // GET: StudentExams
        public ActionResult Index()
        {
            var studentExams = db.StudentExams.Include(s => s.Exam).Include(s => s.Student);
            return View(studentExams.ToList());
        } 

        public ActionResult Exams(int id)
        {
            var studentExams = db.StudentExams.Where(a => a.StudentId == id).Include(s => s.Exam).Include(s => s.Student);
            return View("Index", studentExams.ToList());
        }
        // GET: StudentExams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentExam studentExam = db.StudentExams.Find(id);
            if (studentExam == null)
            {
                return HttpNotFound();
            }
            return View(studentExam);
        }

        // GET: StudentExams/Create
        public ActionResult Create()
        {
            ViewBag.ExamId = new SelectList(db.Exams, "ID", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "Id", "FirstName");
            return View();
        }

        // POST: StudentExams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StudentId,ExamId")] StudentExam studentExam)
        {
            if (ModelState.IsValid)
            {
                db.StudentExams.Add(studentExam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExamId = new SelectList(db.Exams, "ID", "Name", studentExam.ExamId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "FirstName", studentExam.StudentId);
            return View(studentExam);
        }

        // GET: StudentExams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentExam studentExam = db.StudentExams.Find(id);
            if (studentExam == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExamId = new SelectList(db.Exams, "ID", "Name", studentExam.ExamId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "FirstName", studentExam.StudentId);
            return View(studentExam);
        }

        // POST: StudentExams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StudentId,ExamId")] StudentExam studentExam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentExam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExamId = new SelectList(db.Exams, "ID", "Name", studentExam.ExamId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "FirstName", studentExam.StudentId);
            return View(studentExam);
        }

        // GET: StudentExams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentExam studentExam = db.StudentExams.Find(id);
            if (studentExam == null)
            {
                return HttpNotFound();
            }
            return View(studentExam);
        }

        // POST: StudentExams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentExam studentExam = db.StudentExams.Find(id);
            db.StudentExams.Remove(studentExam);
            db.SaveChanges();
            return RedirectToAction("Index");
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
