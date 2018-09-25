using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UniversityRegistrar;
using System;

namespace UniversityRegistrar.Models
{
  public class Student
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime EnrollmentDate { get; set; }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = (this.Id == newStudent.Id);
        bool nameEquality = (this.Name == newStudent.Name);
        bool enrollmentDateEquality = this.EnrollmentDate.Equals(newStudent.EnrollmentDate);
        return (idEquality && nameEquality && enrollmentDateEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.Name.GetHashCode();
    }

    public Student (string name, DateTime enrollmentdate, int id = 0)
    {
      Name = name;
      EnrollmentDate = enrollmentdate;
      Id = id;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students (name, enrollment_date) VALUES (@name, @enrollment_date);";
      cmd.Parameters.AddWithValue("@name", this.Name);
      cmd.Parameters.AddWithValue("@enrollment_date", this.EnrollmentDate);
      cmd.ExecuteNonQuery();
      this.Id = (int)cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student> {};

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        DateTime enrollmentdate = rdr.GetDateTime(2);

        Student newStudent = new Student(name, enrollmentdate, id);
        allStudents.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

    public static Student Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students WHERE id = @id;";
      cmd.Parameters.AddWithValue("@id", id);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      Student foundStudent = new Student("", DateTime.Now, id);
      while (rdr.Read())
      {
        foundStudent.Name = rdr.GetString(1);
        foundStudent.EnrollmentDate = rdr.GetDateTime(2);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundStudent;
    }

    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE students SET name = @name WHERE id=@id;";

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
      cmd.CommandText = @"DELETE FROM students WHERE id = @id;";
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
      cmd.CommandText = @"DELETE FROM students;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
