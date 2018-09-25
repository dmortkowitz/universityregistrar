using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace UniversityRegistrar.Tests
{
  [TestClass]
  public class CourseTests : IDisposable
  {
    public void Dispose()
    {
      Course.DeleteAll();
      Student.DeleteAll();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"ALTER TABLE students AUTO_INCREMENT = 1;";
      cmd.ExecuteNonQuery();
      cmd.CommandText = @"ALTER TABLE courses AUTO_INCREMENT = 1;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public CourseTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_registrar_test;";
    }
    [TestMethod]
    public void GetAll_DbStartsEmpty_0()
    {
      //Arrange
      //Act
      int result = Course.GetAll().Count;
      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_returnsTrue_Course()
    {
      //Arrange, Act
      Course firstCourse = new Course("Database", "CS210", 1);
      Course secondCourse = new Course("Database", "CS210", 1);

      //Assert
      Assert.AreEqual(firstCourse, secondCourse);
    }

    [TestMethod]
    public void Save_SaveToDatabase_List()
    {
      //Arrange
      Course firstCourse = new Course("Database", "CS210");
      List<Course> expectedResult = new List<Course> {firstCourse};
      //Act
      firstCourse.Save();
      List<Course> result = Course.GetAll();

      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public void GetAll_ReturnCourseCorrectly_List()
    {
      //Arrange
      Course firstCourse = new Course("Database", "CS210");
      firstCourse.Save();
      Course secondCourse = new Course("Discrete Mathematics", "CS220");
      secondCourse.Save();
      List<Course> expectedCourses = new List<Course> {firstCourse, secondCourse};
      //Act
      List<Course> courses = Course.GetAll();
      //Assert
      CollectionAssert.AreEqual(expectedCourses, courses);
    }
    [TestMethod]
    public void Find_FindCourseInDatabase_Course()
    {
      //Arrange
      Course firstCourse = new Course ("Database", "CS210");
      firstCourse.Save();
      //Act
      Course foundCourse = Course.Find(firstCourse.Id);
      //Assert
      Assert.AreEqual(firstCourse, foundCourse);
    }
    [TestMethod]
    public void Update_UpdatesDatabase_String()
    {
      //Arrange
      Course firstCourse = new Course("Database", "CS210");
      firstCourse.Save();
      Course firstCourseCopy = new Course("Database", "CS210", firstCourse.Id);
      string newName = "Discrete Mathematics";
      //Act
      firstCourse.Update(newName);
      firstCourseCopy.Name = newName;
      //Assert
      Assert.AreEqual(firstCourseCopy, firstCourse);
    }
  }
}
