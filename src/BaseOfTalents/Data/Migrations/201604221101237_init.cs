namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        IsMale = c.Boolean(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        Email = c.String(nullable: false),
                        Skype = c.String(),
                        PositionDesired = c.String(nullable: false),
                        SalaryDesired = c.Int(nullable: false),
                        TypeOfEmployment = c.Int(nullable: false),
                        StartExperience = c.DateTime(nullable: false),
                        Practice = c.String(),
                        Description = c.String(),
                        LocationId = c.Int(nullable: false),
                        RelocationAgreement = c.Boolean(nullable: false),
                        Education = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Industry_Id = c.Int(),
                        Photo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Industry", t => t.Industry_Id)
                .ForeignKey("dbo.Location", t => t.LocationId)
                .ForeignKey("dbo.Photo", t => t.Photo_Id)
                .Index(t => t.LocationId)
                .Index(t => t.Industry_Id)
                .Index(t => t.Photo_Id);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        Description = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(),
                        Vacancy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.Industry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LanguageSkill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageLevel = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Language", t => t.LanguageId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        CountryId = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhoneNumber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        Description = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Skill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CandidateSocial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(nullable: false),
                        SocialNetworkId = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocialNetwork", t => t.SocialNetworkId)
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .Index(t => t.SocialNetworkId)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.SocialNetwork",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ImagePath = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CandidateSource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.Int(nullable: false),
                        Path = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(),
                        Vacancy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.VacancyStageInfo",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CandidateId = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Comment_Id = c.Int(),
                        Vacancy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comment", t => t.Comment_Id)
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .ForeignKey("dbo.VacancyStage", t => t.Id)
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .Index(t => t.Id)
                .Index(t => t.CandidateId)
                .Index(t => t.Comment_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.VacancyStage",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        IsCommentRequired = c.Boolean(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stage", t => t.Id)
                .ForeignKey("dbo.Vacancy", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Stage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vacancy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        SalaryMin = c.Int(nullable: false),
                        SalaryMax = c.Int(nullable: false),
                        TypeOfEmployment = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        DeadlineDate = c.DateTime(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        LanguageSkillId = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Industry_Id = c.Int(),
                        ParentVacancy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .ForeignKey("dbo.Industry", t => t.Industry_Id)
                .ForeignKey("dbo.LanguageSkill", t => t.LanguageSkillId)
                .ForeignKey("dbo.Vacancy", t => t.ParentVacancy_Id)
                .ForeignKey("dbo.User", t => t.ResponsibleId)
                .Index(t => t.DepartmentId)
                .Index(t => t.ResponsibleId)
                .Index(t => t.LanguageSkillId)
                .Index(t => t.Industry_Id)
                .Index(t => t.ParentVacancy_Id);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        DepartmentGroupId = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DepartmentGroup", t => t.DepartmentGroupId)
                .Index(t => t.DepartmentGroupId);
            
            CreateTable(
                "dbo.DepartmentGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Level",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Vacancy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        isMale = c.Boolean(nullable: false),
                        BirthDate = c.DateTime(),
                        Email = c.String(nullable: false),
                        Skype = c.String(),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        RoleId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Photo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Location", t => t.LocationId)
                .ForeignKey("dbo.Photo", t => t.Photo_Id)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.LocationId)
                .Index(t => t.Photo_Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        AccessRights = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventDate = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        EventTypeId = c.Int(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(),
                        Vacancy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .ForeignKey("dbo.EventType", t => t.EventTypeId)
                .ForeignKey("dbo.User", t => t.ResponsibleId)
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .Index(t => t.EventTypeId)
                .Index(t => t.ResponsibleId)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.EventType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ImagePath = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CandidateComment",
                c => new
                    {
                        Candidate_Id = c.Int(nullable: false),
                        Comment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Candidate_Id, t.Comment_Id })
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .ForeignKey("dbo.Comment", t => t.Comment_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Comment_Id);
            
            CreateTable(
                "dbo.CandidateLanguageSkill",
                c => new
                    {
                        Candidate_Id = c.Int(nullable: false),
                        LanguageSkill_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Candidate_Id, t.LanguageSkill_Id })
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .ForeignKey("dbo.LanguageSkill", t => t.LanguageSkill_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.LanguageSkill_Id);
            
            CreateTable(
                "dbo.CandidatePhoneNumber",
                c => new
                    {
                        Candidate_Id = c.Int(nullable: false),
                        PhoneNumber_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Candidate_Id, t.PhoneNumber_Id })
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .ForeignKey("dbo.PhoneNumber", t => t.PhoneNumber_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.PhoneNumber_Id);
            
            CreateTable(
                "dbo.CandidateSkill",
                c => new
                    {
                        Candidate_Id = c.Int(nullable: false),
                        Skill_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Candidate_Id, t.Skill_Id })
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .ForeignKey("dbo.Skill", t => t.Skill_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Skill_Id);
            
            CreateTable(
                "dbo.VacancyStageComment",
                c => new
                    {
                        VacancyStage_Id = c.Int(nullable: false),
                        Comment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacancyStage_Id, t.Comment_Id })
                .ForeignKey("dbo.VacancyStage", t => t.VacancyStage_Id)
                .ForeignKey("dbo.Comment", t => t.Comment_Id)
                .Index(t => t.VacancyStage_Id)
                .Index(t => t.Comment_Id);
            
            CreateTable(
                "dbo.VacancyComment",
                c => new
                    {
                        Vacancy_Id = c.Int(nullable: false),
                        Comment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vacancy_Id, t.Comment_Id })
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .ForeignKey("dbo.Comment", t => t.Comment_Id)
                .Index(t => t.Vacancy_Id)
                .Index(t => t.Comment_Id);
            
            CreateTable(
                "dbo.VacancyLocation",
                c => new
                    {
                        Vacancy_Id = c.Int(nullable: false),
                        Location_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vacancy_Id, t.Location_Id })
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .ForeignKey("dbo.Location", t => t.Location_Id)
                .Index(t => t.Vacancy_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.VacancySkill",
                c => new
                    {
                        Vacancy_Id = c.Int(nullable: false),
                        Skill_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vacancy_Id, t.Skill_Id })
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .ForeignKey("dbo.Skill", t => t.Skill_Id)
                .Index(t => t.Vacancy_Id)
                .Index(t => t.Skill_Id);
            
            CreateTable(
                "dbo.UserPhoneNumber",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        PhoneNumber_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.PhoneNumber_Id })
                .ForeignKey("dbo.User", t => t.User_Id)
                .ForeignKey("dbo.PhoneNumber", t => t.PhoneNumber_Id)
                .Index(t => t.User_Id)
                .Index(t => t.PhoneNumber_Id);
            
            CreateTable(
                "dbo.PermissionRole",
                c => new
                    {
                        Permission_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Permission_Id, t.Role_Id })
                .ForeignKey("dbo.Permission", t => t.Permission_Id)
                .ForeignKey("dbo.Role", t => t.Role_Id)
                .Index(t => t.Permission_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Event", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.Event", "ResponsibleId", "dbo.User");
            DropForeignKey("dbo.Event", "EventTypeId", "dbo.EventType");
            DropForeignKey("dbo.Event", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.VacancyStageInfo", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.VacancyStageInfo", "Id", "dbo.VacancyStage");
            DropForeignKey("dbo.VacancyStage", "Id", "dbo.Vacancy");
            DropForeignKey("dbo.Tag", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "ResponsibleId", "dbo.User");
            DropForeignKey("dbo.User", "RoleId", "dbo.Role");
            DropForeignKey("dbo.PermissionRole", "Role_Id", "dbo.Role");
            DropForeignKey("dbo.PermissionRole", "Permission_Id", "dbo.Permission");
            DropForeignKey("dbo.User", "Photo_Id", "dbo.Photo");
            DropForeignKey("dbo.UserPhoneNumber", "PhoneNumber_Id", "dbo.PhoneNumber");
            DropForeignKey("dbo.UserPhoneNumber", "User_Id", "dbo.User");
            DropForeignKey("dbo.User", "LocationId", "dbo.Location");
            DropForeignKey("dbo.VacancySkill", "Skill_Id", "dbo.Skill");
            DropForeignKey("dbo.VacancySkill", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "ParentVacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyLocation", "Location_Id", "dbo.Location");
            DropForeignKey("dbo.VacancyLocation", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.Level", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "LanguageSkillId", "dbo.LanguageSkill");
            DropForeignKey("dbo.Vacancy", "Industry_Id", "dbo.Industry");
            DropForeignKey("dbo.File", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.Department", "DepartmentGroupId", "dbo.DepartmentGroup");
            DropForeignKey("dbo.VacancyComment", "Comment_Id", "dbo.Comment");
            DropForeignKey("dbo.VacancyComment", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyStageInfo", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyStage", "Id", "dbo.Stage");
            DropForeignKey("dbo.VacancyStageComment", "Comment_Id", "dbo.Comment");
            DropForeignKey("dbo.VacancyStageComment", "VacancyStage_Id", "dbo.VacancyStage");
            DropForeignKey("dbo.VacancyStageInfo", "Comment_Id", "dbo.Comment");
            DropForeignKey("dbo.Tag", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.CandidateSource", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.CandidateSocial", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.CandidateSocial", "SocialNetworkId", "dbo.SocialNetwork");
            DropForeignKey("dbo.CandidateSkill", "Skill_Id", "dbo.Skill");
            DropForeignKey("dbo.CandidateSkill", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.Candidate", "Photo_Id", "dbo.Photo");
            DropForeignKey("dbo.CandidatePhoneNumber", "PhoneNumber_Id", "dbo.PhoneNumber");
            DropForeignKey("dbo.CandidatePhoneNumber", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.Candidate", "LocationId", "dbo.Location");
            DropForeignKey("dbo.Location", "CountryId", "dbo.Country");
            DropForeignKey("dbo.CandidateLanguageSkill", "LanguageSkill_Id", "dbo.LanguageSkill");
            DropForeignKey("dbo.CandidateLanguageSkill", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.LanguageSkill", "LanguageId", "dbo.Language");
            DropForeignKey("dbo.Candidate", "Industry_Id", "dbo.Industry");
            DropForeignKey("dbo.File", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.CandidateComment", "Comment_Id", "dbo.Comment");
            DropForeignKey("dbo.CandidateComment", "Candidate_Id", "dbo.Candidate");
            DropIndex("dbo.PermissionRole", new[] { "Role_Id" });
            DropIndex("dbo.PermissionRole", new[] { "Permission_Id" });
            DropIndex("dbo.UserPhoneNumber", new[] { "PhoneNumber_Id" });
            DropIndex("dbo.UserPhoneNumber", new[] { "User_Id" });
            DropIndex("dbo.VacancySkill", new[] { "Skill_Id" });
            DropIndex("dbo.VacancySkill", new[] { "Vacancy_Id" });
            DropIndex("dbo.VacancyLocation", new[] { "Location_Id" });
            DropIndex("dbo.VacancyLocation", new[] { "Vacancy_Id" });
            DropIndex("dbo.VacancyComment", new[] { "Comment_Id" });
            DropIndex("dbo.VacancyComment", new[] { "Vacancy_Id" });
            DropIndex("dbo.VacancyStageComment", new[] { "Comment_Id" });
            DropIndex("dbo.VacancyStageComment", new[] { "VacancyStage_Id" });
            DropIndex("dbo.CandidateSkill", new[] { "Skill_Id" });
            DropIndex("dbo.CandidateSkill", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidatePhoneNumber", new[] { "PhoneNumber_Id" });
            DropIndex("dbo.CandidatePhoneNumber", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateLanguageSkill", new[] { "LanguageSkill_Id" });
            DropIndex("dbo.CandidateLanguageSkill", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateComment", new[] { "Comment_Id" });
            DropIndex("dbo.CandidateComment", new[] { "Candidate_Id" });
            DropIndex("dbo.Event", new[] { "Vacancy_Id" });
            DropIndex("dbo.Event", new[] { "Candidate_Id" });
            DropIndex("dbo.Event", new[] { "ResponsibleId" });
            DropIndex("dbo.Event", new[] { "EventTypeId" });
            DropIndex("dbo.User", new[] { "Photo_Id" });
            DropIndex("dbo.User", new[] { "LocationId" });
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.Level", new[] { "Vacancy_Id" });
            DropIndex("dbo.Department", new[] { "DepartmentGroupId" });
            DropIndex("dbo.Vacancy", new[] { "ParentVacancy_Id" });
            DropIndex("dbo.Vacancy", new[] { "Industry_Id" });
            DropIndex("dbo.Vacancy", new[] { "LanguageSkillId" });
            DropIndex("dbo.Vacancy", new[] { "ResponsibleId" });
            DropIndex("dbo.Vacancy", new[] { "DepartmentId" });
            DropIndex("dbo.VacancyStage", new[] { "Id" });
            DropIndex("dbo.VacancyStageInfo", new[] { "Vacancy_Id" });
            DropIndex("dbo.VacancyStageInfo", new[] { "Comment_Id" });
            DropIndex("dbo.VacancyStageInfo", new[] { "CandidateId" });
            DropIndex("dbo.VacancyStageInfo", new[] { "Id" });
            DropIndex("dbo.Tag", new[] { "Vacancy_Id" });
            DropIndex("dbo.Tag", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateSource", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateSocial", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateSocial", new[] { "SocialNetworkId" });
            DropIndex("dbo.Location", new[] { "CountryId" });
            DropIndex("dbo.LanguageSkill", new[] { "LanguageId" });
            DropIndex("dbo.File", new[] { "Vacancy_Id" });
            DropIndex("dbo.File", new[] { "Candidate_Id" });
            DropIndex("dbo.Candidate", new[] { "Photo_Id" });
            DropIndex("dbo.Candidate", new[] { "Industry_Id" });
            DropIndex("dbo.Candidate", new[] { "LocationId" });
            DropTable("dbo.PermissionRole");
            DropTable("dbo.UserPhoneNumber");
            DropTable("dbo.VacancySkill");
            DropTable("dbo.VacancyLocation");
            DropTable("dbo.VacancyComment");
            DropTable("dbo.VacancyStageComment");
            DropTable("dbo.CandidateSkill");
            DropTable("dbo.CandidatePhoneNumber");
            DropTable("dbo.CandidateLanguageSkill");
            DropTable("dbo.CandidateComment");
            DropTable("dbo.EventType");
            DropTable("dbo.Event");
            DropTable("dbo.Permission");
            DropTable("dbo.Role");
            DropTable("dbo.User");
            DropTable("dbo.Level");
            DropTable("dbo.DepartmentGroup");
            DropTable("dbo.Department");
            DropTable("dbo.Vacancy");
            DropTable("dbo.Stage");
            DropTable("dbo.VacancyStage");
            DropTable("dbo.VacancyStageInfo");
            DropTable("dbo.Tag");
            DropTable("dbo.CandidateSource");
            DropTable("dbo.SocialNetwork");
            DropTable("dbo.CandidateSocial");
            DropTable("dbo.Skill");
            DropTable("dbo.Photo");
            DropTable("dbo.PhoneNumber");
            DropTable("dbo.Country");
            DropTable("dbo.Location");
            DropTable("dbo.Language");
            DropTable("dbo.LanguageSkill");
            DropTable("dbo.Industry");
            DropTable("dbo.File");
            DropTable("dbo.Comment");
            DropTable("dbo.Candidate");
        }
    }
}
