using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Registrar.Objects
{
  public class Department
  {
    private string _name;
    private int _id;

    public Department(string name, int Id=0)
    {
      _name = name;
      _id = Id;
    }

    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM departments;", conn);
      cmd.ExecuteNonQuery();
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM departments WHERE id = @DepartmentId;", conn);
      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(departmentIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public override bool Equals(System.Object otherDepartment)
    {
      if(!(otherDepartment is Department))
      {
        return false;
      }
      else
      {
        Department newDepartment = (Department) otherDepartment;
        bool idEquality = (_id == newDepartment.GetId());
        bool nameEquality = (_name == newDepartment.GetName());
        return (idEquality && nameEquality);
      }
    }

    public static List<Department> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT * FROM departments;", conn);

      List<Department> allDepartments = new List<Department>{};

      rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int newDepartmentId = rdr.GetInt32(0);
        string newDepartmentName = rdr.GetString(1);
        Department newDepartment = new Department(newDepartmentName, newDepartmentId);
        allDepartments.Add(newDepartment);
      }
      if(rdr != null) rdr.Close();
      if(conn != null) conn.Close();

      return allDepartments;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO departments (name) OUTPUT INSERTED.id VALUES (@DepartmentName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@DepartmentName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Department Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE id = @DepartmentId;", conn);
      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = id.ToString();
      cmd.Parameters.Add(departmentIdParameter);
      rdr = cmd.ExecuteReader();

      int foundDepartmentId = 0;
      string foundDepartmentName = null;

      while(rdr.Read())
      {
        foundDepartmentId = rdr.GetInt32(0);
        foundDepartmentName = rdr.GetString(1);
      }
      Department foundDepartment = new Department(foundDepartmentName, foundDepartmentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundDepartment;
    }

    public List<Course> GetCourses()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE department_id = @DepartmentId;", conn);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = _id;
      cmd.Parameters.Add(departmentIdParameter);

      rdr = cmd.ExecuteReader();
      List<Course> courses = new List<Course>{};

      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        int courseNumber = rdr.GetInt32(2);
        int courseDepartment = rdr.GetInt32(3);
        Course foundCourse = new Course(courseName, courseNumber, courseDepartment, courseId);
        courses.Add(foundCourse);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
      return courses;
    }

    public List<Student> GetStudents()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT *  FROM students WHERE department_id = @DepartmentId;", conn);

      SqlParameter departmentParameter = new SqlParameter();
      departmentParameter.ParameterName = "@DepartmentId";
      departmentParameter.Value = _id;
      cmd.Parameters.Add(departmentParameter);

      rdr = cmd.ExecuteReader();
      List<Student> students = new List<Student>{};

      while(rdr.Read())
      {
        int thisStudentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime? studentenrollmentDate = rdr.GetDateTime(2);
        int studentDepartmentId = rdr.GetInt32(3);
        Student foundStudent = new Student(studentName, studentenrollmentDate, studentDepartmentId, thisStudentId);
        students.Add(foundStudent);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
      return students;
    }

  }
}
