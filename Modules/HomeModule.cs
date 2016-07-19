using Nancy;
using System.Collections.Generic;
using Registrar.Objects;

namespace Registrar
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => View["index.cshtml"];

      Get["/students"] = _ =>
      {
        List<Student> allStudents = Student.GetAll();
        return View["students.cshtml", allStudents];
      };
      Get["/students/add"] = _ =>
      {
        List<Department> allDepartments = Department.GetAll();
        return View["add_student.cshtml", allDepartments];
      };

      Post["/students/add"] = _ =>
      {
        Student newStudent = new Student(Request.Form["name"], Request.Form["date"], Request.Form["major"]);
        newStudent.Save();
        List<Student> allStudents = Student.GetAll();
        return View["students.cshtml", allStudents];
      };

      Get["/courses"] = _ =>
      {
        List<Course> allCourses = Course.GetAll();
        return View["courses.cshtml", allCourses];
      };

      Get["/courses/add"] = _ => {
        List<Department> allDepartments = Department.GetAll();
        return View["add_course.cshtml", allDepartments];
      };

      Post["/courses/add"] = _ =>
      {
        Course newCourse = new Course(Request.Form["name"], Request.Form["number"], Request.Form["department"]);
        newCourse.Save();
        List<Course> allCourses = Course.GetAll();
        return View["courses.cshtml", allCourses];
      };

      Get["/students/{id}"] = parameters =>
      {
        Student student = Student.Find(parameters.id);
        return View["student.cshtml", student];
      };

      Get["/courses/{id}"] = parameters =>
      {
        Course course = Course.Find(parameters.id);
        return View["course.cshtml", course];
      };

      Get["/students/{id}/enroll"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Student student = Student.Find(parameters.id);
        model.Add("student", student);
        List<Course> courses = student.GetAvailableCourses();
        model.Add("courses", courses);
        return View["enroll.cshtml", model];
      };

      Patch["/students/{id}/enroll"] = parameters =>
      {
        Student student = Student.Find(parameters.id);
        Course course = Course.Find(Request.Form["course"]);
        student.AddCourse(course);
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("student", student);
        List<Course> allCourses = Course.GetAll();
        model.Add("courses", allCourses);
        return View["enroll.cshtml", model];
      };

      Delete["/courses/delete"] = _ =>
      {
        Course.DeleteAll();
        return View["index.cshtml"];
      };

      Delete["/delete"] = _ =>
      {
        Course.DeleteAll();
        Student.DeleteAll();
        return View["index.cshtml"];
      };

    }
  }

}
