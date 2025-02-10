using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TradeCategorizationSystem.Domain;

namespace TradeCategorizationSystem.Infrastructure
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
builder.HasIndex(c => c.Name).IsUnique();
            builder.Property(c => c.InitialValue).IsRequired();
            builder.Property(c => c.FinalValue).IsRequired();
            builder.Property(c => c.ClientSector).IsRequired();
            builder.Property(c => c.InitialValue).IsRequired();
builder.Property(c => c.FinalValue).IsRequired();
builder.Property(c => c.ClientSector).IsRequired();
builder.Property(c => c.IsActive).IsRequired();
        }
    }
}
