﻿using Autofac;
using Microsoft.Extensions.Configuration;
using Sample.Infrastructure.Persistences;
using TripleSix.Core.AutofacModules;

namespace Sample.Infrastructure
{
    public class AutofacModule : BaseModule
    {
        public AutofacModule(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterDbContext(c => new ApplicationDbContext(Configuration));
        }
    }
}
