using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;
using FileInfo = SachkovTech.SharedKernel.ValueObjects.Ids.FileInfo;

namespace SachkovTech.Issues.Infrastructure.Configurations.Read;

public class IssueDtoConfiguration : IEntityTypeConfiguration<IssueDto>
{
    public void Configure(EntityTypeBuilder<IssueDto> builder)
    {
        builder.ToTable("issues");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Files)
            .HasConversion(
                values => string.Empty,
                json => JsonSerializer.Deserialize<IEnumerable<FileInfo>>(json, JsonSerializerOptions.Default)!
                    .Select(f => f.Id.Value).ToArray());
    }
}