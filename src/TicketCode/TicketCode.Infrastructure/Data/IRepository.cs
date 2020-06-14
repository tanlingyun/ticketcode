using TicketCode.Infrastructure.Models;

namespace TicketCode.Infrastructure.Data
{
    public interface IRepository<T> : IRepositoryWithTypedId<T, long> where T : IEntityWithTypedId<long>
    {

    }
}
