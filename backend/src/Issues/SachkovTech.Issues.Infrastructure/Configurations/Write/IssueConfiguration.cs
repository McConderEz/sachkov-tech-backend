using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SachkovTech.Core.Extensions;
using SachkovTech.Issues.Domain.Module.Entities;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;
using FileInfo = SachkovTech.SharedKernel.ValueObjects.Ids.FileInfo;

namespace SachkovTech.Issues.Infrastructure.Configurations.Write;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("issues");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => IssueId.Create(value));

        builder.ComplexProperty(i => i.LessonId,
            lb =>
            {
                lb.Property(l => l!.Value)
                    .HasColumnName("lesson_id");
            });

        builder.ComplexProperty(i => i.Experience,
            lb =>
            {
                lb.Property(l => l.Value)
                    .IsRequired()
                    .HasColumnName("experience");
            });

        builder.ComplexProperty(i => i.Position,
            lb =>
            {
                lb.Property(l => l.Value)
                    .IsRequired()
                    .HasColumnName("position");
            });

        builder.ComplexProperty(m => m.Title, tb =>
        {
            tb.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Title.MAX_LENGTH)
                .HasColumnName("title");
        });

        builder.ComplexProperty(m => m.Description, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Description.MAX_LENGTH)
                .HasColumnName("description");
        });

        var enumConverter = new ValueConverter<IssueType, string>(
            v => v.ToString(),
            v => (IssueType)Enum.Parse(typeof(IssueType), v));

        builder.Property(i => i.Type)
            .HasConversion(new EnumToStringConverter<IssueType>());

        builder.Property(i => i.FilesInfo)
            .HasConversion(
                filesInfo => JsonSerializer.Serialize(filesInfo, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IReadOnlyList<FileInfo>>(json, JsonSerializerOptions.Default)!);

        builder.Property(i => i.CreatedAt)
            .SetDefaultDateTimeKind(DateTimeKind.Utc);

        // builder.Property(i => i.FilesInfo)
        //     .ValueObjectsCollectionJsonConversion(
        //         fileId => fileId.Value,
        //         FileId.Create)
        //     .HasColumnName("files");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

        builder.Property(i => i.DeletionDate)
            .IsRequired(false)
            .HasColumnName("deletion_date");
    }
}