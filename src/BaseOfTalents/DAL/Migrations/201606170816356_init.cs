namespace BaseOfTalents.DAL.Migrations
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
                        TypeOfEmployment = c.Int(nullable: false),
                        StartExperience = c.DateTime(nullable: false),
                        Practice = c.String(),
                        Description = c.String(),
                        SalaryDesired = c.Int(nullable: false),
                        CurrencyId = c.Int(),
                        RelocationAgreement = c.Boolean(nullable: false),
                        LocationId = c.Int(nullable: false),
                        LevelId = c.Int(),
                        IndustryId = c.Int(),
                        Education = c.String(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Photo_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currency", t => t.CurrencyId)
                .ForeignKey("dbo.Industry", t => t.IndustryId)
                .ForeignKey("dbo.Level", t => t.LevelId)
                .ForeignKey("dbo.City", t => t.LocationId)
                .ForeignKey("dbo.Photo", t => t.Photo_Id)
                .Index(t => t.CurrencyId)
                .Index(t => t.LocationId)
                .Index(t => t.LevelId)
                .Index(t => t.IndustryId)
                .Index(t => t.Photo_Id);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventDate = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        EventTypeId = c.Int(),
                        VacancyId = c.Int(),
                        CandidateId = c.Int(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventType", t => t.EventTypeId)
                .ForeignKey("dbo.User", t => t.ResponsibleId)
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .Index(t => t.ResponsibleId)
                .Index(t => t.EventTypeId)
                .Index(t => t.VacancyId)
                .Index(t => t.CandidateId);
            
            CreateTable(
                "dbo.EventType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ImagePath = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Photo_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.City", t => t.LocationId)
                .ForeignKey("dbo.Photo", t => t.Photo_Id)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.LocationId)
                .Index(t => t.Photo_Id);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        CountryId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RelocationPlace",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .Index(t => t.CountryId)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.PhoneNumber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(nullable: false),
                        Description = c.String(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        AccessRights = c.Int(nullable: false),
                        Group = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vacancy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        TypeOfEmployment = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        DeadlineDate = c.DateTime(),
                        ParentVacancyId = c.Int(),
                        IndustryId = c.Int(),
                        DepartmentId = c.Int(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        SalaryMin = c.Int(nullable: false),
                        SalaryMax = c.Int(nullable: false),
                        CurrencyId = c.Int(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        LanguageSkill_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currency", t => t.CurrencyId)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .ForeignKey("dbo.Industry", t => t.IndustryId)
                .ForeignKey("dbo.LanguageSkill", t => t.LanguageSkill_Id)
                .ForeignKey("dbo.Vacancy", t => t.ParentVacancyId)
                .ForeignKey("dbo.User", t => t.ResponsibleId)
                .Index(t => t.ParentVacancyId)
                .Index(t => t.IndustryId)
                .Index(t => t.DepartmentId)
                .Index(t => t.ResponsibleId)
                .Index(t => t.CurrencyId)
                .Index(t => t.LanguageSkill_Id);
            
            CreateTable(
                "dbo.VacancyStageInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        VacancyId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Comment_Id = c.Int(),
                        VacancyStage_Id = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comment", t => t.Comment_Id)
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.VacancyStage", t => t.VacancyStage_Id)
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .Index(t => t.CandidateId)
                .Index(t => t.VacancyId)
                .Index(t => t.Comment_Id)
                .Index(t => t.VacancyStage_Id);
            
            CreateTable(
                "dbo.VacancyStage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        IsCommentRequired = c.Boolean(nullable: false),
                        StageId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stage", t => t.StageId)
                .Index(t => t.StageId);
            
            CreateTable(
                "dbo.Stage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        DepartmentGroupId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DepartmentGroup", t => t.DepartmentGroupId)
                .Index(t => t.DepartmentGroupId);
            
            CreateTable(
                "dbo.DepartmentGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(nullable: false),
                        Description = c.String(),
                        Size = c.Long(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Vacancy_Id = c.Int(),
                        Candidate_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .Index(t => t.Vacancy_Id)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.Industry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LanguageSkill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageLevel = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
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
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Level",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Skill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CandidateSocial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(nullable: false),
                        SocialNetworkId = c.Int(nullable: false),
                        CandidateId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .ForeignKey("dbo.SocialNetwork", t => t.SocialNetworkId)
                .Index(t => t.SocialNetworkId)
                .Index(t => t.CandidateId);
            
            CreateTable(
                "dbo.SocialNetwork",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ImagePath = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CandidateSource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.Int(nullable: false),
                        Path = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.Note",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        UserId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CandidateToComment",
                c => new
                    {
                        CandidateId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CandidateId, t.CommentId })
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .Index(t => t.CandidateId)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.RelocationPlaceCity",
                c => new
                    {
                        RelocationPlace_Id = c.Int(nullable: false),
                        City_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RelocationPlace_Id, t.City_Id })
                .ForeignKey("dbo.RelocationPlace", t => t.RelocationPlace_Id)
                .ForeignKey("dbo.City", t => t.City_Id)
                .Index(t => t.RelocationPlace_Id)
                .Index(t => t.City_Id);
            
            CreateTable(
                "dbo.UserToPhoneNumber",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        PhoneNumberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PhoneNumberId })
                .ForeignKey("dbo.User", t => t.UserId)
                .ForeignKey("dbo.PhoneNumber", t => t.PhoneNumberId)
                .Index(t => t.UserId)
                .Index(t => t.PhoneNumberId);
            
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
            
            CreateTable(
                "dbo.ParentVacancyToChildVacancy",
                c => new
                    {
                        ParentVacancyId = c.Int(nullable: false),
                        ChildVacancyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentVacancyId, t.ChildVacancyId })
                .ForeignKey("dbo.Vacancy", t => t.ParentVacancyId)
                .ForeignKey("dbo.Vacancy", t => t.ChildVacancyId)
                .Index(t => t.ParentVacancyId)
                .Index(t => t.ChildVacancyId);
            
            CreateTable(
                "dbo.VacancyToComment",
                c => new
                    {
                        VacancyId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacancyId, t.CommentId })
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .Index(t => t.VacancyId)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.VacancyToLevel",
                c => new
                    {
                        VacancyId = c.Int(nullable: false),
                        LevelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacancyId, t.LevelId })
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.Level", t => t.LevelId)
                .Index(t => t.VacancyId)
                .Index(t => t.LevelId);
            
            CreateTable(
                "dbo.VacancyToLocation",
                c => new
                    {
                        VacancyId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacancyId, t.LocationId })
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.City", t => t.LocationId)
                .Index(t => t.VacancyId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.VacancyToSkill",
                c => new
                    {
                        VacancyId = c.Int(nullable: false),
                        SkillId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacancyId, t.SkillId })
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.Skill", t => t.SkillId)
                .Index(t => t.VacancyId)
                .Index(t => t.SkillId);
            
            CreateTable(
                "dbo.VacancyToTag",
                c => new
                    {
                        VacancyId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacancyId, t.TagId })
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.Tag", t => t.TagId)
                .Index(t => t.VacancyId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.CandidateToLanguageSkill",
                c => new
                    {
                        CandidateId = c.Int(nullable: false),
                        LanguageSkillId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CandidateId, t.LanguageSkillId })
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .ForeignKey("dbo.LanguageSkill", t => t.LanguageSkillId)
                .Index(t => t.CandidateId)
                .Index(t => t.LanguageSkillId);
            
            CreateTable(
                "dbo.CandidateToPhoneNumber",
                c => new
                    {
                        CandidateId = c.Int(nullable: false),
                        PhoneNumberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CandidateId, t.PhoneNumberId })
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .ForeignKey("dbo.PhoneNumber", t => t.PhoneNumberId)
                .Index(t => t.CandidateId)
                .Index(t => t.PhoneNumberId);
            
            CreateTable(
                "dbo.CandidateToSkill",
                c => new
                    {
                        CandidateId = c.Int(nullable: false),
                        SkillId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CandidateId, t.SkillId })
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .ForeignKey("dbo.Skill", t => t.SkillId)
                .Index(t => t.CandidateId)
                .Index(t => t.SkillId);
            
            CreateTable(
                "dbo.CandidateToTag",
                c => new
                    {
                        CandidateId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CandidateId, t.TagId })
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .ForeignKey("dbo.Tag", t => t.TagId)
                .Index(t => t.CandidateId)
                .Index(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Note", "UserId", "dbo.User");
            DropForeignKey("dbo.VacancyStageInfo", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.CandidateToTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.CandidateToTag", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.CandidateSource", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.CandidateSocial", "SocialNetworkId", "dbo.SocialNetwork");
            DropForeignKey("dbo.CandidateSocial", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.CandidateToSkill", "SkillId", "dbo.Skill");
            DropForeignKey("dbo.CandidateToSkill", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.RelocationPlace", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.Candidate", "Photo_Id", "dbo.Photo");
            DropForeignKey("dbo.CandidateToPhoneNumber", "PhoneNumberId", "dbo.PhoneNumber");
            DropForeignKey("dbo.CandidateToPhoneNumber", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Candidate", "LocationId", "dbo.City");
            DropForeignKey("dbo.Candidate", "LevelId", "dbo.Level");
            DropForeignKey("dbo.CandidateToLanguageSkill", "LanguageSkillId", "dbo.LanguageSkill");
            DropForeignKey("dbo.CandidateToLanguageSkill", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Candidate", "IndustryId", "dbo.Industry");
            DropForeignKey("dbo.File", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.Event", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Event", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyToTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.VacancyToTag", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "ResponsibleId", "dbo.User");
            DropForeignKey("dbo.VacancyToSkill", "SkillId", "dbo.Skill");
            DropForeignKey("dbo.VacancyToSkill", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "ParentVacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyToLocation", "LocationId", "dbo.City");
            DropForeignKey("dbo.VacancyToLocation", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyToLevel", "LevelId", "dbo.Level");
            DropForeignKey("dbo.VacancyToLevel", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "LanguageSkill_Id", "dbo.LanguageSkill");
            DropForeignKey("dbo.LanguageSkill", "LanguageId", "dbo.Language");
            DropForeignKey("dbo.Vacancy", "IndustryId", "dbo.Industry");
            DropForeignKey("dbo.File", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.Department", "DepartmentGroupId", "dbo.DepartmentGroup");
            DropForeignKey("dbo.Vacancy", "CurrencyId", "dbo.Currency");
            DropForeignKey("dbo.VacancyToComment", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.VacancyToComment", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.ParentVacancyToChildVacancy", "ChildVacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.ParentVacancyToChildVacancy", "ParentVacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyStageInfo", "VacancyStage_Id", "dbo.VacancyStage");
            DropForeignKey("dbo.VacancyStage", "StageId", "dbo.Stage");
            DropForeignKey("dbo.VacancyStageInfo", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyStageInfo", "Comment_Id", "dbo.Comment");
            DropForeignKey("dbo.Event", "ResponsibleId", "dbo.User");
            DropForeignKey("dbo.User", "RoleId", "dbo.Role");
            DropForeignKey("dbo.PermissionRole", "Role_Id", "dbo.Role");
            DropForeignKey("dbo.PermissionRole", "Permission_Id", "dbo.Permission");
            DropForeignKey("dbo.User", "Photo_Id", "dbo.Photo");
            DropForeignKey("dbo.UserToPhoneNumber", "PhoneNumberId", "dbo.PhoneNumber");
            DropForeignKey("dbo.UserToPhoneNumber", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "LocationId", "dbo.City");
            DropForeignKey("dbo.RelocationPlace", "CountryId", "dbo.Country");
            DropForeignKey("dbo.RelocationPlaceCity", "City_Id", "dbo.City");
            DropForeignKey("dbo.RelocationPlaceCity", "RelocationPlace_Id", "dbo.RelocationPlace");
            DropForeignKey("dbo.City", "CountryId", "dbo.Country");
            DropForeignKey("dbo.Event", "EventTypeId", "dbo.EventType");
            DropForeignKey("dbo.Candidate", "CurrencyId", "dbo.Currency");
            DropForeignKey("dbo.CandidateToComment", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.CandidateToComment", "CandidateId", "dbo.Candidate");
            DropIndex("dbo.CandidateToTag", new[] { "TagId" });
            DropIndex("dbo.CandidateToTag", new[] { "CandidateId" });
            DropIndex("dbo.CandidateToSkill", new[] { "SkillId" });
            DropIndex("dbo.CandidateToSkill", new[] { "CandidateId" });
            DropIndex("dbo.CandidateToPhoneNumber", new[] { "PhoneNumberId" });
            DropIndex("dbo.CandidateToPhoneNumber", new[] { "CandidateId" });
            DropIndex("dbo.CandidateToLanguageSkill", new[] { "LanguageSkillId" });
            DropIndex("dbo.CandidateToLanguageSkill", new[] { "CandidateId" });
            DropIndex("dbo.VacancyToTag", new[] { "TagId" });
            DropIndex("dbo.VacancyToTag", new[] { "VacancyId" });
            DropIndex("dbo.VacancyToSkill", new[] { "SkillId" });
            DropIndex("dbo.VacancyToSkill", new[] { "VacancyId" });
            DropIndex("dbo.VacancyToLocation", new[] { "LocationId" });
            DropIndex("dbo.VacancyToLocation", new[] { "VacancyId" });
            DropIndex("dbo.VacancyToLevel", new[] { "LevelId" });
            DropIndex("dbo.VacancyToLevel", new[] { "VacancyId" });
            DropIndex("dbo.VacancyToComment", new[] { "CommentId" });
            DropIndex("dbo.VacancyToComment", new[] { "VacancyId" });
            DropIndex("dbo.ParentVacancyToChildVacancy", new[] { "ChildVacancyId" });
            DropIndex("dbo.ParentVacancyToChildVacancy", new[] { "ParentVacancyId" });
            DropIndex("dbo.PermissionRole", new[] { "Role_Id" });
            DropIndex("dbo.PermissionRole", new[] { "Permission_Id" });
            DropIndex("dbo.UserToPhoneNumber", new[] { "PhoneNumberId" });
            DropIndex("dbo.UserToPhoneNumber", new[] { "UserId" });
            DropIndex("dbo.RelocationPlaceCity", new[] { "City_Id" });
            DropIndex("dbo.RelocationPlaceCity", new[] { "RelocationPlace_Id" });
            DropIndex("dbo.CandidateToComment", new[] { "CommentId" });
            DropIndex("dbo.CandidateToComment", new[] { "CandidateId" });
            DropIndex("dbo.Note", new[] { "UserId" });
            DropIndex("dbo.CandidateSource", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateSocial", new[] { "CandidateId" });
            DropIndex("dbo.CandidateSocial", new[] { "SocialNetworkId" });
            DropIndex("dbo.LanguageSkill", new[] { "LanguageId" });
            DropIndex("dbo.File", new[] { "Candidate_Id" });
            DropIndex("dbo.File", new[] { "Vacancy_Id" });
            DropIndex("dbo.Department", new[] { "DepartmentGroupId" });
            DropIndex("dbo.VacancyStage", new[] { "StageId" });
            DropIndex("dbo.VacancyStageInfo", new[] { "VacancyStage_Id" });
            DropIndex("dbo.VacancyStageInfo", new[] { "Comment_Id" });
            DropIndex("dbo.VacancyStageInfo", new[] { "VacancyId" });
            DropIndex("dbo.VacancyStageInfo", new[] { "CandidateId" });
            DropIndex("dbo.Vacancy", new[] { "LanguageSkill_Id" });
            DropIndex("dbo.Vacancy", new[] { "CurrencyId" });
            DropIndex("dbo.Vacancy", new[] { "ResponsibleId" });
            DropIndex("dbo.Vacancy", new[] { "DepartmentId" });
            DropIndex("dbo.Vacancy", new[] { "IndustryId" });
            DropIndex("dbo.Vacancy", new[] { "ParentVacancyId" });
            DropIndex("dbo.RelocationPlace", new[] { "Candidate_Id" });
            DropIndex("dbo.RelocationPlace", new[] { "CountryId" });
            DropIndex("dbo.City", new[] { "CountryId" });
            DropIndex("dbo.User", new[] { "Photo_Id" });
            DropIndex("dbo.User", new[] { "LocationId" });
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.Event", new[] { "CandidateId" });
            DropIndex("dbo.Event", new[] { "VacancyId" });
            DropIndex("dbo.Event", new[] { "EventTypeId" });
            DropIndex("dbo.Event", new[] { "ResponsibleId" });
            DropIndex("dbo.Candidate", new[] { "Photo_Id" });
            DropIndex("dbo.Candidate", new[] { "IndustryId" });
            DropIndex("dbo.Candidate", new[] { "LevelId" });
            DropIndex("dbo.Candidate", new[] { "LocationId" });
            DropIndex("dbo.Candidate", new[] { "CurrencyId" });
            DropTable("dbo.CandidateToTag");
            DropTable("dbo.CandidateToSkill");
            DropTable("dbo.CandidateToPhoneNumber");
            DropTable("dbo.CandidateToLanguageSkill");
            DropTable("dbo.VacancyToTag");
            DropTable("dbo.VacancyToSkill");
            DropTable("dbo.VacancyToLocation");
            DropTable("dbo.VacancyToLevel");
            DropTable("dbo.VacancyToComment");
            DropTable("dbo.ParentVacancyToChildVacancy");
            DropTable("dbo.PermissionRole");
            DropTable("dbo.UserToPhoneNumber");
            DropTable("dbo.RelocationPlaceCity");
            DropTable("dbo.CandidateToComment");
            DropTable("dbo.Note");
            DropTable("dbo.CandidateSource");
            DropTable("dbo.SocialNetwork");
            DropTable("dbo.CandidateSocial");
            DropTable("dbo.Tag");
            DropTable("dbo.Skill");
            DropTable("dbo.Level");
            DropTable("dbo.Language");
            DropTable("dbo.LanguageSkill");
            DropTable("dbo.Industry");
            DropTable("dbo.File");
            DropTable("dbo.DepartmentGroup");
            DropTable("dbo.Department");
            DropTable("dbo.Stage");
            DropTable("dbo.VacancyStage");
            DropTable("dbo.VacancyStageInfo");
            DropTable("dbo.Vacancy");
            DropTable("dbo.Permission");
            DropTable("dbo.Role");
            DropTable("dbo.Photo");
            DropTable("dbo.PhoneNumber");
            DropTable("dbo.RelocationPlace");
            DropTable("dbo.Country");
            DropTable("dbo.City");
            DropTable("dbo.User");
            DropTable("dbo.EventType");
            DropTable("dbo.Event");
            DropTable("dbo.Currency");
            DropTable("dbo.Comment");
            DropTable("dbo.Candidate");
        }
    }
}
