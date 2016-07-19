using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Registrar.Objects
{
  public class Student
  {
    private string _name;
    private DateTime? _enrollmentDate;
    private int _departmentId;
    private int _id;

    public Student(string name, DateTime? enrollmentDate, int departmentId, int Id = 0)
    {
      _name = name;
      _enrollmentDate = enrollmentDate;
      _departmentId = departmentId;
      _id = Id;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public DateTime? GetEnrollmentDate()
    {
      return _enrollmentDate;
    }
    public int GetDepartmentId()
    {
      return _departmentId;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool nameEquality = this.GetName() == newStudent.GetName();
        bool enrollmentDateEquality = this.GetEnrollmentDate() == newStudent.GetEnrollmentDate();
        bool departmentIdEquality = this.GetDepartmentId() == newStudent.GetDepartmentId();
        return (idEquality && nameEquality && enrollmentDateEquality && departmentIdEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students; DELETE FROM students_courses", conn);
      cmd.ExecuteNonQuery();
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM students WHERE id = @StudentId; DELETE FROM students_courses WHERE student_id = @StudentId;", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(studentIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime? studentenrollmentDate = rdr.GetDateTime(2);
        int studentDepartmentId = rdr.GetInt32(3);
        Student newStudent = new Student(studentName, studentenrollmentDate, studentDepartmentId, studentId);
        allStudents.Add(newStudent);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allStudents;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students (name, enrollment_date, department_id) OUTPUT INSERTED.id VALUES (@StudentName, @EnrollmentDate, @DepartmentId);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@StudentName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);

      SqlParameter dateParameter = new SqlParameter();
      dateParameter.ParameterName = "@EnrollmentDate";
      dateParameter.Value = this.GetEnrollmentDate();
      cmd.Parameters.Add(dateParameter);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = this.GetDepartmentId();
      cmd.Parameters.Add(departmentIdParameter);
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

    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = id.ToString();
      cmd.Parameters.Add(studentIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundStudentName = null;
      DateTime? foundEnrollmentDate = null;
      int foundDepartmentId = 0;

      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundStudentName = rdr.GetString(1);
        foundEnrollmentDate = rdr.GetDateTime(2);
        foundDepartmentId = rdr.GetInt32(3);
      }
      Student foundStudent = new Student(foundStudentName, foundEnrollmentDate, foundDepartmentId, foundStudentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStudent;
    }

    public void AddCourse(Course newCourse)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (course_id, student_id) VALUES (@CourseId, @StudentId);", conn);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = newCourse.GetId();
      cmd.Parameters.Add(courseIdParameter);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(studentIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }

    }

    public List<Course> GetCourses()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN students_courses ON (students_courses.student_id = students.id) JOIN courses ON (students_courses.course_id = courses.id) WHERE students.id = @StudentId;", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = _id;
      cmd.Parameters.Add(studentIdParameter);

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

    public List<Course> GetAvailableCourses()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN students_courses ON (students_courses.student_id = students.id) JOIN courses ON (students_courses.course_id != courses.id) WHERE students.id = @StudentId;", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = _id;
      cmd.Parameters.Add(studentIdParameter);

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

  }
}
