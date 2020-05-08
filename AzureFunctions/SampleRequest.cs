using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PolicyManagementSaga.Creation;
using PolicyEntities;
using System.Collections.Generic;

namespace AzureFunctions
{
    public static class SampleRequest
    {
        [FunctionName("SampleRequest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string reqType = req.Query["type"];

            var reqObj = new NewPolicyRequest()
            {
                Holder = Person("Van", "Mathew", DummyAddress()),
                Insured = Person("Jon", "Mathew", DummyAddress()),
                ValidFrom = new DateTime(2020, 6, 1),
                ValidTo = new DateTime(2020, 5, 31),
                Payment = BankPayment(),
                Coverages = Coverages()
            };

            return new OkObjectResult(reqObj);
        }

        private static Person Person(string fName, string lName, Address addr)
        {
            return new Person()
            {
                FirstName = fName,
                LastName = lName,
                Address = addr
            };
        }

        private static Address DummyAddress()
        {
            return new Address()
            {
                Line1 = "345 Orchid Ave",
                City = "Pembroke Pines",
                State = "FL",
                ZipCode = "33024"

            };
        }

        private static BankPayment BankPayment()
        {
            return new BankPayment()
            {
                AccountNo = "0000000000000",
                RoutingNo = "9999999999999",
                Bank = "Penny Bank"
            };
        }

        private static List<Coverage> Coverages()
        {
            List<Coverage> covs = new List<Coverage>();

            covs.Add(new Coverage() { 
                Code = "NLM",
                Premium = 334.34M
            });
            covs.Add(new Coverage()
            {
                Code = "GKM",
                Premium = 231.23M
            });

            return covs;
        }
    }
}
