using Microsoft.EntityFrameworkCore;
using TicketCode.Infrastructure.Data;
using TicketCode.Core.Models;

namespace TicketCode.Core.Data
{
    public class CoreCustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TcAccounts>()
                .ToTable("TcAccounts");
            modelBuilder.Entity<TcGroups>()
                .ToTable("TcGroups");
            modelBuilder.Entity<TcGroupInAccount>()
                .ToTable("TcGroupInAccount");
            modelBuilder.Entity<TcRequsets>()
                .ToTable("TcRequsets");
            modelBuilder.Entity<TcRequestLines>()
                .ToTable("TcRequestLines");
            modelBuilder.Entity<TcConsume>()
                .ToTable("TcConsume");


            modelBuilder.Entity<TcAccounts>(account =>
            {
                account.HasIndex(x => x.sAppId).IsUnique();
            });

            modelBuilder.Entity<TcGroupInAccount>(gia =>
            {
                gia.HasOne(x => x.TcAccount)
                .WithMany(x => x.TcGroupInAccounts)
                .HasForeignKey(x => x.iAccountId)
                .OnDelete(DeleteBehavior.Restrict);

                gia.HasOne(x => x.TcGroup)
                .WithMany(x => x.TcGroupInAccounts)
                .HasForeignKey(x => x.iGroupId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TcRequsets>(request =>
            {
                request.HasIndex(x => new { x.iAccountId, x.sOuterNo }).IsUnique();

                request.HasOne(x => x.TcAccount)
                .WithMany()
                .HasForeignKey(x => x.iAccountId)
                .OnDelete(DeleteBehavior.Restrict);

                request.HasOne(x => x.TcGroup)
                .WithMany()
                .HasForeignKey(x => x.iGroupId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TcRequestLines>(line =>
            {
                line.HasOne(x => x.TcRequset)
                .WithMany(x => x.TcRequestLines)
                .HasForeignKey(x => x.iRequestId)
                .OnDelete(DeleteBehavior.Restrict);

                line.HasIndex(x => x.iFullCode);
            });

            modelBuilder.Entity<TcConsume>(consume =>
            {
                consume.HasOne(x => x.TcGroup)
                .WithMany()
                .HasForeignKey(x => x.iGroupId)
                .OnDelete(DeleteBehavior.Restrict);

                consume.HasOne(x => x.TcAccount)
                .WithMany()
                .HasForeignKey(x => x.iAccountId)
                .OnDelete(DeleteBehavior.Restrict);

                consume.HasOne(x => x.TcRequestLine)
                .WithOne(x => x.TcConsume)
                .HasForeignKey<TcConsume>(x => x.iRequestLineId)
                .OnDelete(DeleteBehavior.Restrict);

                consume.HasIndex(x => x.iFullCode);
            });

        }
    }
}
