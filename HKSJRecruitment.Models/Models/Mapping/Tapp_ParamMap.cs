using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class Tapp_ParamMap : EntityTypeConfiguration<Tapp_Param>
    {
        public Tapp_ParamMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ParamsName)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(250);

            this.Property(t => t.CreateBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EditBy)
                .HasMaxLength(50);

            this.Property(t => t.DeleteBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Tapp_Param");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Parentid).HasColumnName("Parentid");
            this.Property(t => t.ParamsName).HasColumnName("ParamsName");
            this.Property(t => t.Sort).HasColumnName("Sort");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.EditBy).HasColumnName("EditBy");
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            this.Property(t => t.DeleteBy).HasColumnName("DeleteBy");
            this.Property(t => t.DeleteTime).HasColumnName("DeleteTime");
        }
    }
}
