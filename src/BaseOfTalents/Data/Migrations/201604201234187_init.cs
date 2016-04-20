namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        IsMale = c.Boolean(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        Email = c.String(),
                        Skype = c.String(),
                        PositionDesired = c.String(),
                        SalaryDesired = c.Int(nullable: false),
                        TypeOfEmployment = c.Int(nullable: false),
                        StartExperience = c.DateTime(nullable: false),
                        Practice = c.String(),
                        Description = c.String(),
                        RelocationAgreement = c.Boolean(nullable: false),
                        Education = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Industry_Id = c.Int(),
                        Location_Id = c.Int(),
                        Photo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Industries", t => t.Industry_Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .Index(t => t.Industry_Id)
                .Index(t => t.Location_Id)
                .Index(t => t.Photo_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        CommentType = c.Int(nullable: false),
                        RelativeId = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(),
                        Vacancy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.Files",
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
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.Industries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LanguageSkills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageLevel = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Language_Id = c.Int(),
                        Candidate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.Language_Id)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .Index(t => t.Language_Id)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Country_Id = c.Int(),
                        Vacancy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id)
                .Index(t => t.Country_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhoneNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Photos",
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
                "dbo.Skills",
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
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.CandidateSocials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        SocialNetwork_Id = c.Int(),
                        Candidate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocialNetworks", t => t.SocialNetwork_Id)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .Index(t => t.SocialNetwork_Id)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.SocialNetworks",
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
                "dbo.CandidateSources",
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
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.Tags",
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
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.VacancyStageInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(),
                        Comment_Id = c.Int(),
                        Vacancy_Id = c.Int(),
                        VacancyStage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .ForeignKey("dbo.Comments", t => t.Comment_Id)
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id)
                .ForeignKey("dbo.VacancyStages", t => t.VacancyStage_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Comment_Id)
                .Index(t => t.Vacancy_Id)
                .Index(t => t.VacancyStage_Id);
            
            CreateTable(
                "dbo.VacancyStages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        IsCommentRequired = c.Boolean(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Stage_Id = c.Int(),
                        Vacacny_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stages", t => t.Stage_Id)
                .ForeignKey("dbo.Vacancies", t => t.Vacacny_Id)
                .Index(t => t.Stage_Id)
                .Index(t => t.Vacacny_Id);
            
            CreateTable(
                "dbo.Stages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vacancies",
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
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Department_Id = c.Int(),
                        Industry_Id = c.Int(),
                        LanguageSkill_Id = c.Int(),
                        ParentVacancy_Id = c.Int(),
                        Responsible_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .ForeignKey("dbo.Industries", t => t.Industry_Id)
                .ForeignKey("dbo.LanguageSkills", t => t.LanguageSkill_Id)
                .ForeignKey("dbo.Vacancies", t => t.ParentVacancy_Id)
                .ForeignKey("dbo.Users", t => t.Responsible_Id)
                .Index(t => t.Department_Id)
                .Index(t => t.Industry_Id)
                .Index(t => t.LanguageSkill_Id)
                .Index(t => t.ParentVacancy_Id)
                .Index(t => t.Responsible_Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        DepartmentGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DepartmentGroups", t => t.DepartmentGroup_Id)
                .Index(t => t.DepartmentGroup_Id);
            
            CreateTable(
                "dbo.DepartmentGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        isMale = c.Boolean(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        Email = c.String(),
                        Skype = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Location_Id = c.Int(),
                        Photo_Id = c.Int(),
                        Role_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .ForeignKey("dbo.Roles", t => t.Role_Id)
                .Index(t => t.Location_Id)
                .Index(t => t.Photo_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        AccessRights = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Role_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.Role_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VacancyStageInfoes", "VacancyStage_Id", "dbo.VacancyStages");
            DropForeignKey("dbo.VacancyStages", "Vacacny_Id", "dbo.Vacancies");
            DropForeignKey("dbo.Tags", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.Vacancies", "Responsible_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.Permissions", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.Users", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.PhoneNumbers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Skills", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.Vacancies", "ParentVacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.Locations", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.Vacancies", "LanguageSkill_Id", "dbo.LanguageSkills");
            DropForeignKey("dbo.Vacancies", "Industry_Id", "dbo.Industries");
            DropForeignKey("dbo.Files", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.Vacancies", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Departments", "DepartmentGroup_Id", "dbo.DepartmentGroups");
            DropForeignKey("dbo.Comments", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.VacancyStageInfoes", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.VacancyStages", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.VacancyStageInfoes", "Comment_Id", "dbo.Comments");
            DropForeignKey("dbo.VacancyStageInfoes", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.Tags", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.CandidateSources", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.CandidateSocials", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.CandidateSocials", "SocialNetwork_Id", "dbo.SocialNetworks");
            DropForeignKey("dbo.Skills", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.Candidates", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.PhoneNumbers", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.Candidates", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Locations", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.LanguageSkills", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.LanguageSkills", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.Candidates", "Industry_Id", "dbo.Industries");
            DropForeignKey("dbo.Files", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.Comments", "Candidate_Id", "dbo.Candidates");
            DropIndex("dbo.Permissions", new[] { "Role_Id" });
            DropIndex("dbo.Users", new[] { "Role_Id" });
            DropIndex("dbo.Users", new[] { "Photo_Id" });
            DropIndex("dbo.Users", new[] { "Location_Id" });
            DropIndex("dbo.Departments", new[] { "DepartmentGroup_Id" });
            DropIndex("dbo.Vacancies", new[] { "Responsible_Id" });
            DropIndex("dbo.Vacancies", new[] { "ParentVacancy_Id" });
            DropIndex("dbo.Vacancies", new[] { "LanguageSkill_Id" });
            DropIndex("dbo.Vacancies", new[] { "Industry_Id" });
            DropIndex("dbo.Vacancies", new[] { "Department_Id" });
            DropIndex("dbo.VacancyStages", new[] { "Vacacny_Id" });
            DropIndex("dbo.VacancyStages", new[] { "Stage_Id" });
            DropIndex("dbo.VacancyStageInfoes", new[] { "VacancyStage_Id" });
            DropIndex("dbo.VacancyStageInfoes", new[] { "Vacancy_Id" });
            DropIndex("dbo.VacancyStageInfoes", new[] { "Comment_Id" });
            DropIndex("dbo.VacancyStageInfoes", new[] { "Candidate_Id" });
            DropIndex("dbo.Tags", new[] { "Vacancy_Id" });
            DropIndex("dbo.Tags", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateSources", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateSocials", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateSocials", new[] { "SocialNetwork_Id" });
            DropIndex("dbo.Skills", new[] { "Vacancy_Id" });
            DropIndex("dbo.Skills", new[] { "Candidate_Id" });
            DropIndex("dbo.PhoneNumbers", new[] { "User_Id" });
            DropIndex("dbo.PhoneNumbers", new[] { "Candidate_Id" });
            DropIndex("dbo.Locations", new[] { "Vacancy_Id" });
            DropIndex("dbo.Locations", new[] { "Country_Id" });
            DropIndex("dbo.LanguageSkills", new[] { "Candidate_Id" });
            DropIndex("dbo.LanguageSkills", new[] { "Language_Id" });
            DropIndex("dbo.Files", new[] { "Vacancy_Id" });
            DropIndex("dbo.Files", new[] { "Candidate_Id" });
            DropIndex("dbo.Comments", new[] { "Vacancy_Id" });
            DropIndex("dbo.Comments", new[] { "Candidate_Id" });
            DropIndex("dbo.Candidates", new[] { "Photo_Id" });
            DropIndex("dbo.Candidates", new[] { "Location_Id" });
            DropIndex("dbo.Candidates", new[] { "Industry_Id" });
            DropTable("dbo.Permissions");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.DepartmentGroups");
            DropTable("dbo.Departments");
            DropTable("dbo.Vacancies");
            DropTable("dbo.Stages");
            DropTable("dbo.VacancyStages");
            DropTable("dbo.VacancyStageInfoes");
            DropTable("dbo.Tags");
            DropTable("dbo.CandidateSources");
            DropTable("dbo.SocialNetworks");
            DropTable("dbo.CandidateSocials");
            DropTable("dbo.Skills");
            DropTable("dbo.Photos");
            DropTable("dbo.PhoneNumbers");
            DropTable("dbo.Countries");
            DropTable("dbo.Locations");
            DropTable("dbo.Languages");
            DropTable("dbo.LanguageSkills");
            DropTable("dbo.Industries");
            DropTable("dbo.Files");
            DropTable("dbo.Comments");
            DropTable("dbo.Candidates");
        }
    }
}
