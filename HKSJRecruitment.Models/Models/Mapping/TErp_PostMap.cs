using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class TErp_PostMap : EntityTypeConfiguration<TErp_Post>
    {
        public TErp_PostMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PostName)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.PostDesc)
                .HasMaxLength(200);

            this.Property(t => t.CreateBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EditBy)
                .HasMaxLength(50);

            this.Property(t => t.DeleteBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TErp_Post");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PostName).HasColumnName("PostName");
            this.Property(t => t.PostDesc).HasColumnName("PostDesc");
            this.Property(t => t.PositionId).HasColumnName("PositionId");
            this.Property(t => t.DeptId).HasColumnName("DeptId");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.EditBy).HasColumnName("EditBy");
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.DeleteBy).HasColumnName("DeleteBy");
            this.Property(t => t.DeleteTime).HasColumnName("DeleteTime");

            // Relationships
            this.HasRequired(t => t.TErp_Department)
                .WithMany(t => t.TErp_Post)
                .HasForeignKey(d => d.DeptId);
            this.HasRequired(t => t.TErp_Position)
                .WithMany(t => t.TErp_Post)
                .HasForeignKey(d => d.PositionId);

        }
    }
}
