﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TicketCode.Infrastructure;
using TicketCode.Infrastructure.Data;
using TicketCode.Infrastructure.Models;
using TicketCode.Core.Models;

namespace TicketCode.Core.Data
{
    public class TcDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public TcDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ValidateEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Type> typeToRegisters = new List<Type>();
            //foreach (var module in GlobalConfiguration.Modules)
            //{
            //    typeToRegisters.AddRange(module.Assembly.DefinedTypes.Select(t => t.AsType()));
            //}

            RegisterEntities(modelBuilder);

            RegisterConvention(modelBuilder);

            base.OnModelCreating(modelBuilder);

            RegisterCustomMappings(modelBuilder);

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
                // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset

                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));
                    foreach (var property in properties)
                    {
                        modelBuilder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }

                    var decimalProperties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?));
                    foreach (var property in decimalProperties)
                    {
                        modelBuilder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion<double>();
                    }
                }
            }
        }

        private void ValidateEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                    .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in modifiedEntries)
            {
                if (entity.Entity is ValidatableObject validatableObject)
                {
                    var validationResults = validatableObject.Validate();
                    if (validationResults.Any())
                    {
                        throw new ValidationException(entity.Entity.GetType(), validationResults);
                    }
                }
            }
        }

        private static void RegisterConvention(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.ClrType.Namespace != null)
                {
                    var nameParts = entity.ClrType.Namespace.Split('.');
                    var tableName = string.Concat(nameParts[2], "_", entity.ClrType.Name);
                    modelBuilder.Entity(entity.Name).ToTable(tableName);
                }
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void RegisterEntities(ModelBuilder modelBuilder)
        {
            var entityTypes = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.GetTypeInfo().IsSubclassOf(typeof(EntityBase)) && !x.GetTypeInfo().IsAbstract);
            foreach (var type in entityTypes)
            {
                modelBuilder.Entity(type);
            }
        }

        private static void RegisterCustomMappings(ModelBuilder modelBuilder)
        {
            var customModelBuilderTypes = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => typeof(ICustomModelBuilder).IsAssignableFrom(x));
            foreach (var builderType in customModelBuilderTypes)
            {
                if (builderType != null && builderType != typeof(ICustomModelBuilder))
                {
                    var builder = (ICustomModelBuilder)Activator.CreateInstance(builderType);
                    builder.Build(modelBuilder);
                }
            }
        }
    }
}
