using System;
using ImaginedWorlds.Domain.Agent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImaginedWorlds.Infrastructure.Persistence.Configurations;

public class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(
                ulid => ulid.ToGuid(),
                guid => new Ulid(guid)
            );

        builder.Property(a => a.DisplayName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.CodeName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(a => a.CodeName).IsUnique();

        builder.Property(a => a.Description)
            .IsRequired()
            .HasMaxLength(500);


        builder.Property(a => a.ProviderConfiguration)
            .IsRequired()
            .HasColumnType("jsonb");;
            
        builder.Property(a => a.IconUrl)
            .HasConversion(
                uri => uri.ToString(),
                str => new Uri(str)
            )
            .IsRequired();
    }
}

