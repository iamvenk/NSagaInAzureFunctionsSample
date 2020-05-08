using System;
using System.Collections.Generic;
using System.Text;

namespace PolicyEntities
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
    }
}
