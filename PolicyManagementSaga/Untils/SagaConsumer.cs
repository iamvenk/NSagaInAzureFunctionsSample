using System;
using System.Collections.Generic;
using System.Text;
using NSaga;

namespace PolicyManagementSaga.Untils
{
    public class SagaConsumer
    {
        private readonly ISagaMediator _sagaMediator;
        
        public List<string> Errors = new List<string>();

        public SagaConsumer(ISagaMediator sagaMediator)
        {
            _sagaMediator = sagaMediator;
        }

        public OperationResult Consume(IInitiatingSagaMessage sagaMessage)
        {
            var opResult = _sagaMediator.Consume(sagaMessage);
            if (opResult.HasErrors)
                Errors.AddCollection(opResult.Errors);

            return opResult;
        }

        public OperationResult Consume(ISagaMessage sagaMessage)
        {
            var opResult = _sagaMediator.Consume(sagaMessage);
            if (opResult.HasErrors)
                Errors.AddCollection(opResult.Errors);

            return opResult;
        }
    }
}
