using Microsoft.EntityFrameworkCore;

namespace CreditCardSimulator.Models
{
    public partial class MagicBankContext
    {
        [DbFunction("add_two")]
        public Int32 AddTwoMethod(Int32 a, Int32 b)
    => throw new NotSupportedException();

        [DbFunction("concat_mk")]
        public string ConcatTwoMethod(string a, string b)
    => throw new NotSupportedException();
        //throws if null is returned, if not value matches
        [DbFunction("search_by_year")]
        public IQueryable<CreditCard> SearchByYear(int year)
     => FromExpression(() => SearchByYear(year));

        public async Task<int> AddTwoNumbers(int a, int b)
        {
            var res = this.Database.SqlQuery<int>($"Select add_two({a},{b}) AS \"Value\"").First();
            //await res.ForEachAsync(elem => Console.WriteLine(elem));
            return res;
        }
        // https://stackoverflow.com/questions/45698359/entity-framework-core-stored-proc-without-dbset
        // used for stored proecures/ functions with types defined outside the entity models
    }
}
