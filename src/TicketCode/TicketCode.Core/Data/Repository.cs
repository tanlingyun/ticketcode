using TicketCode.Infrastructure.Data;
using TicketCode.Infrastructure.Models;

namespace TicketCode.Core.Data
{
    public class Repository<T> : RepositoryWithTypedId<T, long>, IRepository<T>
       where T : class, IEntityWithTypedId<long>
    {
        public Repository(SimplDbContext context) : base(context)
        {
        }
    }
}
