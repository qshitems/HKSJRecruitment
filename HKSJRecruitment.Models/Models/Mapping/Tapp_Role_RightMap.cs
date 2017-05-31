using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class Tapp_Role_RightMap : EntityTypeConfiguration<Tapp_Role_Right>
    {
        public Tapp_Role_RightMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Tapp_Role_Right");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.ButtonId).HasColumnName("ButtonId");

            // Relationships
            this.HasRequired(t => t.Tapp_Menu)
                .WithMany(t => t.Tapp_Role_Right)
                .HasForeignKey(d => d.MenuId);
            this.HasRequired(t => t.Tapp_Role)
                .WithMany(t => t.Tapp_Role_Right)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
