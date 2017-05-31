using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class Report_ZhiweistatisticsMonthMap : EntityTypeConfiguration<Report_ZhiweistatisticsMonth>
    {
        public Report_ZhiweistatisticsMonthMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Dptname)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Zhiweiname)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_ZhiweistatisticsMonth");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Dptname).HasColumnName("Dptname");
            this.Property(t => t.Zhiweiname).HasColumnName("Zhiweiname");
            this.Property(t => t.Querentime_couunt).HasColumnName("Querentime_couunt");
            this.Property(t => t.Xianchang_count).HasColumnName("Xianchang_count");
            this.Property(t => t.Phone_count).HasColumnName("Phone_count");
            this.Property(t => t.Luyong_count).HasColumnName("Luyong_count");
            this.Property(t => t.Taotai_count).HasColumnName("Taotai_count");
            this.Property(t => t.Beiyong_count).HasColumnName("Beiyong_count");
            this.Property(t => t.Daihuifu_count).HasColumnName("Daihuifu_count");
            this.Property(t => t.Phonetongzhi_count).HasColumnName("Phonetongzhi_count");
            this.Property(t => t.Mailtongzhi_count).HasColumnName("Mailtongzhi_count");
            this.Property(t => t.Createon).HasColumnName("Createon");
        }
    }
}
