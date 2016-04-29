using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoderUi.DummyModels
{
    public class DummyUsers
    {   
        public int id { get; set; }     
        public string name { get; set; }
        public string role { get; set; }
        public string email { get; set; }
        public int CourseId { get; set; }

        public List<DummyUsers> getDummyUsers()
        {

            DummyUsers as1 =  new DummyUsers {id=1,  name = "DumDum6",  role = "Teacher",  email = "somedummy@dumdum.com",CourseId = 1};
            DummyUsers as2 =  new DummyUsers {id=2,  name = "DumDum7",  role = "Teacher",  email = "somedummy@dumdum.com",CourseId = 2};
            DummyUsers as3 =  new DummyUsers {id=3,  name = "DumDum8",  role = "Teacher",  email = "somedummy@dumdum.com",CourseId = 3};
            DummyUsers as4 =  new DummyUsers {id=4,  name = "DumDum9",  role = "Teacher",  email = "somedummy@dumdum.com",CourseId = 1};
            DummyUsers as5 =  new DummyUsers {id=5,  name = "DumDum10",  role = "Teacher", email = "somedummy@dumdum.com",CourseId = 2};
            DummyUsers as6 =  new DummyUsers {id=6,  name = "DumDum5",  role = "Teacher",  email = "somedummy@dumdum.com",CourseId = 3};
            DummyUsers as7 =  new DummyUsers {id=7,  name = "DumDum4",  role = "TA",       email = "somedummy@dumdum.com",CourseId = 3};
            DummyUsers as8 =  new DummyUsers {id=8,  name = "DumDum3",  role = "TA",       email = "somedummy@dumdum.com",CourseId = 2};
            DummyUsers as9 =  new DummyUsers {id=9,  name = "DumDum2",  role = "TA",       email = "somedummy@dumdum.com",CourseId = 1};
            DummyUsers as10 = new DummyUsers {id=10, name = "DumDum1", role = "TA",        email = "somedummy@dumdum.com",CourseId = 2};

            List<DummyUsers> benni = new List<DummyUsers>();
            benni.Add(as1);
            benni.Add(as2);
            benni.Add(as3);
            benni.Add(as4);
            benni.Add(as5);
            benni.Add(as6);
            benni.Add(as7);
            benni.Add(as8);
            benni.Add(as9);
            benni.Add(as10);
            return benni;
        }
    }
    
}
