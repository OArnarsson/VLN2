using CoderUi.DummyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoderUi.Controllers
{
    public class HomeController : Controller
    {

        public List<Assignment> getAss()
        {
            Assignment svenni = new Assignment();
            List<Assignment> benni = svenni.getAssignments();
            return benni;
        }

        public List<DummyUsers> getUser(int id)
        {
            DummyUsers hemmi = new DummyUsers();
            List<DummyUsers> benni = (from x in hemmi.getDummyUsers() where x.CourseId == id select x).ToList();
            return benni;
        }

        public List<CoursesAndAssignments> getActive()
        {
            CoursesAndAssignments svenni = new CoursesAndAssignments();

            List<CoursesAndAssignments> benni = new List<CoursesAndAssignments>();
            CoursesAndAssignments as1 = new CoursesAndAssignments { id = 1, CourseName = "Gagnaskipan", Description = "THIS is a shitty Gagnaskipan description", assignments = getAss(), allUsers = getUser(1) };
            CoursesAndAssignments as2 = new CoursesAndAssignments { id = 2, CourseName = "Vefforitun", Description = "THIS is a shitty Vefforitun description", assignments = getAss(), allUsers = getUser(2) };
            CoursesAndAssignments as3 = new CoursesAndAssignments { id = 3, CourseName = "Forritun", Description = "THIS is a shitty Forritun description", assignments = getAss(), allUsers = getUser(3) };
            benni.Add(as1);
            benni.Add(as2);
            benni.Add(as3);
            return benni;
        }
        public List<CoursesAndAssignments> getCourseDetail(int id)
        {

            List<CoursesAndAssignments> benni = new List<CoursesAndAssignments>();
            benni = (from x in getActive() where x.id == id select x).ToList();

            return benni;
        }

        public ActionResult Index()
        {
            //This view is for Student index to see upcoming projects.


            return View(getAss());
        }

        public ActionResult ActiveCourses()
        {

            return View(getActive());
        }

        public ActionResult CourseDetail(int id)
        {
            
                return View(getCourseDetail(id));
           
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}