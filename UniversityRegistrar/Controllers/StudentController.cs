using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System;
using System.Collections.Generic;

namespace UniversityRegistrar.Controllers
{
  public class StudentController : Controller
  {
    [HttpGet("/students")]
    public ActionResult Index()
    {
      List<Student> allStudents = Student.GetAll();
      return View(allStudents);
    }
    [HttpGet("/students/{studentId}")]
    public ActionResult Details(int studentId)
    {
      Dictionary<string, object> dict = new Dictionary<string, object> ();
      Student foundStudent = Student.Find(studentId);
      List <Course> courses = foundStudent.GetAllCourses();
      dict.Add("student", foundStudent);
      dict.Add("courses", courses);
      return View(dict);
    }
    [HttpGet("/students/new")]
    public ActionResult CreateForm()
    {
      return View();
    }
    [HttpPost("/students")]
    public ActionResult Create(string studentName, DateTime studentEnrollmentDate)
    {
      Student newStudent = new Student(studentName, studentEnrollmentDate);
      newStudent.Save();
      return RedirectToAction("Index");
    }
  }
}
