using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using HKSJRecruitment.Models.Models.Mapping;

namespace HKSJRecruitment.Models.Models
{
    public partial class HKSJRecruitmentContext : DbContext
    {
        static HKSJRecruitmentContext()
        {
            Database.SetInitializer<HKSJRecruitmentContext>(null);
        }

        public HKSJRecruitmentContext()
            : base("Name=HKSJRecruitmentContext")
        {
        }

        public DbSet<HR_Employee> HR_Employee { get; set; }
        public DbSet<Report_ColourSelectManager> Report_ColourSelectManager { get; set; }
        public DbSet<Report_Createstatisticsday> Report_Createstatisticsday { get; set; }
        public DbSet<Report_CreatestatisticsMonth> Report_CreatestatisticsMonth { get; set; }
        public DbSet<Report_Needs> Report_Needs { get; set; }
        public DbSet<Report_ZhiweistatisticsDay> Report_ZhiweistatisticsDay { get; set; }
        public DbSet<Report_ZhiweistatisticsMonth> Report_ZhiweistatisticsMonth { get; set; }
        public DbSet<Tapp_Button> Tapp_Button { get; set; }
        public DbSet<Tapp_Menu> Tapp_Menu { get; set; }
        public DbSet<Tapp_Param> Tapp_Param { get; set; }
        public DbSet<Tapp_ParamType> Tapp_ParamType { get; set; }
        public DbSet<Tapp_Role> Tapp_Role { get; set; }
        public DbSet<Tapp_Role_Right> Tapp_Role_Right { get; set; }
        public DbSet<Tapp_User> Tapp_User { get; set; }
        public DbSet<Tapp_User_Role> Tapp_User_Role { get; set; }
        public DbSet<TErp_Agileboy> TErp_Agileboy { get; set; }
        public DbSet<TErp_Department> TErp_Department { get; set; }
        public DbSet<TErp_Position> TErp_Position { get; set; }
        public DbSet<TErp_Post> TErp_Post { get; set; }
        public DbSet<THR_Needs> THR_Needs { get; set; }
        public DbSet<THR_Recruit> THR_Recruit { get; set; }
        public DbSet<Vapp_UserRight> Vapp_UserRight { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new HR_EmployeeMap());
            modelBuilder.Configurations.Add(new Report_ColourSelectManagerMap());
            modelBuilder.Configurations.Add(new Report_CreatestatisticsdayMap());
            modelBuilder.Configurations.Add(new Report_CreatestatisticsMonthMap());
            modelBuilder.Configurations.Add(new Report_NeedsMap());
            modelBuilder.Configurations.Add(new Report_ZhiweistatisticsDayMap());
            modelBuilder.Configurations.Add(new Report_ZhiweistatisticsMonthMap());
            modelBuilder.Configurations.Add(new Tapp_ButtonMap());
            modelBuilder.Configurations.Add(new Tapp_MenuMap());
            modelBuilder.Configurations.Add(new Tapp_ParamMap());
            modelBuilder.Configurations.Add(new Tapp_ParamTypeMap());
            modelBuilder.Configurations.Add(new Tapp_RoleMap());
            modelBuilder.Configurations.Add(new Tapp_Role_RightMap());
            modelBuilder.Configurations.Add(new Tapp_UserMap());
            modelBuilder.Configurations.Add(new Tapp_User_RoleMap());
            modelBuilder.Configurations.Add(new TErp_AgileboyMap());
            modelBuilder.Configurations.Add(new TErp_DepartmentMap());
            modelBuilder.Configurations.Add(new TErp_PositionMap());
            modelBuilder.Configurations.Add(new TErp_PostMap());
            modelBuilder.Configurations.Add(new THR_NeedsMap());
            modelBuilder.Configurations.Add(new THR_RecruitMap());
            modelBuilder.Configurations.Add(new Vapp_UserRightMap());
        }
    }
}
