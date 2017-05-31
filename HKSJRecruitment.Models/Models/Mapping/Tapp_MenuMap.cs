using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class Tapp_MenuMap : EntityTypeConfiguration<Tapp_Menu>
    {
        public Tapp_MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MenuCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.MenuText)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.MenuUrl)
                .HasMaxLength(100);

            this.Property(t => t.MenuIcon)
                .HasMaxLength(100);

            this.Property(t => t.MenuClass)
                .HasMaxLength(100);

            this.Property(t => t.Remarks)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Tapp_Menu");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuCode).HasColumnName("MenuCode");
            this.Property(t => t.MenuText).HasColumnName("MenuText");
            this.Property(t => t.MenuUrl).HasColumnName("MenuUrl");
            this.Property(t => t.MenuIcon).HasColumnName("MenuIcon");
            this.Property(t => t.MenuClass).HasColumnName("MenuClass");
            this.Property(t => t.SeqNo).HasColumnName("SeqNo");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.IsShow).HasColumnName("IsShow");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
        }
    }
}
