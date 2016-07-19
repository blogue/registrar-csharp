using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar.Objects
{
  public class DepartmentTest : IDisposable
  {
    private DateTime date = new DateTime(2016, 7, 1);

    public DepartmentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Department.DeleteAll();
      Student.DeleteAll();
      Course.DeleteAll();
    }
    [Fact]
    public void Department_DatabaseEmpty()
    {
      //Arrange
      List<Department> allDepartments = Department.GetAll();
      int result = allDepartments.Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Department_Equals_SameDepartments()
    {
      //Arrange, Act
      Department firstDepartment = new Department("History");
      Department secondDepartment = new Department("History");
      //Assert
      Assert.Equal(firstDepartment, secondDepartment);
    }

    [Fact]
    public void Department_Save_SavesToDatabase()
    {
      //Arrange
      Department testDepartment = new Department("History");
      //Act
      testDepartment.Save();
      Department savedDepartment = Department.GetAll()[0];
      //Assert
      Assert.Equal(testDepartment, savedDepartment);
    }

    [Fact]
    public void Department_Delete_DeletesFromDatabase()
    {
      //Arrange
      Department testDepartment = new Department("History");
      testDepartment.Save();
      //Act
      testDepartment.Delete();
      int result = Department.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Department_FindDepartmentInDatabase()
    {
      //Arrange
      Department testDepartment = new Department("History");
      testDepartment.Save();
      //Act
      Department foundDepartment = Department.Find(testDepartment.GetId());
      //Assert
      Assert.Equal(testDepartment, foundDepartment);
    }

    [Fact]
    public void Department_FindCoursesByDepartment()
    {
      //Arrange
      Department testDepartment = new Department("History");
      testDepartment.Save();
      Course testCourse = new Course("History", 101, testDepartment.GetId());
      List<Course> expectedResult = new List<Course>{testCourse};
      testCourse.Save();
      //Act
      List<Course> result = testDepartment.GetCourses();
      //Assert
      Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void Course_FindStudentByDepartment()
    {
      //Arrange
      Department testDepartment = new Department("History");
      testDepartment.Save();
      Student testStudent = new Student("Aaron", date, testDepartment.GetId());
      List<Student> expectedResult = new List<Student>{testStudent};
      testStudent.Save();
      //Act
      List<Student> result = testDepartment.GetStudents();
      //Assert
      Assert.Equal(expectedResult, result);
    }
  }
}
