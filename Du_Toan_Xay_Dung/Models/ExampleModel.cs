using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class ExampleModel
    {
        public ExampleModel() 
        {
            //person = new List<person>();
        }

        public string email { get; set; }
        public List<Person> person { get; set; }
    }

    public class Person
    {
        public Person() { }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}