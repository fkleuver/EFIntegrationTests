using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using Effort;
using Xunit.Abstractions;

namespace EFIntegrationTests.Tests
{
    public class DbContextTestClass<TContext> where TContext : DbContext
    {
        protected readonly ITestOutputHelper Output;
        private string DbConnectionInstanceId { get; set; }

        public DbContextTestClass(ITestOutputHelper output)
        {
            Output = output;
            DbConnectionInstanceId = Guid.NewGuid().ToString();
        }

        public TContext CreateContext(bool reuseLastInstance = true)
        {
            TContext output = null;

            var publicConstructors = typeof(TContext).GetConstructors()
                .Where(c => c.GetParameters().Any(p => p.ParameterType == typeof(DbConnection)))
                .OrderBy(c => c.GetParameters().Length)
                .ToList();

            var constructor = publicConstructors.LastOrDefault();
            if (constructor != null)
            {
                var @params = new List<object>();
                if (!reuseLastInstance)
                {
                    DbConnectionInstanceId = Guid.NewGuid().ToString();
                }
                var connection = DbConnectionFactory.CreatePersistent(DbConnectionInstanceId);

                @params.Add(connection);
                if (constructor.GetParameters().Any(p => p.ParameterType == typeof(bool)))
                {
                    @params.Add(true);
                }
                output = (TContext)constructor.Invoke(@params.ToArray());
            }

            return output;
        }
    }
}
