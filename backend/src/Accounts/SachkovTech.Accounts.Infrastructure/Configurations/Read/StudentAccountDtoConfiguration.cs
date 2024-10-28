using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Accounts.Contracts.Dtos;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;

namespace SachkovTech.Accounts.Infrastructure.Configurations.Read;

public class StudentAccountDtoConfiguration : IEntityTypeConfiguration<StudentAccountDto>
{
    public void Configure(EntityTypeBuilder<StudentAccountDto> builder)
    {
        builder.ToTable("student_accounts");

        builder.HasKey(s => s.Id);
        
        builder.Property(v => v.SocialNetworks)
            .HasConversion(
                values => ModelBuilderExtensions.SerializeValueObjectsCollection(),
                json => ModelBuilderExtensions.DeserializeDtoCollection<SocialNetworkDto>(json));
    }
}