﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;

namespace SachkovTech.Files.Infrastructure.Database.Configurations.Read
{
    internal class FileDtoDataConfiguration : IEntityTypeConfiguration<FileDto>
    {
        public void Configure(EntityTypeBuilder<FileDto> builder)
        {
            builder.ToTable("files");

            builder.HasKey(i => i.Id);
            
            builder.Property(i => i.Attributes)
                .HasConversion(
                    values => ModelBuilderExtensions.SerializeValueObjectsCollection(),
                    json => ModelBuilderExtensions.DeserializeDtoCollection<string>(json));
        }
    }
}
