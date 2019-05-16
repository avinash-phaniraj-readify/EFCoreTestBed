using Linq2SqlEFCoreBehaviorsTest;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace MemCompare
{
    class ProgramLeakTest
    {
        static void Main(string[] args)
        {
            //L2S();
            EFCore();
        }

        private static void EFCore()
        {
            using (var databaseFixure = new DatabaseFixture())
            {
                Console.WriteLine("Start!");

                var builder = new DbContextOptionsBuilder<Linq2SqlEFCoreBehaviorsTest.EFCore.EFCoreDataContext>();
                builder.UseSqlCe(databaseFixure.Connection);

                var options = builder.Options;

                for (int i = 0; i < 10000; i++)
                {
                    using (var context = new Linq2SqlEFCoreBehaviorsTest.EFCore.EFCoreDataContext(options))
                    {
                        Console.WriteLine(i);
                        var data = new LeakTest().EFCore_ProjectionWithInstanceMethodCall(context);
                    }
                }

                GC.Collect();
                Console.WriteLine("Done!");
            }
        }

        private static void L2S()
        {
            using (var databaseFixure = new DatabaseFixture())
            {
                Console.WriteLine("Start!");

                for (int i = 0; i < 10000; i++)
                {
                    using (var context = new Linq2SqlEFCoreBehaviorsTest.Linq2Sql.Linq2SqlDataContext(databaseFixure.Connection))
                    {
                        Console.WriteLine(i);
                        var data = new LeakTest().L2S_ProjectionWithInstanceMethodCall(context);
                        //var data = new LeakTest().L2S_CompiledQueryProjectionWithInstanceMethodCall(context).ToList();
                    }
                }

                GC.Collect();
                Console.WriteLine("Done!");
            }
        }
    }

    public class LeakTest
    {
        public LeakTest()
        {
            // NOTE: Does not leak.
            L2S_CompiledQueryProjectionWithInstanceMethodCall = 
                CompiledQuery.Compile((Linq2SqlEFCoreBehaviorsTest.Linq2Sql.Linq2SqlDataContext context) => context.Employees.Select(w => new Projection { Name = MyFunc(w.Name) }));
        }

        // NOTE: This leaks ~35MB+ in 10,000 executions.
        public IEnumerable<Projection> EFCore_ProjectionWithInstanceMethodCall(Linq2SqlEFCoreBehaviorsTest.EFCore.EFCoreDataContext context)
        {
            return context.Set<Linq2SqlEFCoreBehaviorsTest.EFCore.Employee>().Select(w => new Projection
            {
                Name = MyFunc(w.Name)
            }).ToList();
        }

        // NOTE: Does not leak.
        public IEnumerable<Projection> EFCore_ProjectionWithLocalLambda(Linq2SqlEFCoreBehaviorsTest.EFCore.EFCoreDataContext context)
        {
            Func<string, string> myFunc = name => name;

            return context.Set<Linq2SqlEFCoreBehaviorsTest.EFCore.Employee>().Select(w => new Projection
            {
                Name = myFunc(w.Name)
            });
        }

        // NOTE: Does not leak.
        public IEnumerable<Projection> L2S_ProjectionWithInstanceMethodCall(Linq2SqlEFCoreBehaviorsTest.Linq2Sql.Linq2SqlDataContext context)
        {
            return context.Employees.Select(w => new Projection
            {
                Name = MyFunc(w.Name)
            });
        }

        public Func<Linq2SqlEFCoreBehaviorsTest.Linq2Sql.Linq2SqlDataContext, IQueryable<Projection>> L2S_CompiledQueryProjectionWithInstanceMethodCall;

        private string MyFunc(string value)
        {
            return value.ToString();
        }
    }

    public class Projection
    {
        public Projection()
        {
        }

        public string Name { get; set; }
    }
}