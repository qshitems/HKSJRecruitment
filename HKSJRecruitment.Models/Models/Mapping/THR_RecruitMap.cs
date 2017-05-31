using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class THR_RecruitMap : EntityTypeConfiguration<THR_Recruit>
    {
        public THR_RecruitMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Tel)
                .HasMaxLength(50);

            this.Property(t => t.Interviewer)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(250);

            this.Property(t => t.Userurl)
                .HasMaxLength(1000);

            this.Property(t => t.Email)
                .HasMaxLength(50);

            this.Property(t => t.CreateBy)
                .HasMaxLength(50);

            this.Property(t => t.EditBy)
                .HasMaxLength(50);

            this.Property(t => t.DeleteBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("THR_Recruit");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Tel).HasColumnName("Tel");
            this.Property(t => t.DptId).HasColumnName("DptId");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Interviewer).HasColumnName("Interviewer");
            this.Property(t => t.Interview).HasColumnName("Interview");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Userurl).HasColumnName("Userurl");
            this.Property(t => t.HireTime).HasColumnName("HireTime");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.NeedsId).HasColumnName("NeedsId");
            this.Property(t => t.EntryType).HasColumnName("EntryType");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.EditBy).HasColumnName("EditBy");
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.DeleteBy).HasColumnName("DeleteBy");
            this.Property(t => t.DeleteTime).HasColumnName("DeleteTime");

            // Relationships
            this.HasOptional(t => t.Tapp_Param)
                .WithMany(t => t.THR_Recruit)
                .HasForeignKey(d => d.Status);
            this.HasOptional(t => t.TErp_Department)
                .WithMany(t => t.THR_Recruit)
                .HasForeignKey(d => d.DptId);
            this.HasOptional(t => t.TErp_Position)
                .WithMany(t => t.THR_Recruit)
                .HasForeignKey(d => d.PostId);
            this.HasOptional(t => t.THR_Needs)
                .WithMany(t => t.THR_Recruit)
                .HasForeignKey(d => d.NeedsId);

        }
    }
}
