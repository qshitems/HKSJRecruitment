using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HKSJRecruitment.Models.Models.Mapping
{
    public class TErp_PositionMap : EntityTypeConfiguration<TErp_Position>
    {
        public TErp_PositionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PositionName)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.PositionDesc)
                .HasMaxLength(2000);

            this.Property(t => t.PositionLevel)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CreateBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EditBy)
                .HasMaxLength(50);

            this.Property(t => t.DeleteBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TErp_Position");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PositionName).HasColumnName("PositionName");
            this.Property(t => t.PositionDesc).HasColumnName("PositionDesc");
            this.Property(t => t.PositionLevel).HasColumnName("PositionLevel");
            this.Property(t => t.SeqNo).HasColumnName("SeqNo");
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
