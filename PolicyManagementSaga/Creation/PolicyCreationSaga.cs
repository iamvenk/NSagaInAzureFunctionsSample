using NSaga;
using PolicyManagementSaga.Payment;
using PolicyManagementSaga.Process;
using System;
using System.Collections.Generic;
using PolicyBusiness;
using PolicyManagementSaga.Untils;
using System.Linq;

namespace PolicyManagementSaga.Creation
{
    public class PolicyCreationSaga : ISaga<PolicyCreationSagaData>,
                                        InitiatedBy<NewPolicyRequest>,
                                        ConsumerOf<Underwriting>,
                                        ConsumerOf<BankPaymentProcessing>,
                                        ConsumerOf<PolicyCreation>
    {
        #region ISaga

        public PolicyCreationSagaData SagaData { get; set; }
        public Guid CorrelationId { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        #endregion

        private readonly PolicyManagement _policyMmgt;
        private readonly PaymentProcessor _pmntProcess;

        public PolicyCreationSaga(PolicyManagement policyManagement, PaymentProcessor paymentProcessor)
        {
            _policyMmgt = policyManagement;
            _pmntProcess = paymentProcessor;

            SagaData = new PolicyCreationSagaData();
            Headers = new Dictionary<string, string>();
        }

        #region Initiate

        public OperationResult Initiate(NewPolicyRequest message)
        {
            Console.WriteLine(" * Intializing Saga * ");

            SagaData.Request = message;
            var policy = message.ConvertToPolicy();

            var validationResult = _policyMmgt.ValidateNewPolicyRequest(policy);
            if (!validationResult.Any())
                SagaData.Policy = policy;

            return new OperationResult(validationResult.ToArray());
        }

        #endregion

        #region Underwrite

        public OperationResult Consume(Underwriting message)
        {
            Console.WriteLine(" * Underwriting Policy * ");
            
            if(SagaData.Policy == null)
                return new OperationResult();

            var underwritingResult = _policyMmgt.UnderwriteNewPolicy(SagaData.Policy);

            if(underwritingResult.Any())
                return new OperationResult(underwritingResult.ToArray());

            var invoice = _policyMmgt.GenerateInvoice(SagaData.Policy);
            SagaData.Invoice = invoice;

            return new OperationResult().AddPayload(SagaData.Invoice);
        }

        #endregion

        #region Payment

        public OperationResult Consume(BankPaymentProcessing message)
        {
            Console.WriteLine(" * Processing Payment * ");

            if (SagaData.Invoice == null)
                return new OperationResult();

            var paymentResult = _pmntProcess.InvoicePayment(SagaData.Invoice, message.BankPayment);
            SagaData.PaymentConfirmationNumber = paymentResult.PaymentConfirmatitonNumber;

            return new OperationResult(paymentResult.Errors.ToArray());
        }

        #endregion

        #region Policy Creation

        public OperationResult Consume(PolicyCreation message)
        {
            Console.WriteLine(" * Creating Policy in PMS * ");

            if (string.IsNullOrEmpty(SagaData.PaymentConfirmationNumber))
                return new OperationResult();

            if(!string.IsNullOrEmpty(SagaData.PaymentConfirmationNumber))
            {
                _policyMmgt.CreatePolicy(SagaData.Policy);

                SagaData.Invoice.PolicyId = SagaData.Policy.Id;
                _policyMmgt.SaveInvoice(SagaData.Invoice);
            }

            return new OperationResult().AddPayload(SagaData.Policy)
                                        .AddPayload(SagaData.Invoice);
        }

        #endregion

    }
}