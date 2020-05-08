using System;
using System.Collections.Generic;

namespace PolicyEntities
{
    public class Policy
    {
        public string Id { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public Person Insured { get; set; }
        public Person Holder { get; set; }
        public List<Coverage> Coverages { get; set; }
    }
}
