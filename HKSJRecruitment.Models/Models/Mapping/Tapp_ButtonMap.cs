using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class Tapp_ButtonMap : EntityTypeConfiguration<Tapp_Button>
    {
        public Tapp_ButtonMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ButtonText)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HandlerUrl)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ButtonJsId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ButtonIcon)
                .HasMaxLength(50);

            this.Property(t => t.ButtonClass)
                .HasMaxLength(50);

            this.Property(t => t.Remarks)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Tapp_Button");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.ButtonText).HasColumnName("ButtonText");
            this.Property(t => t.HandlerUrl).HasColumnName("HandlerUrl");
            this.Property(t => t.ButtonJsId).HasColumnName("ButtonJsId");
            this.Property(t => t.ButtonIcon).HasColumnName("ButtonIcon");
            this.Property(t => t.ButtonClass).HasColumnName("ButtonClass");
            this.Property(t => t.SeqNo).HasColumnName("SeqNo");
            this.Property(t => t.IsShow).HasColumnName("IsShow");
            this.Property(t => t.Remarks).HasColumnName("Remarks");

            // Relationships
            this.HasRequired(t => t.Tapp_Menu)
                .WithMany(t => t.Tapp_Button)
                .HasForeignKey(d => d.MenuId);

        }
    }
}
