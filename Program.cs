using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;

namespace TestHostForCastException
{
    class Program
    {
        static void Main(string[] args)
        {
            var ceConnectionString = "data source=.;initial catalog=TestingDataBase;integrated security=True;MultipleActiveResultSets=True;";
            var ceConnection = new SqlCeConnection(ceConnectionString);


            var options = new DbContextOptionsBuilder()
                .UseSqlCe(ceConnection);

            Func<DbContextOptionsBuilder, DbContextOptionsBuilder> configureOptions = (c) => c.UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) }));


            options = configureOptions(options);

            using (var context = new TestDataContext(options.Options))
            {
                var assetQuery = from asset in context.Assets.OfType<Product>()
                                 where asset.TaxCategory.SerialCategory.GetValueOrDefault() != (int)ProvicerTypes.Small
                                 select asset;

                var productInfo = from  provider in context.Providers 
                                  join  l in assetQuery on provider.Id equals l.ProviderId
                                  select provider;

                var transactions = from transaction in context.Transactions
                                   join l in productInfo on transaction.ProductId equals l.Id
                                   let cache = l.TaxCategory.SerialCategory
                                   select new EE
                                   {
                                       Total = transaction.TotalAmount,
                                       Tax = (transaction.TotalAmount) * (l.TaxCategory.Percentage/100)
                                   };

                var s = transactions.ToList();

                }
        }

        public enum ProvicerTypes
        {
            Large = 1,
            Small = 0
        }

    }
    
    public class EE
    {
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
