﻿using System;
using System.Data.Entity;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.AppInfrastructure
{
    public class AppUnitOfWorkProvider : EntityFrameworkUnitOfWorkProvider
    {
        public AppUnitOfWorkProvider(IUnitOfWorkRegistry registry, Func<DbContext> dbContextFactory) 
            : base(registry, dbContextFactory) { }

        protected override EntityFrameworkUnitOfWork CreateUnitOfWork(Func<DbContext> dbContextFactory, DbContextOptions options)
        {
            return new AppUnitOfWork(this, dbContextFactory, options);
        }
    }
}
