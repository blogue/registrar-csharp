using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar.Objects
{
  public class CourseTest : IDisposable
  {
    private DateTime date = new DateTime(2016, 7, 1);

    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Course.DeleteAll();
      Student.DeleteAll();
    }
    [Fact]
    public void Course_DatabaseEmpty()
    {
      //Arrange
      List<Course> allCourses = Course.GetAll();
      int result = allCourses.Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Course_Equals_SameCourses()
    {
      //Arrange, Act
      Course firstCourse = new Course("History", 1535, 1);
      Course secondCourse = new Course("History", 1535, 1);
      //Assert
      Assert.Equal(firstCourse, secondCourse);
    }

    [Fact]
    public void Course_Save_SavesToDatabase()
    {
      //Arrange
      Course testCourse = new Course("History", 404, 1);
      //Act
      testCourse.Save();
      Course savedCourse = Course.GetAll()[0];
      //Assert
      Assert.Equal(testCourse, savedCourse);
    }

    [Fact]
    public void Course_Delete_DeletesFromDatabase()
    {
      //Arrange
      Course testCourse = new Course("History", 101, 1);
      testCourse.Save();
      //Act
      testCourse.Delete();
      int result = Course.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Course_FindCourseInDatabase()
    {
      //Arrange
      Course testCourse = new Course("History", 101, 1);
      testCourse.Save();
      //Act
      Course foundCourse = Course.Find(testCourse.GetId());
      //Assert
      Assert.Equal(testCourse, foundCourse);
    }
    [Fact]
    public void Course_FindStudentByCourse()
    {
      //Arrange
      Student testStudent = new Student("Aaron", date, 1);
      Course testCourse = new Course("History", 101, 1);
      List<Student> expectedResult = new List<Student>{testStudent};
      testStudent.Save();
      testCourse.Save();
      //Act
      testCourse.AddStudent(testStudent);
      List<Student> result = testCourse.GetStudents();
      //Assert
      Assert.Equal(expectedResult, result);

    }
  }
}
