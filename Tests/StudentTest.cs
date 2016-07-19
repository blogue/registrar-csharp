using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar.Objects
{
  public class StudentTest : IDisposable
  {
    private DateTime date = new DateTime(2016, 7, 1);

    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
    [Fact]
    public void Student_DatabaseEmpty()
    {
      //Arrange
      List<Student> allStudents = Student.GetAll();
      int result = allStudents.Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Student_Equals_SameStudents()
    {
      //Arrange, Act
      Student firstStudent = new Student("Aaron", date);
      Student secondStudent = new Student("Aaron", date);
      //Assert
      Assert.Equal(firstStudent, secondStudent);
    }

    [Fact]
    public void Student_Save_SavesToDatabase()
    {
      //Arrange
      Student testStudent = new Student("Aaron", date);
      //Act
      testStudent.Save();
      Student savedStudent = Student.GetAll()[0];
      //Assert
      Assert.Equal(testStudent, savedStudent);
    }

    [Fact]
    public void Student_Delete_DeletesFromDatabase()
    {
      //Arrange
      Student testStudent = new Student("Aaron", date);
      testStudent.Save();
      //Act
      testStudent.Delete();
      int result = Student.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Student_FindStudentInDatabase()
    {
      //Arrange
      Student testStudent = new Student("Aaron", date);
      testStudent.Save();
      //Act
      Student foundStudent = Student.Find(testStudent.GetId());
      //Assert
      Assert.Equal(testStudent, foundStudent);
    }

    [Fact]
    public void Student_GetCoursesByStudent()
    {
      //Arrange
      Student testStudent = new Student("Aaron", date);
      testStudent.Save();
      Course testCourse = new Course("History", 101);
      testCourse.Save();
      List<Course> expectedResult = new List<Course>{testCourse};
      //Act
      testStudent.AddCourse(testCourse);
      List<Course> result = testStudent.GetCourses();
      //Assert
      Assert.Equal(expectedResult, result);
    }
  }
}
