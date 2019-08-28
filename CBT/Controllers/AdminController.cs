using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBT.Entities;

namespace CBT.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private CBTEntities db = new CBTEntities();
        public ActionResult Index()
        {
            ViewBag.totalStudent = db.Students.Count();
            ViewBag.totalExam = db.Exams.Count();
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