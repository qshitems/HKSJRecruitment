using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class Vapp_UserRightMap : EntityTypeConfiguration<Vapp_UserRight>
    {
        public Vapp_UserRightMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Url)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Vapp_UserRight");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Url).HasColumnName("Url");
        }
    }
}
