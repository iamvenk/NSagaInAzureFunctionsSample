using NSaga;
using PolicyEntities;
using PolicyManagementSaga.Payment;
using PolicyManagementSaga.Process;
using PolicyManagementSaga.Untils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PolicyManagementSaga.Creation
{
    /// <summary>
    /// Wrapper class to create new policy. Accepts new policy reuquest and execute saga to create new policy
    /// </summary>
    public class PolicyCreator
    {
        private NewPolicyRequest _newPolicyRequest;
        private ISagaMediator sagaMediator;
        private ISagaRepository sagaRepository;
        private Guid _guid;

        /// <summary>
        /// Default constructor to initialize the policy constructor
        /// </summary>
        /// <param name="request">New policy request</param>
        public PolicyCreator(NewPolicyRequest request)
        {
            _newPolicyRequest = request;

            var builder = Wireup.UseInternalContainer().UseMessageSerialiser<JsonNetSerialiser>();
            sagaMediator = builder.ResolveMediator();
            sagaRepository = builder.ResolveRepository();

            _guid = Guid.NewGuid();
            Console.WriteLine($"New GUID: {_guid}");

            ((ISagaMessage)_newPolicyRequest).CorrelationId = _guid;
        }

        /// <summary>
        /// Executes saga to create policy
        /// </summary>
        /// <returns>Policy creation result</returns>
        public Result Do()
        {
            var consumer = new SagaConsumer(sagaMediator);

            // Initialize Saga
            consumer.Consume(_newPolicyRequest);

            // Underwrite
            consumer.Consume(new Underwriting(_guid));

            // Process Payment
            consumer.Consume(new BankPaymentProcessing(_guid));

            // Create Policyy
            consumer.Consume(new PolicyCreation(_guid));

            var sagaData = sagaRepository.Find<PolicyCreationSaga>(_guid).SagaData;

            var result = new Result()
            {
                Policy = sagaData.Policy,
                Invoice = sagaData.Invoice,
                PaymentConfimrationNumber = sagaData.PaymentConfirmationNumber,
                Errors = consumer.Errors
            };

            return result;
        }

        /// <summary>
        /// Policy creation result
        /// </summary>
        public class Result
        {
            public Policy Policy { get; set; }
            public string PaymentConfimrationNumber { get; set; }
            public Invoice Invoice { get; set; }
            public List<string> Errors { get; set; }
            public List<string> Warnings { get; set; }

            public bool IsFailed => Errors.Any();

            public Result()
            {
                Errors = new List<string>();
                Warnings = new List<string>();
            }
        }

    }
}
