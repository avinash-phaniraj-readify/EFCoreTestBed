using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TestHostForCastException
{
    class Program
    {
        static void Main(string[] args)
        {
            var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
            var ceConnection = new SqlCeConnection(ceConnectionString);
            ceConnection.Open();

            var options = new DbContextOptionsBuilder<TestDataContext>()
                .UseSqlCe(ceConnection)
                .Options;

            var context = new TestDataContext(options);

            var query =
                context.Set<Employee>()
                .Select(e => new
                {
                    EmployeeName = e.Name,
                    DeviceCount = GetDeviceCount(e)
                });

            //var materialized = query.ToList(); // goes boom with System.InvalidOperationException: 'Error generated for warning 'Microsoft.EntityFrameworkCore.Infrastructure.DetachedLazyLoadingWarning: An attempt was made to lazy-load navigation property 'Devices' on detached entity of type 'Employee'. Lazy-loading is not supported for detached entities or entities that are loaded with 'AsNoTracking()'.'. This exception can be suppressed or logged by passing event ID 'CoreEventId.DetachedLazyLoadingWarning' to the 'ConfigureWarnings' method in 'DbContext.OnConfiguring' or 'AddDbContext'.'
            var materialized = 
                query
                    .RewriteWithPrematureMaterialization() // we'll apply this through an IQueryable<T> wrapper 
                    .ToList();
        }

        private static int GetDeviceCount(Employee employee)
        {
            return employee.Devices.Count();
        }
    }

    public static class QueryableExtensions
    {
        public static IQueryable<T> RewriteWithPrematureMaterialization<T>(this IQueryable<T> source)
        {
            if (source.Expression is MethodCallExpression methodCallExpression)
            {
                var expression = (Expression<Func<IQueryable<T>>>) ForcePrematureMaterialization(methodCallExpression);

                return expression.Compile().Invoke();
            }

            return source;
        }

        private static Expression ForcePrematureMaterialization(MethodCallExpression expression)
        {
            MethodBase GetGenericMethod(Type type, string name, Type[] typeArgs, Type[] argTypes, BindingFlags flags)
            {
                int typeArity = typeArgs.Length;
                var methods = type.GetMethods()
                    .Where(m => m.Name == name)
                    .Where(m => m.GetGenericArguments().Length == typeArity)
                    .Select(m => m.MakeGenericMethod(typeArgs));

                return Type.DefaultBinder.SelectMethod(flags, methods.ToArray(), argTypes, null);
            }

            var constant = expression.Arguments[0];

            var nodeType = constant.Type; // e.g. EntityQueryable<Employee>
            var actualType = nodeType.IsGenericType ? nodeType.GetGenericArguments()[0] : nodeType;
            var iEnumerableOfActualType = typeof(IEnumerable<>).MakeGenericType(actualType);

            // public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source);
            var toListMethod = (MethodInfo)
                GetGenericMethod(typeof(Enumerable), "ToList", new[] { actualType }, new Type[] { iEnumerableOfActualType }, BindingFlags.Static);

            var toListCall = Expression.Call(toListMethod, constant);

            // public static IQueryable<TElement> AsQueryable<TElement>(this IEnumerable<TElement> source);
            var asQueryableMethod = (MethodInfo)
                GetGenericMethod(typeof(Queryable), "AsQueryable", new[] { actualType }, new Type[] { iEnumerableOfActualType }, BindingFlags.Static);

            var asQueryableCall = Expression.Call(asQueryableMethod, toListCall);

            var method = expression.Method; // e.g. IQueryable<anonymous> Select<Employee,anonymous<IQueryable<Employee>, Expression<System<Employee, anonymous>>>
            var selector = expression.Arguments[1]; // e.g. Expression<Func<Employee,anonymous>>
            var resultantCall = Expression.Call(method, asQueryableCall, selector);

            return Expression.Lambda(resultantCall);
        }
    }
}
