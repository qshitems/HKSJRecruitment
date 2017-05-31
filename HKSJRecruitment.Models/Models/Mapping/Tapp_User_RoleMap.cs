using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class Tapp_User_RoleMap : EntityTypeConfiguration<Tapp_User_Role>
    {
        public Tapp_User_RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Tapp_User_Role");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");

            // Relationships
            this.HasRequired(t => t.Tapp_Role)
                .WithMany(t => t.Tapp_User_Role)
                .HasForeignKey(d => d.RoleId);
            this.HasRequired(t => t.Tapp_User)
                .WithMany(t => t.Tapp_User_Role)
                .HasForeignKey(d => d.UserId);

        }
    }
}
