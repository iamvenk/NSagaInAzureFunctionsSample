using System;
using System.Collections;
using System.Collections.Generic;
using PolicyEntities;

namespace PolicyBusiness
{
    internal class PolicyValidator
    {
        public static List<string> ValidateNewPolicyRequest(Policy policy)
        {
            var errors = new List<string>();

            if (policy == null)
            {
                errors.Add("Seriously...");
                return errors;
            }

            if(!IsValidPerson(policy.Insured))
            {
                errors.Add("To whom we have to insure? Insured details are not proper");
            }

            return errors;
        }

        private static bool IsValidPerson(Person person)
        {
            return person != null 
                    && !string.IsNullOrEmpty(person.FirstName) 
                    && !string.IsNullOrEmpty(person.LastName)
                    && IsValidAddress(person.Address);
        }

        private static bool IsValidAddress(Address address)
        {
            return address != null
                    && !string.IsNullOrEmpty(address.Line1)
                    && !string.IsNullOrEmpty(address.City)
                    && !string.IsNullOrEmpty(address.State)
                    && !string.IsNullOrEmpty(address.ZipCode);
        }
    }
}
