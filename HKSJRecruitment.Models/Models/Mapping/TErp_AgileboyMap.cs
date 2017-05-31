using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class TErp_AgileboyMap : EntityTypeConfiguration<TErp_Agileboy>
    {
        public TErp_AgileboyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Remarks)
                .IsFixedLength()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("TErp_Agileboy");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrgType).HasColumnName("OrgType");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
        }
    }
}
