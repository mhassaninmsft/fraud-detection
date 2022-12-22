using CreditCardSimulator.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace CreditCardSimulator.Startup
{
    public static class Odata
    {
        public class EdmModelBuilder
        {
            public static IEdmModel GetEdmModel()
            {
                var builder = new ODataConventionModelBuilder();
                builder.EntitySet<CreditCard>("CreditCards");
                builder.EntitySet<CreditCardTransaction>("CreditCardTransactions");
                return builder.GetEdmModel();
            }
        }
    }
}
