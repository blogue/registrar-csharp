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
      Student firstStudent = new Student("Aaron", date, 1);
      Student secondStudent = new Student("Aaron", date, 1);
      //Assert
      Assert.Equal(firstStudent, secondStudent);
    }

    [Fact]
    public void Student_Save_SavesToDatabase()
    {
      //Arrange
      Student testStudent = new Student("Aaron", date, 1);
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
      Student testStudent = new Student("Aaron", date, 1);
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
      Student testStudent = new Student("Aaron", date, 1);
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
      Student testStudent = new Student("Aaron", date, 1);
      testStudent.Save();
      Course testCourse = new Course("History", 101, 1);
      testCourse.Save();
      List<Course> expectedResult = new List<Course>{testCourse};
      //Act
      testStudent.AddCourse(testCourse);
      List<Course> result = testStudent.GetCourses();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Student_GetAvailableCoursesByStudent()
    {
      //Arrange
      Student testStudent = new Student("Aaron", date, 1);
      testStudent.Save();
      Course firstCourse = new Course("History", 101, 1);
      firstCourse.Save();
      Course secondCourse = new Course("Math", 101, 2);
      secondCourse.Save();
      List<Course> expectedResult = new List<Course>{firstCourse, secondCourse};
      //Act
      List<Course> result = testStudent.GetAvailableCourses();
      //Assert
      Assert.Equal(expectedResult, result);
    }
  }
}
