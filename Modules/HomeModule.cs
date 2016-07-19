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
      Get["/students/add"] = _ => View["add_student.cshtml"];

      Post["/students/add"] = _ =>
      {
        Student newStudent = new Student(Request.Form["name"], Request.Form["date"]);
        newStudent.Save();
        List<Student> allStudents = Student.GetAll();
        return View["students.cshtml", allStudents];
      };

      Get["/courses"] = _ =>
      {
        List<Course> allCourses = Course.GetAll();
        return View["courses.cshtml", allCourses];
      };

      Get["/courses/add"] = _ => View["add_course.cshtml"];

      Post["/courses/add"] = _ =>
      {
        Course newCourse = new Course(Request.Form["name"], Request.Form["number"]);
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
        List<Course> allCourses = Course.GetAll();
        model.Add("courses", allCourses);
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

    }
  }

}
