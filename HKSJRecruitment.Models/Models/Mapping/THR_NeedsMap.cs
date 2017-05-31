using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class THR_NeedsMap : EntityTypeConfiguration<THR_Needs>
    {
        public THR_NeedsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Demander)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Remarks)
                .HasMaxLength(500);

            this.Property(t => t.CreateBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EditBy)
                .HasMaxLength(50);

            this.Property(t => t.DeleteBy)
                .HasMaxLength(50);

            this.Property(t => t.FileWord)
                .HasMaxLength(250);

            this.Property(t => t.PostRequest)
                .HasMaxLength(500);

            this.Property(t => t.JobResponsibility)
                .HasMaxLength(500);

            this.Property(t => t.InterviewAddress)
                .HasMaxLength(200);

            this.Property(t => t.Principal)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("THR_Needs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DeptId).HasColumnName("DeptId");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.NeedQuantity).HasColumnName("NeedQuantity");
            this.Property(t => t.HaveBeenQuantity).HasColumnName("HaveBeenQuantity");
            this.Property(t => t.Demander).HasColumnName("Demander");
            this.Property(t => t.CutTime).HasColumnName("CutTime");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.EditBy).HasColumnName("EditBy");
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            this.Property(t => t.DeleteBy).HasColumnName("DeleteBy");
            this.Property(t => t.DeleteTime).HasColumnName("DeleteTime");
            this.Property(t => t.FileWord).HasColumnName("FileWord");
            this.Property(t => t.PostRequest).HasColumnName("PostRequest");
            this.Property(t => t.JobResponsibility).HasColumnName("JobResponsibility");
            this.Property(t => t.IsHaveBeen).HasColumnName("IsHaveBeen");
            this.Property(t => t.InterviewAddress).HasColumnName("InterviewAddress");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.NeedsPostId).HasColumnName("NeedsPostId");

            // Relationships
            this.HasRequired(t => t.TErp_Department)
                .WithMany(t => t.THR_Needs)
                .HasForeignKey(d => d.DeptId);
            this.HasRequired(t => t.TErp_Position)
                .WithMany(t => t.THR_Needs)
                .HasForeignKey(d => d.PostId);

        }
    }
}
