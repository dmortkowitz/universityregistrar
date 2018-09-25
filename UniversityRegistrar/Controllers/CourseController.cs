using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System;
using System.Collections.Generic;

namespace UniversityRegistrar.Controllers
{
    public class CourseController : Controller
    {
      [HttpGet("/courses")]
      public ActionResult Index()
      {
        List <Course> allCourses = Course.GetAll();
        return View(allCourses);
      }
      [HttpGet("/courses/{courseId}")]
      public ActionResult Details(int courseId)
      {
        Dictionary<string, object> dict = new Dictionary<string, object> ();
        Course foundCourse = Course.Find(courseId);
        List <Student> allStudents = foundCourse.GetAllStudents();
        dict.Add("course", foundCourse);
        dict.Add("students", allStudents);
        return View(dict);
      }
      [HttpGet("/courses/new")]
      public ActionResult CreateForm()
      {
        return View();
      }
      [HttpPost("/courses")]
      public ActionResult Create(string courseName, string courseNumber)
      {
        Course newCourse = new Course(courseName, courseNumber);
        newCourse.Save();
        return RedirectToAction("Index");
      }
      [HttpGet("/courses/edit/{courseId}")]
      public ActionResult UpdateForm(int courseId)
      {
        Course foundCourse = Course.Find(courseId);
        return View(foundCourse);
      }
      [HttpPost("/courses/edit/{courseId}")]
      public ActionResult Update(int courseId, string newCourseName)
      {
        Course foundCourse = Course.Find(courseId);
        foundCourse.Update(newCourseName);
        return RedirectToAction("Index");
      }
      [HttpGet("/courses/{courseId}/students/new")]
      public ActionResult AddStudent(int courseId)
      {
        Course foundCourse = Course.Find(courseId);
        return View(foundCourse);
      }
      [HttpPost("/courses/{courseId}/students/new")]
      public ActionResult AddStudent(int courseId, int student_id)
      {
        Course foundCourse = Course.Find(courseId);
        foundCourse.AddStudent(student_id);
        return RedirectToAction("Details", new {courseId = courseId});
      }
      [HttpPost("/courses/delete/{courseId}")]
      public ActionResult Delete(int courseId)
      {
        Course.Delete(courseId);
        return RedirectToAction("Index");
      }
    }
}
