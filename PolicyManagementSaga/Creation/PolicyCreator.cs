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
    public class PolicyCreator
    {
        private NewPolicyRequest _newPolicyRequest;
        private ISagaMediator sagaMediator;
        private ISagaRepository sagaRepository;
        private Guid _guid;

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

        public Result Do()
        {
            var result = new Result();

            // Initialize Saga
            //sagaMediator.Consume(_newPolicyRequest);

            // Underwrite
            //sagaMediator.Consume(new Underwriting(_guid));

            // Process Payment
            //sagaMediator.Consume(new BankPaymentProcessing(_guid, _newPolicyRequest.Payment));

            // Create Policyy
            //sagaMediator.Consume(new PolicyCreation(_guid));

            // Initiate New Policy Request -> Underwrite -> Process Payment -> Generate Policy

            var errs = Execute(sagaMediator, 
                                _newPolicyRequest,
                                new Underwriting(_guid),
                                new BankPaymentProcessing(_guid, _newPolicyRequest.Payment),
                                new PolicyCreation(_guid));

            var saga = sagaRepository.Find<PolicyCreationSaga>(_guid);

            result.Policy = saga.SagaData.Policy;
            result.Invoice = saga.SagaData.Invoice;
            result.PaymentConfimrationNumber = saga.SagaData.PaymentConfirmationNumber;
            result.Errors = errs;
            
            return result;
        }

        public List<string> Execute(ISagaMediator mediator, IInitiatingSagaMessage initMsg, params ISagaMessage[] sagaSteps)
        {
            List<string> errors = new List<string>();

            // Initalize
            var opResult = mediator.Consume(initMsg);
            if (opResult.HasErrors)
                errors.AddCollection(opResult.Errors);

            foreach(var step in sagaSteps)
            {
                opResult = mediator.Consume(step);
                if (opResult.HasErrors)
                    errors.AddCollection(opResult.Errors);
            }

            return errors;
        }

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
