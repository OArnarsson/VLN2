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
            CoursesAndAssignments as1 = new CoursesAndAssignments { id = 1, CourseName = "Gagnaskipan", Description = "Í þessu námskeiði er fjallað um ýmiss konar gagnaskipan, eins og tengda lista, stafla, biðraðir, tré og tætitöflur. Jafnframt er farið í endurkvæma forritun og röðunaralgrím. Í námskeiðinu er að auki lögð áhersla á hugræn gagnatög, hlutbundna forritun, sniðmát og meðhöndlun frábrigða. Forritunarmálið C++ er notað í námskeiðinu."
            , assignments = getAss(), allUsers = getUser(1) };

            CoursesAndAssignments as2 = new CoursesAndAssignments { id = 2, CourseName = "Vefforitun", Description = "Áfanginn mun kynna grunnatriði við gerð veflausna og hönnunarmynstra tengdum þeim. Áherslan verður á samskiptamáta og staðla sem eru sameiginlegir með öllum veflausnum, ásamt þeim grunnatriðum úr hlutbundinni forritun sem eru notuð við útfærslu veflausna. Nemendur munu einnig læra að auka öryggi veflausna, hvernig meðhöndla skuli villur, hvernig eigi að skrifa og nota vefþjónustur, og um bestu venjur við útfærslur þeirra. Þá munu nemendur læra hvernig tengjast skuli gagnagrunnum og öðrum gagnageymslum."
            , assignments = getAss(), allUsers = getUser(2) };

            CoursesAndAssignments as3 = new CoursesAndAssignments { id = 3, CourseName = "Forritun", Description = "Þetta er inngangsnámskeið í forritun þar sem forritunarmálið C++ er notað. Fjallað er um grunneiningar í forritun, t.d.  breytur, tög, stýriskipanir, föll og benda. Jafnframt er lögð áhersla á innbyggðar gagnagrindur eins og fylki, strengi og vektora.  Hugtakið klasi er kynnt og hvernig það styður við hjúpun og upplýsingarhuld í hlutbundinni forritun.", assignments = getAss(), allUsers = getUser(3) };
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