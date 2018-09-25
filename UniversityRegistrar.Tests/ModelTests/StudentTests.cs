using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace UniversityRegistrar.Tests
{
  [TestClass]
  public class StudentTests : IDisposable
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
    public StudentTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_registrar_test;";
    }
    [TestMethod]
    public void GetAll_DbStartsEmpty_0()
    {
      //Arrange
      //Act
      int result = Student.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }
    [TestMethod]
    public void Equals_ReturnsTrue_Student()
    {
      //Arrange, Act
      Student firstStudent = new Student("Hyewon Cho", new DateTime(2018, 9, 24, 9, 0, 30), 1);
      Student secondStudent = new Student("Hyewon Cho", new DateTime(2018, 9, 24, 9, 0, 30), 1);

      Student thirdStudent = new Student("Hyeryun Cho", new DateTime(2018, 9, 23, 9, 0, 30), 2);

      //Assert
      Assert.AreEqual(firstStudent, secondStudent);
      Assert.AreNotEqual(firstStudent, thirdStudent);
    }
    [TestMethod]
    public void Save_SaveToDatabase_List()
    {
      //Arrange
      Student firstStudent = new Student("Hyewon Cho", new DateTime(2018, 9, 24, 9, 0, 30));
      List<Student> expectedResult = new List<Student> {firstStudent};
      //Act
      firstStudent.Save();
      List<Student> result = Student.GetAll();
      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
    [TestMethod]
    public void GetAll_ReturnStudentsCorrectly_List()
    {
      //Arrange
      Student firstStudent = new Student("Hyewon Cho", new DateTime(2018, 9, 24, 9, 0, 30));
      firstStudent.Save();
      Student secondStudent = new Student("Hyeryun Cho", new DateTime(2018, 9, 23, 9, 0, 30));
      secondStudent.Save();
      List <Student> expectedStudents = new List <Student> {firstStudent, secondStudent};

      //Act
      List <Student> students = Student.GetAll();

      //Assert
      CollectionAssert.AreEqual(expectedStudents, students);
    }
    [TestMethod]
    public void Find_FindStudentInDatabase_Student()
    {
      //Arrange
      Student firstStudent = new Student("Hyewon Cho", new DateTime(2018, 9, 24, 9, 0, 30));
      firstStudent.Save();
      //Act
      Student foundStudent = Student.Find(firstStudent.Id);

      //Assert
      Assert.AreEqual(firstStudent, foundStudent);
    }

    [TestMethod]
    public void Update_UpdatesDatabase_String()
    {
      //Arrange
      Student firstStudent = new Student("Hyewon Cho", new DateTime(2018, 9, 24, 9, 0, 30));
      firstStudent.Save();
      Student firstStudentCopy = new Student("Hyewon Cho", new DateTime(2018, 9, 24, 9, 0, 30), firstStudent.Id);
      string newName = "Hyeryun Cho";

      //Act
      firstStudent.Update(newName);
      firstStudentCopy.Name = newName;

      //Assert
      Assert.AreEqual(firstStudentCopy, firstStudent);
    }
    [TestMethod]
    public void Delete_DeletesStudentFromDatabase_Student()
    {
      //Arrange
      Student firstStudent = new Student("Hyewon Cho", new DateTime(2018, 9, 24, 9, 0, 30));
      firstStudent.Save();
      Student secondStudent = new Student("Hyeryun Cho", new DateTime(2018, 9, 23, 9, 0, 30));
      secondStudent.Save();
      List<Student> expectedStudents = new List<Student>{firstStudent};
      //Act
      Student.Delete(secondStudent.Id);
      List<Student> students = Student.GetAll();
      //Assert
      CollectionAssert.AreEqual(expectedStudents, students);
    }
  }
}
