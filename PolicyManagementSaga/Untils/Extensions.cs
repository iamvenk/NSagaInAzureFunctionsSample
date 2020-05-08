using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PolicyEntities;
using PolicyManagementSaga.Creation;

namespace PolicyManagementSaga.Untils
{
    public static class Extensions
    {
        public static Policy ConvertToPolicy(this NewPolicyRequest request)
        {
            var policy = new Policy()
            {
                Insured = request.Insured,
                Holder = request.Holder,
                ValidFrom = request.ValidFrom,
                ValidTo = request.ValidTo,
                Coverages = request.Coverages
            };

            return policy;
        }

        public static void AddCollection<T>(this List<T> collection, List<T> newValues)
        {
            if (newValues == null || !newValues.Any())
                return;

            collection.AddRange(newValues);
        }
    }
}
