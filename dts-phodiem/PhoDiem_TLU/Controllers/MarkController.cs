﻿using OfficeOpenXml;
using OfficeOpenXml.Style;
using PhoDiem_TLU.Core;
using PhoDiem_TLU.DatabaseIO;
using PhoDiem_TLU.Helpers;
using PhoDiem_TLU.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace PhoDiem_TLU.Controllers
{
    public class MarkController : Controller
    {
        // GET: Mark
        private DBIO dBIO = new DBIO();
        private ExcelExport ex = new ExcelExport();

        //Khai báo log
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MarkController));
        public ActionResult Index()
        {
            setViewbag();
            return View();
        }
        private void setViewbag()
        {
            var subject = dBIO.getSubject();
            var years = dBIO.getYear();
            List<SelectListItem> provinces = new List<SelectListItem>();
            provinces.Add(new SelectListItem() { Text = "Hiển thị theo nhóm", Value = "HTN" });
            provinces.Add(new SelectListItem() { Text = "Hiển thị theo giáo viên", Value = "HTGV" });
            provinces.Add(new SelectListItem() { Text = "Hiển thị theo lớp quản lý", Value = "HTL" });

            List<SelectListItem> listOption = new List<SelectListItem>();
            listOption.Add(new SelectListItem() { Text = "Điểm quá trình", Value = "Điểm quá trình" });
            listOption.Add(new SelectListItem() { Text = "Điểm thi", Value = "Điểm thi" });
            listOption.Add(new SelectListItem() { Text = "Điểm tổng kết", Value = "Điểm tổng kết" });
            SelectList showOption = new SelectList(provinces, "Value", "Text");
            SelectList markOption = new SelectList(listOption, "Value", "Text");
            SelectList s = new SelectList(subject, "id", "subject_name");
            SelectList y = new SelectList(years, "id", "year");
            ViewBag.subject = s;
            ViewBag.years = y;
            ViewBag.showOption = showOption;
            ViewBag.markOption = markOption;
        }
        [HttpPost]

        public JsonResult MarkResult(long? subject_id, long? school_year_id_start, long? school_year_id_end, string showoption, string markOption)
        {

            try
            {

                if (subject_id != null && school_year_id_start != null && school_year_id_end != null && showoption != null && markOption != null)
                {

                    if (showoption == "HTN")
                    {
                        List<StudentCourseSubject> studentCourseSubjects = dBIO.getMarks_2(subject_id, school_year_id_start, school_year_id_end);

                        if (markOption == "Điểm quá trình")
                        {
                            var data = dBIO.getCourseSubjectData(studentCourseSubjects, 2);
                            var sumMark = dBIO.getSumMarks(data);
                            //var test = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end);
                            //var test1 = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end,3);
                            return Json(new
                            {
                                code = 200,
                                data,
                                showoption,
                                markOption,
                                sumMark,

                            }, JsonRequestBehavior.AllowGet);
                        }
                        else if (markOption == "Điểm thi")
                        {
                            var data = dBIO.getCourseSubjectData(studentCourseSubjects, 3);
                            var sumMark = dBIO.getSumMarks(data);
                            return Json(new
                            {
                                code = 200,
                                data,
                                showoption,
                                markOption,
                                sumMark,

                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Diem tong ket
                            var list = dBIO.getMarks(subject_id, school_year_id_start, school_year_id_end);
                            var data = dBIO.getCourseSubjectData(list);
                            var sumMark = dBIO.getSumMarks(data);
                            return Json(new
                            {
                                code = 200,
                                data,
                                showoption,
                                markOption,
                                sumMark,

                            }, JsonRequestBehavior.AllowGet);

                        }
                    }
                    else if (showoption == "HTGV")
                    {
                        List<StudentCourseSubject> studentCourseSubjects = dBIO.getMarks_2(subject_id, school_year_id_start, school_year_id_end);

                        if (markOption == "Điểm quá trình")
                        {

                            var data = dBIO.getMarkByTeacher(studentCourseSubjects, 2);
                            var sumMark = dBIO.getSumMarks(data);
                            return Json(new
                            {
                                code = 200,
                                data,
                                showoption,
                                markOption,
                                sumMark,

                            }, JsonRequestBehavior.AllowGet);
                        }
                        else if (markOption == "Điểm thi")
                        {
                            var data = dBIO.getMarkByTeacher(studentCourseSubjects, 3);
                            var sumMark = dBIO.getSumMarks(data);

                            return Json(new
                            {
                                code = 200,
                                data,
                                showoption,
                                markOption,
                                sumMark,

                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var list = dBIO.getMarks(subject_id, school_year_id_start, school_year_id_end);
                            var sublist = dBIO.getCourseSubjectData(list);
                            var data = dBIO.getMarkByTeacher(list);
                            var sumMark = dBIO.getSumMarks(data);
                            return Json(new
                            {
                                code = 200,
                                data,
                                showoption,
                                markOption,
                                sumMark,

                            }, JsonRequestBehavior.AllowGet);

                        }
                    }
                    else //Diem theo lop quan ly
                    {
                        if (markOption == "Điểm quá trình")
                        {
                            var data = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end, 2);
                            var sumMark = dBIO.getSumMarks(data);
                            //var test = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end);
                            //var test1 = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end,3);
                            return Json(new
                            {
                                code = 200,
                                data,
                                showoption,
                                markOption,
                                sumMark,

                            }, JsonRequestBehavior.AllowGet);
                        }
                        else if (markOption == "Điểm thi")
                        {
                            var data = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end, 3);
                            var sumMark = dBIO.getSumMarks(data);
                            return Json(new
                            {
                                code = 200,
                                data,
                                showoption,
                                markOption,
                                sumMark,

                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Diem tong ket
                            var data = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end);
                            var sumMark = dBIO.getSumMarks(data);
                            return Json(new
                            {
                                code = 200,
                                data,
                                showoption,
                                markOption,
                                sumMark,

                            }, JsonRequestBehavior.AllowGet);

                        }
                    }
                }
                return Json(new { code = 404 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return Json(new { code = 404, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = 404 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Export(long? subject_id, long? school_year_id_start, long? school_year_id_end, string showoption, string markOption)
        {
            if (subject_id != null && school_year_id_start != null &&
                school_year_id_end != null && showoption != null && markOption != null)
            {
                long startYear = dBIO.getYear(school_year_id_start);
                long EndYear = dBIO.getYear(school_year_id_end);
                string subjectName = dBIO.getSubject(subject_id);
                long numberOfCredit = dBIO.getNumberOfCredit(subject_id);
                if (showoption == "HTN")
                {
                    List<StudentCourseSubject> studentCourseSubjects
                        = dBIO.getMarks_2(subject_id, school_year_id_start, school_year_id_end);
                    List<Data> dataMark;
                    if (markOption == "Điểm thi")
                    {
                        dataMark = dBIO.getCourseSubjectData(studentCourseSubjects, 3);
                    }
                    else if (markOption == "Điểm quá trình")
                    {
                        dataMark = dBIO.getCourseSubjectData(studentCourseSubjects, 2);
                    }
                    else
                    {
                        var list = dBIO.getMarks(subject_id, school_year_id_start, school_year_id_end);
                        dataMark = dBIO.getCourseSubjectData(list);
                    }
                    var fileName = $"{markOption}_{subjectName}_{startYear}-{EndYear}.xlsx";
                    var data = ex.ExportExcelDataCourseSubject(markOption, subjectName, numberOfCredit, startYear, EndYear, dataMark);
                    return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
                else if (showoption == "HTGV")
                {
                    List<StudentCourseSubject> studentCourseSubjects
                        = dBIO.getMarks_2(subject_id, school_year_id_start, school_year_id_end);
                    List<MarkRate> dataMark;
                    if (markOption == "Điểm thi")
                    {
                        dataMark = dBIO.getMarkByTeacher(studentCourseSubjects, 3);
                    }
                    else if (markOption == "Điểm quá trình")
                    {
                        dataMark = dBIO.getMarkByTeacher(studentCourseSubjects, 2);
                    }
                    else
                    {
                        var list = dBIO.getMarks(subject_id, school_year_id_start, school_year_id_end);
                        var sublist = dBIO.getCourseSubjectData(list);
                        dataMark = dBIO.getMarkByTeacher(list);
                        
                    }
                    var fileName = $"{markOption}_{subjectName}_{startYear}-{EndYear}.xlsx";
                    var data = ex.ExportExcelDataTeacher(markOption, subjectName, numberOfCredit, startYear, EndYear, dataMark,1);
                    return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                }
                else
                {
                    List<MarksByEnrollmentClass> dataMark;
                    if (markOption == "Điểm quá trình")
                    {
                        dataMark = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end, 2);
                    }
                    else if (markOption == "Điểm thi")
                    {
                        dataMark = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end, 3);
                    }
                    else
                    {
                        //Diem tong ket
                        dataMark = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end);

                    }
                    var fileName = $"{markOption}_{subjectName}_{startYear}-{EndYear}.xlsx";
                    var data = ex.ExportExcelDataEnrollmentClass(markOption, subjectName, numberOfCredit, startYear, EndYear, dataMark);
                    return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            else return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ExportFileEnrollmentClass(long? subject_id, long? school_year_id_start, long? school_year_id_end,
            long? enrollmentClassID,string markOption)
        {
            try
            {
                List<MarksByEnrollmentClass> dataMark;
                long startYear = dBIO.getYear(school_year_id_start);
                long EndYear = dBIO.getYear(school_year_id_end);
                string subjectName = dBIO.getSubject(subject_id);
                long numberOfCredit = dBIO.getNumberOfCredit(subject_id);
                string enrollmentClassName = dBIO.getEnrollmentClassName(enrollmentClassID);
                if (markOption == "Điểm quá trình")
                {
                    dataMark = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end, 2, enrollmentClassID);
                }
                else if (markOption == "Điểm thi")
                {
                    dataMark = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end, 3, enrollmentClassID);
                }
                else
                {
                    //Diem tong ket
                    dataMark = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end,enrollmentClassID);

                }
                var fileName = $"{markOption}_{subjectName}_{enrollmentClassName}_{startYear}-{EndYear}.xlsx";
                var data = ex.ExportExcelDataEnrollmentClass(markOption, subjectName, numberOfCredit, enrollmentClassName, startYear, EndYear, dataMark);
                return Json(new {code = 200,fileName,enrollmentClassID, result = Convert.ToBase64String(data) },JsonRequestBehavior.AllowGet);
                //return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult ExportFileTeacher(long? subject_id, long? school_year_id_start, long? school_year_id_end,
            long? teacherID, string markOption)
        {
            try
            {
                List<StudentCourseSubject> studentCourseSubjects
                        = dBIO.getMarks_2(subject_id, school_year_id_start, school_year_id_end,teacherID);
                List<MarkRate> dataMark;
                long startYear = dBIO.getYear(school_year_id_start);
                long EndYear = dBIO.getYear(school_year_id_end);
                string subjectName = dBIO.getSubject(subject_id);
                string teacherName = dBIO.getTeacherName(teacherID);
                long numberOfCredit = dBIO.getNumberOfCredit(subject_id);
                if (markOption == "Điểm thi")
                {
                    dataMark = dBIO.getMarkByTeacher(studentCourseSubjects,teacherID, 3);
                }
                else if (markOption == "Điểm quá trình")
                {
                    dataMark = dBIO.getMarkByTeacher(studentCourseSubjects,teacherID, 2);
                }
                else
                {
                    var list = dBIO.getMarks(subject_id, school_year_id_start, school_year_id_end,teacherID);
                    dataMark = dBIO.getMarkByTeacher(list,teacherID);
                }
                var fileName = $"{markOption}_{subjectName}_{teacherName}_{startYear}-{EndYear}.xlsx";
                var data = ex.ExportExcelDataTeacher(markOption, subjectName, numberOfCredit, startYear, EndYear, dataMark,2);
                return Json(new { code = 200, fileName, result = Convert.ToBase64String(data) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult test1(long? subject_id, long? school_year_id_start, long? school_year_id_end,
            long? enrollmentClassID, string markOption)
        {
            
            List<MarksByEnrollmentClass> dataMark;
            long startYear = dBIO.getYear(school_year_id_start);
            long EndYear = dBIO.getYear(school_year_id_end);
            string subjectName = dBIO.getSubject(subject_id);
            long numberOfCredit = dBIO.getNumberOfCredit(subject_id);
            string enrollmentClassName = dBIO.getEnrollmentClassName(enrollmentClassID);
            if (markOption == "DQT")
            {
                dataMark = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end, 2, enrollmentClassID);
            }
            else if (markOption == "DT")
            {
                dataMark = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end, 3, enrollmentClassID);
            }
            else
            {
                //Diem tong ket
                dataMark = dBIO.getMarksEnrollmentClass(subject_id, school_year_id_start, school_year_id_end);

            }
            var fileName = $"{markOption}_{subjectName}_{enrollmentClassName}_{startYear}-{EndYear}.xlsx";
            var data = ex.ExportExcelDataEnrollmentClass(markOption, subjectName, numberOfCredit, enrollmentClassName, startYear, EndYear, dataMark);
            //return Json(new { code = 200, result = Convert.ToBase64String(data) }, JsonRequestBehavior.AllowGet);
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            

        }
        [HttpGet]
        public virtual ActionResult Download(byte[] file)
        {
            var fileName = "test.xlsx";
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        [HttpPost]
        public ActionResult ExportMarkByTeacher(int teacher, int semester, int courseYear, string markOption)
        {
            if (teacher != null && semester != null &&
                courseYear != null && markOption != null)
            {
                try
                {
                    List<MarkRate> list;
                    var teacherName = dBIO.getTeacherName(teacher);
                    var semesterName = dBIO.getSemester(semester);
                    var courseYearName = dBIO.getCourseYearName(courseYear);
                    string markOptionName = "";
                    if (markOption == "DTK")
                    {
                        list = dBIO.getRateMarkByTeacher(teacher, courseYear,semester);
                        markOptionName = "Điểm tổng kết";
                    }
                    else if(markOption == "DT")
                    {
                        list = dBIO.getRateMarkByTeacher(teacher, courseYear, semester,markOption);
                        markOptionName = "Điểm thi";
                    }
                    else
                    {
                        list = dBIO.getRateMarkByTeacher(teacher, courseYear, semester,markOption);
                        markOptionName = "Điểm quá trình";
                    }

                    var fileName = $"{markOptionName}_{teacherName}_{semesterName}_{courseYearName}.xlsx";
                    var data = ex.ExportExcelDataTeacher(markOptionName,teacherName,semesterName,courseYearName, list);
                    return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return RedirectToAction("MarkByTeacher");
                }
                
            }
            else return RedirectToAction("MarkByTeacher");
        }
        [HttpGet]
        public ActionResult MarkByTeacher()
        {
            log.Info("Info");
            setViewBagTeacher();
            setViewBagYear();
            setViewBagMarkOption();
            setViewBagCourseYear();
            return View();
        }
        [HttpPost]
        public JsonResult MarkByTeacher(int teacher, int semester,int courseYear,string markOption)
        {
            try
            {
                List<MarkRate> list ;
                if(markOption == "DTK") list = dBIO.getRateMarkByTeacher(teacher, courseYear, semester);
                else list = dBIO.getRateMarkByTeacher(teacher, courseYear, semester,markOption);

                var dataChart = dBIO.getSumMarks(list);
                return Json(new { code = 200,list,dataChart }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new {code = 500}, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult test()
        {
            try
            {
                var list = dBIO.getRateMarkByTeacher(4444, 2, 3);
                return Json(new { code = 200, list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { code = 500 }, JsonRequestBehavior.AllowGet);
            }
        }
        private void setViewBagYear()
        {
            var years = dBIO.getYear();
            SelectList y = new SelectList(years, "year", "year");
            ViewBag.years = y;
        }
        private void setViewBagTeacher()
        {
            var teacherName = dBIO.getListNameTeachers();
            
            SelectList y = new SelectList(teacherName, "id", "name");
            ViewBag.teacherName = y;
        }
        private void setViewBagMarkOption()
        {
            List<SelectListItem> listOption = new List<SelectListItem>();
            listOption.Add(new SelectListItem() { Text = "Điểm quá trình", Value = "DQT" });
            listOption.Add(new SelectListItem() { Text = "Điểm thi", Value = "DT" });
            listOption.Add(new SelectListItem() { Text = "Điểm tổng kết", Value = "DTK" });
            SelectList markOption = new SelectList(listOption, "Value", "Text");
            ViewBag.markOption = markOption;
        }
        private void setViewBagCourseYear()
        {
            var courseYear = dBIO.getCourseYear();
            SelectList s = new SelectList(courseYear, "id", "name");
            ViewBag.courseYear = s;
        }
        public JsonResult getSemester(int startYear,int endYear)
        {
            try
            {
                var semester = dBIO.getSemester(startYear,endYear);
                return Json(new {code = 200,semester},JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new {code = 500},JsonRequestBehavior.AllowGet);
            }
        }
    }
    
}