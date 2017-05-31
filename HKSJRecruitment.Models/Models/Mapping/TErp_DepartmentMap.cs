using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class TErp_DepartmentMap : EntityTypeConfiguration<TErp_Department>
    {
        public TErp_DepartmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DeptName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DeptType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DeptIcon)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DeptMaster)
                .HasMaxLength(50);

            this.Property(t => t.DeptTel)
                .HasMaxLength(50);

            this.Property(t => t.DeptFax)
                .HasMaxLength(50);

            this.Property(t => t.DeptAddr)
                .HasMaxLength(255);

            this.Property(t => t.DeptEmail)
                .HasMaxLength(50);

            this.Property(t => t.DeptDesc)
                .HasMaxLength(255);

            this.Property(t => t.CreateBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EditBy)
                .HasMaxLength(50);

            this.Property(t => t.DeleteBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TErp_Department");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DeptName).HasColumnName("DeptName");
            this.Property(t => t.DeptType).HasColumnName("DeptType");
            this.Property(t => t.DeptIcon).HasColumnName("DeptIcon");
            this.Property(t => t.DeptMaster).HasColumnName("DeptMaster");
            this.Property(t => t.DeptTel).HasColumnName("DeptTel");
            this.Property(t => t.DeptFax).HasColumnName("DeptFax");
            this.Property(t => t.DeptAddr).HasColumnName("DeptAddr");
            this.Property(t => t.DeptEmail).HasColumnName("DeptEmail");
            this.Property(t => t.DeptDesc).HasColumnName("DeptDesc");
            this.Property(t => t.SeqNo).HasColumnName("SeqNo");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.EditBy).HasColumnName("EditBy");
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.DeleteBy).HasColumnName("DeleteBy");
            this.Property(t => t.DeleteTime).HasColumnName("DeleteTime");
        }
    }
}
