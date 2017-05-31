using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class Tapp_ParamTypeMap : EntityTypeConfiguration<Tapp_ParamType>
    {
        public Tapp_ParamTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ParamName)
                .HasMaxLength(50);

            this.Property(t => t.ParamIcon)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("Tapp_ParamType");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParamName).HasColumnName("ParamName");
            this.Property(t => t.ParamIcon).HasColumnName("ParamIcon");
            this.Property(t => t.Sort).HasColumnName("Sort");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.IsShow).HasColumnName("IsShow");
        }
    }
}
