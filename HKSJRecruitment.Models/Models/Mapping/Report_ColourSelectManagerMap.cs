using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class Report_ColourSelectManagerMap : EntityTypeConfiguration<Report_ColourSelectManager>
    {
        public Report_ColourSelectManagerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Manager)
                .HasMaxLength(50);

            this.Property(t => t.Colour)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Report_ColourSelectManager");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Manager).HasColumnName("Manager");
            this.Property(t => t.Colour).HasColumnName("Colour");
            this.Property(t => t.Sort).HasColumnName("Sort");
        }
    }
}
