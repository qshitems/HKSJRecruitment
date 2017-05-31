using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class HR_EmployeeMap : EntityTypeConfiguration<HR_Employee>
    {
        public HR_EmployeeMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.uid)
                .HasMaxLength(50);

            this.Property(t => t.pwd)
                .HasMaxLength(50);

            this.Property(t => t.name)
                .HasMaxLength(50);

            this.Property(t => t.idcard)
                .HasMaxLength(50);

            this.Property(t => t.birthday)
                .HasMaxLength(50);

            this.Property(t => t.dptname)
                .HasMaxLength(50);

            this.Property(t => t.post)
                .HasMaxLength(250);

            this.Property(t => t.email)
                .HasMaxLength(50);

            this.Property(t => t.sex)
                .HasMaxLength(50);

            this.Property(t => t.tel)
                .HasMaxLength(50);

            this.Property(t => t.status)
                .HasMaxLength(50);

            this.Property(t => t.zhiwu)
                .HasMaxLength(50);

            this.Property(t => t.EntryDate)
                .HasMaxLength(50);

            this.Property(t => t.address)
                .HasMaxLength(255);

            this.Property(t => t.remarks)
                .HasMaxLength(255);

            this.Property(t => t.education)
                .HasMaxLength(50);

            this.Property(t => t.level)
                .HasMaxLength(50);

            this.Property(t => t.professional)
                .HasMaxLength(50);

            this.Property(t => t.schools)
                .HasMaxLength(50);

            this.Property(t => t.title)
                .HasMaxLength(50);

            this.Property(t => t.portal)
                .HasMaxLength(250);

            this.Property(t => t.theme)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("HR_Employee");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.uid).HasColumnName("uid");
            this.Property(t => t.pwd).HasColumnName("pwd");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.idcard).HasColumnName("idcard");
            this.Property(t => t.birthday).HasColumnName("birthday");
            this.Property(t => t.dptid).HasColumnName("dptid");
            this.Property(t => t.dptname).HasColumnName("dptname");
            this.Property(t => t.postid).HasColumnName("postid");
            this.Property(t => t.post).HasColumnName("post");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.sex).HasColumnName("sex");
            this.Property(t => t.tel).HasColumnName("tel");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.zhiwuid).HasColumnName("zhiwuid");
            this.Property(t => t.zhiwu).HasColumnName("zhiwu");
            this.Property(t => t.sort).HasColumnName("sort");
            this.Property(t => t.EntryDate).HasColumnName("EntryDate");
            this.Property(t => t.address).HasColumnName("address");
            this.Property(t => t.remarks).HasColumnName("remarks");
            this.Property(t => t.education).HasColumnName("education");
            this.Property(t => t.level).HasColumnName("level");
            this.Property(t => t.professional).HasColumnName("professional");
            this.Property(t => t.schools).HasColumnName("schools");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.isDelete).HasColumnName("isDelete");
            this.Property(t => t.Delete_time).HasColumnName("Delete_time");
            this.Property(t => t.portal).HasColumnName("portal");
            this.Property(t => t.theme).HasColumnName("theme");
            this.Property(t => t.canlogin).HasColumnName("canlogin");
        }
    }
}
