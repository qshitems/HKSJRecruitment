using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class Report_NeedsMap : EntityTypeConfiguration<Report_Needs>
    {
        public Report_NeedsMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.deptname)
                .HasMaxLength(50);

            this.Property(t => t.PositionName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_Needs");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.deptname).HasColumnName("deptname");
            this.Property(t => t.PositionName).HasColumnName("PositionName");
            this.Property(t => t.cnt).HasColumnName("cnt");
            this.Property(t => t.createtime).HasColumnName("createtime");
        }
    }
}
