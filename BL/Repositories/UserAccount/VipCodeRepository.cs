using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class VipCodeRepository : EntityFrameworkRepository<VipCodes, int>
    {
        public VipCodeRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }
    }
}
