using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBT.Models
{
    public class QuestionAttemptVm
    {
    public int QuestionId { get; set; } 
    public int StudentId { get; set; } 
        public string Answer { get; set; } 
        public int ExamNo { get; set; }
        public int StudentExamNo { get; set; }

        public int CurrentPageNo { get; set; }
    }

    public class TextSimilarityApi
    {
        public DateTime timestamp { get; set; }
        public int time { get; set; }
        public string lang { get; set; }
        public int langConfidence { get; set; }
        public string text1 { get; set; }
        public string url1 { get; set; }
        public string text2 { get; set; }
        public string url2 { get; set; }
        public double similarity { get; set; }
    }
}