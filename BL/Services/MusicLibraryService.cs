using System;
using System.Collections.Generic;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services
{
    /// <summary>
    /// Base class for all services
    /// </summary>
    public abstract class MusicLibraryService
    {
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }
    }
}
