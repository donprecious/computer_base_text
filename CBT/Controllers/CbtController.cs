using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBT.Entities;
using CBT.Models;
using PagedList;
using RestSharp;
using RestSharp.Serialization.Json;

namespace CBT.Controllers
{
    public class CbtController : Controller
    {
        // GET: Cbt
        private CBTEntities db = new CBTEntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ExamLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExamLogin(int examCode)
        {
           
            var studentExam = db.StudentExams.Where(a => a.ID == examCode).SingleOrDefault();
            if (studentExam != null)
            {
                Session["examCode"] = studentExam.ID;
                Session["studentExam"] = studentExam;
                return RedirectToAction("StartExam");
            }

            ViewBag.error = "Invalid pin number";
            return View();
        }

        public ActionResult StartExam(int? page, string error=null)
        { 
            var examNo = Convert.ToInt32( Session["examCode"]);
            if (error != null)
            {
                ViewBag.error = error;
            }
        if (examNo != 0)
            {
                var studentExam = db.StudentExams.Where(a => a.ID == examNo).Include(a => a.Exam).Include(a => a.Exam.ExamQuestions).Include(a => a.Student.StudentAnswers).Include(a => a.Student).SingleOrDefault();
                int pageSize = 1;
                int pageNumber = (page ?? 1);
                return View(studentExam.Exam.ExamQuestions.ToPagedList(pageNumber, pageSize));
               
            }

            return RedirectToAction("ExamLogin");
        }

        [HttpPost]
        public ActionResult StartExam(QuestionAttemptVm m)
        {
            var question = db.ExamQuestions.Where(a => a.Id == m.QuestionId).SingleOrDefault();

            var stanswer = db.StudentAnswers.Where(a => a.QuestionId == m.QuestionId && a.StudentExamNo == m.StudentExamNo).FirstOrDefault();
            if(stanswer != null)
            {
                // // has already attemted this question  
                  return RedirectToAction("StartExam", new { page = m.CurrentPageNo, error = "You Have Already Answered this question please answer another one" });

            }
            if (question != null)
            {
                var client = new RestClient("https://api.dandelion.eu/datatxt/sim/v1");
                var request = new RestRequest();
                request.AddParameter("text1", m.Answer);
                request.AddParameter("text2", question.Answer);
                request.AddParameter("token", "19512dc20d8549b592425d177fc0f132");

                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JsonDeserializer deserial = new JsonDeserializer();
                    var obj = deserial.Deserialize<TextSimilarityApi>(response);
                    bool correct = false;
                    if(obj.similarity >= 0.400)
                    {
                        correct = true;
                    }

                    var score = Convert.ToDecimal(obj.similarity) *  question.Point;
                    var answer = new StudentAnswer
                    {
                        Answer = m.Answer,
                        IsCorrect = correct,
                        PrecisionLevel = Convert.ToDecimal(obj.similarity),
                        QuestionId = m.QuestionId,
                        StudentExamNo = m.StudentExamNo,
                        StudentId = m.StudentId,
                        Score = score
                    };
                    db.StudentAnswers.Add(answer);
                    db.SaveChanges();
                    var next = m.CurrentPageNo + 1;
                    return RedirectToAction("StartExam", new { page = next});
                }
                    return RedirectToAction("StartExam", new { page = m.CurrentPageNo, error="Unable to submit request"});

            }

            return RedirectToAction("StartExam", new { page = m.CurrentPageNo, error = "Something Went Wrong" });




        } 
        public ActionResult ViewResult()
        {
            return View();
        }
        public ActionResult PortalLogin(int id)
        {
            var studentExam = db.StudentExams.Where(a => a.ID == id).SingleOrDefault();
            if (studentExam != null)
            {
                Session["examCode"] = studentExam.ID;
                Session["studentExam"] = studentExam;
                return View();
            }
            return View("error");
        } 

        public ActionResult ExamResult(int id)
        {
            var result = db.StudentAnswers.Where(a => a.StudentExamNo == id).Include(a=>a.Student).Include(a=>a.ExamQuestion).ToList();
            return View(result);
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