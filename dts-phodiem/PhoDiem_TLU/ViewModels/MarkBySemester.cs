using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoDiem_TLU.ViewModels
{
    public class MarkBySemester
    {
        public MarkBySemester()
        {

        }
        public MarkBySemester(string _class_name, String _student_code, String _student_name, double _mark, double _mark_exam, double _mark_final, char _gpa, double _mark_gpa, String _note)
        {
            this.class_name = _class_name;
            this.student_code = _student_code;
            this.student_name = _student_name;
            this.mark = _mark<0?"Chưa nhập điểm":_mark.ToString();
            this.mark_exam = _mark_exam<0?"Chưa nhập điểm":_mark_exam.ToString();
            this.mark_final = _mark_final<0?"Chưa nhập điểm":_mark_final.ToString();
            this.gpa = _gpa;
            this.mark_gpa = _mark_gpa;
            this.note = _note;
        }

        public string class_name { get; set; }
        public string student_code { get; set; }
        public string student_name { get; set; }
        public string mark { get; set; }
        public string mark_exam { get; set; }
        public string mark_final { get; set; }
        public char gpa { get; set; }
        public double? mark_gpa { get; set; }
        public string note { get; set; }
        public long status { get; set; }

    }
}
