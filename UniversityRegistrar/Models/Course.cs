using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UniversityRegistrar;
using System;

namespace UniversityRegistrar.Models
{
  public class Course
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string CourseNumber { get; set; }

    public override bool Equals(System.Object otherCourse)
    {
      if(!(otherCourse is Course))
      {
        return false;
      }
      else
      {
      Course newCourse = (Course) otherCourse;
      bool idEquality = (this.Id == newCourse.Id);
      bool nameEquality = (this.Name == newCourse.Name);
      bool courseNumberEquality = (this.CourseNumber == newCourse.CourseNumber);
      return (idEquality && nameEquality && courseNumberEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.Name.GetHashCode();
    }

    public Course(string name, string coursenumber, int id = 0)
    {
      Name = name;
      CourseNumber = coursenumber;
      Id = id;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses (name, course_number) VALUES (@name, @course_number);";
      cmd.Parameters.AddWithValue("@name", this.Name);
      cmd.Parameters.AddWithValue("@course_number", this.CourseNumber);

      cmd.ExecuteNonQuery();
      this.Id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddStudent(int studentId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO student_course (student_id, course_id) VALUES (@studentId, @courseId);";

      cmd.Parameters.AddWithValue("@studentId", studentId);
      cmd.Parameters.AddWithValue("@courseId", this.Id);
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string course_number = rdr.GetString(2);

        Course newCourse = new Course(name, course_number, id);
        allCourses.Add(newCourse);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCourses;
    }

    public List<Student> GetAllStudents()
    {
      List <Student> allStudents = new List<Student>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT student_course.id, student_course.student_id, students.name AS student_name, students.enrollment_date, student_course.course_id, courses.name AS course_name FROM students JOIN student_course ON student_course.student_id = students.id JOIN courses ON student_course.course_id = courses.id WHERE student_course.course_id = @courseId;";

      // CREATE VIEW student_course_view AS SELECT student_course.id, student_course.student_id, students.name AS student_name, students.enrollment_date, student_course.course_id, courses.name AS course_name FROM students JOIN student_course ON student_course.student_id = students.id JOIN courses ON student_course.course_id = courses.id WHERE student_course.course_id = @courseId;

      cmd.Parameters.AddWithValue("@courseId", this.Id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int student_id = rdr.GetInt32(1);
        string student_name = rdr.GetString(2);
        DateTime student_enrollment_date = rdr.GetDateTime(3);
        Student foundStudent = new Student(student_name, student_enrollment_date, student_id);
        allStudents.Add(foundStudent);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

    public static Course Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses WHERE id=@id;";
      cmd.Parameters.AddWithValue("@id", id);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      Course foundCourse = new Course("", "");

      while (rdr.Read())
      {
        int idd = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string course_number = rdr.GetString(2);
        foundCourse.Id = idd;
        foundCourse.Name = name;
        foundCourse.CourseNumber = course_number;
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundCourse;
    }

    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE courses SET name = @name WHERE id=@id;";
      cmd.Parameters.AddWithValue("@name", newName);
      cmd.Parameters.AddWithValue("@id", this.Id);
      cmd.ExecuteNonQuery();
      this.Name = newName;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void Delete(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM courses WHERE id = @id;";
      cmd.Parameters.AddWithValue("@id", id);
      cmd.ExecuteNonQuery();

      cmd.CommandText = @"DELETE FROM student_course WHERE course_id = @id;";
      cmd.Parameters.AddWithValue("@id", id);
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM student_course;";
      cmd.ExecuteNonQuery();

      cmd.CommandText = @"DELETE FROM courses;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
