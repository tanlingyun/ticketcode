using Microsoft.EntityFrameworkCore;

namespace TicketCode.Infrastructure.Data
{
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}
