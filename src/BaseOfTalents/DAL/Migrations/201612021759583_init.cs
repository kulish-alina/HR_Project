namespace DAL.Migrations
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
                        MiddleName = c.String(),
                        CreatorId = c.Int(nullable: false),
                        LastName = c.String(nullable: false),
                        IsMale = c.Boolean(),
                        BirthDate = c.DateTime(),
                        Email = c.String(),
                        Skype = c.String(),
                        PositionDesired = c.String(),
                        TypeOfEmployment = c.Int(),
                        StartExperience = c.DateTime(),
                        Practice = c.String(),
                        Description = c.String(),
                        Recomendator = c.String(),
                        SalaryDesired = c.Int(),
                        CurrencyId = c.Int(),
                        RelocationAgreement = c.Boolean(),
                        MainSourceId = c.Int(),
                        CityId = c.Int(),
                        LevelId = c.Int(),
                        IndustryId = c.Int(nullable: false),
                        Education = c.String(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Photo_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.City", t => t.CityId)
                .ForeignKey("dbo.User", t => t.CreatorId)
                .ForeignKey("dbo.Currency", t => t.CurrencyId)
                .ForeignKey("dbo.Industry", t => t.IndustryId)
                .ForeignKey("dbo.Level", t => t.LevelId)
                .ForeignKey("dbo.Source", t => t.MainSourceId)
                .ForeignKey("dbo.File", t => t.Photo_Id)
                .Index(t => t.CreatorId)
                .Index(t => t.CurrencyId)
                .Index(t => t.MainSourceId)
                .Index(t => t.CityId)
                .Index(t => t.LevelId)
                .Index(t => t.IndustryId)
                .Index(t => t.Photo_Id);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        CountryId = c.Int(nullable: false),
                        HasOffice = c.Boolean(nullable: false),
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
                        CityId = c.Int(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.City", t => t.CityId)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Vacancy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        TypeOfEmployment = c.Int(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        DeadlineDate = c.DateTime(),
                        DeadlineToCalendar = c.Boolean(nullable: false),
                        ClosingCandidateId = c.Int(),
                        ParentVacancyId = c.Int(),
                        IndustryId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        SalaryMin = c.Int(),
                        SalaryMax = c.Int(),
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
                .ForeignKey("dbo.Candidate", t => t.ClosingCandidateId)
                .Index(t => t.ClosingCandidateId)
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
                        StageId = c.Int(nullable: false),
                        StageState = c.Int(nullable: false),
                        VacancyId = c.Int(nullable: false),
                        CandidateId = c.Int(nullable: false),
                        DateOfPass = c.DateTime(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Comment_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .ForeignKey("dbo.Comment", t => t.Comment_Id)
                .ForeignKey("dbo.Stage", t => t.StageId)
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .Index(t => t.StageId)
                .Index(t => t.VacancyId)
                .Index(t => t.CandidateId)
                .Index(t => t.Comment_Id);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        AuthorId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.AuthorId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        isMale = c.Boolean(nullable: false),
                        BirthDate = c.DateTime(),
                        Email = c.String(nullable: false),
                        Skype = c.String(),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CityId = c.Int(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Photo_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.City", t => t.CityId)
                .ForeignKey("dbo.File", t => t.Photo_Id)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.CityId)
                .Index(t => t.Photo_Id);
            
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
                        User_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.UserId)
                .Index(t => t.User_Id);
            
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
                "dbo.Stage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Order = c.Int(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        CommentField = c.Boolean(nullable: false),
                        IsCommentRequired = c.Boolean(nullable: false),
                        StageType = c.Int(nullable: false),
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
                "dbo.LogUnit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Field = c.String(),
                        FieldType = c.Int(nullable: false),
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
                "dbo.LogValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        LanguageLevel = c.Int(),
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
                        Title = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
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
                "dbo.ExtendedStage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StageId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Vacancy_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stage", t => t.StageId)
                .ForeignKey("dbo.Vacancy", t => t.Vacancy_Id)
                .Index(t => t.StageId)
                .Index(t => t.Vacancy_Id);
            
            CreateTable(
                "dbo.VacancyState",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VacancyId = c.Int(nullable: false),
                        Passed = c.DateTime(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .Index(t => t.VacancyId);
            
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
                        ImagePath = c.String(),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Source",
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
                "dbo.CandidateSocial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(nullable: false),
                        SocialNetworkId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
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
                        Title = c.String(nullable: false),
                        ImagePath = c.String(),
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
                        Path = c.String(nullable: false),
                        SourceId = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        Candidate_Id = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Source", t => t.SourceId)
                .ForeignKey("dbo.Candidate", t => t.Candidate_Id)
                .Index(t => t.SourceId)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.MailContent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        Invitation = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        Farewell = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        State = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.UserToComment",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CommentId })
                .ForeignKey("dbo.User", t => t.UserId)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .Index(t => t.UserId)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.VacancyToCity",
                c => new
                    {
                        VacancyId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacancyId, t.CityId })
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.City", t => t.CityId)
                .Index(t => t.VacancyId)
                .Index(t => t.CityId);
            
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
                "dbo.FileToVacancy",
                c => new
                    {
                        VacancyId = c.Int(nullable: false),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacancyId, t.FileId })
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.File", t => t.FileId)
                .Index(t => t.VacancyId)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.LogUnitToNewLogValue",
                c => new
                    {
                        NewLogValueId = c.Int(nullable: false),
                        LogUnitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NewLogValueId, t.LogUnitId })
                .ForeignKey("dbo.LogUnit", t => t.NewLogValueId)
                .ForeignKey("dbo.LogValue", t => t.LogUnitId)
                .Index(t => t.NewLogValueId)
                .Index(t => t.LogUnitId);
            
            CreateTable(
                "dbo.LogUnitToPastLogValue",
                c => new
                    {
                        PastLogValueId = c.Int(nullable: false),
                        LogUnitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PastLogValueId, t.LogUnitId })
                .ForeignKey("dbo.LogUnit", t => t.PastLogValueId)
                .ForeignKey("dbo.LogValue", t => t.LogUnitId)
                .Index(t => t.PastLogValueId)
                .Index(t => t.LogUnitId);
            
            CreateTable(
                "dbo.LogUnitToVacancy",
                c => new
                    {
                        VacancyId = c.Int(nullable: false),
                        LogUnitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacancyId, t.LogUnitId })
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.LogUnit", t => t.LogUnitId)
                .Index(t => t.VacancyId)
                .Index(t => t.LogUnitId);
            
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
                "dbo.FileToCandidate",
                c => new
                    {
                        CandidateId = c.Int(nullable: false),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CandidateId, t.FileId })
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .ForeignKey("dbo.File", t => t.FileId)
                .Index(t => t.CandidateId)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.CandidateToLogUnit",
                c => new
                    {
                        CandidateId = c.Int(nullable: false),
                        LogUnitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CandidateId, t.LogUnitId })
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .ForeignKey("dbo.LogUnit", t => t.LogUnitId)
                .Index(t => t.CandidateId)
                .Index(t => t.LogUnitId);
            
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
                "dbo.CandidateToRelocationPlace",
                c => new
                    {
                        CandidateId = c.Int(nullable: false),
                        RelocationPlaceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CandidateId, t.RelocationPlaceId })
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .ForeignKey("dbo.RelocationPlace", t => t.RelocationPlaceId)
                .Index(t => t.CandidateId)
                .Index(t => t.RelocationPlaceId);
            
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
            DropForeignKey("dbo.CandidateToTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.CandidateToTag", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.CandidateSource", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.CandidateSource", "SourceId", "dbo.Source");
            DropForeignKey("dbo.CandidateSocial", "Candidate_Id", "dbo.Candidate");
            DropForeignKey("dbo.CandidateSocial", "SocialNetworkId", "dbo.SocialNetwork");
            DropForeignKey("dbo.CandidateToSkill", "SkillId", "dbo.Skill");
            DropForeignKey("dbo.CandidateToSkill", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.CandidateToRelocationPlace", "RelocationPlaceId", "dbo.RelocationPlace");
            DropForeignKey("dbo.CandidateToRelocationPlace", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Candidate", "Photo_Id", "dbo.File");
            DropForeignKey("dbo.CandidateToPhoneNumber", "PhoneNumberId", "dbo.PhoneNumber");
            DropForeignKey("dbo.CandidateToPhoneNumber", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Candidate", "MainSourceId", "dbo.Source");
            DropForeignKey("dbo.Candidate", "LevelId", "dbo.Level");
            DropForeignKey("dbo.CandidateToLanguageSkill", "LanguageSkillId", "dbo.LanguageSkill");
            DropForeignKey("dbo.CandidateToLanguageSkill", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Candidate", "IndustryId", "dbo.Industry");
            DropForeignKey("dbo.CandidateToLogUnit", "LogUnitId", "dbo.LogUnit");
            DropForeignKey("dbo.CandidateToLogUnit", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.FileToCandidate", "FileId", "dbo.File");
            DropForeignKey("dbo.FileToCandidate", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Event", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Event", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.Event", "ResponsibleId", "dbo.User");
            DropForeignKey("dbo.Event", "EventTypeId", "dbo.EventType");
            DropForeignKey("dbo.Candidate", "CurrencyId", "dbo.Currency");
            DropForeignKey("dbo.Candidate", "CreatorId", "dbo.User");
            DropForeignKey("dbo.CandidateToComment", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.CandidateToComment", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Vacancy", "ClosingCandidateId", "dbo.Candidate");
            DropForeignKey("dbo.VacancyToTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.VacancyToTag", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyState", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.ExtendedStage", "Vacancy_Id", "dbo.Vacancy");
            DropForeignKey("dbo.ExtendedStage", "StageId", "dbo.Stage");
            DropForeignKey("dbo.Vacancy", "ResponsibleId", "dbo.User");
            DropForeignKey("dbo.VacancyToSkill", "SkillId", "dbo.Skill");
            DropForeignKey("dbo.VacancyToSkill", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "ParentVacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyToLevel", "LevelId", "dbo.Level");
            DropForeignKey("dbo.VacancyToLevel", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "LanguageSkill_Id", "dbo.LanguageSkill");
            DropForeignKey("dbo.LanguageSkill", "LanguageId", "dbo.Language");
            DropForeignKey("dbo.Vacancy", "IndustryId", "dbo.Industry");
            DropForeignKey("dbo.LogUnitToVacancy", "LogUnitId", "dbo.LogUnit");
            DropForeignKey("dbo.LogUnitToVacancy", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.LogUnit", "UserId", "dbo.User");
            DropForeignKey("dbo.LogUnitToPastLogValue", "LogUnitId", "dbo.LogValue");
            DropForeignKey("dbo.LogUnitToPastLogValue", "PastLogValueId", "dbo.LogUnit");
            DropForeignKey("dbo.LogUnitToNewLogValue", "LogUnitId", "dbo.LogValue");
            DropForeignKey("dbo.LogUnitToNewLogValue", "NewLogValueId", "dbo.LogUnit");
            DropForeignKey("dbo.FileToVacancy", "FileId", "dbo.File");
            DropForeignKey("dbo.FileToVacancy", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.Vacancy", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.Department", "DepartmentGroupId", "dbo.DepartmentGroup");
            DropForeignKey("dbo.Vacancy", "CurrencyId", "dbo.Currency");
            DropForeignKey("dbo.VacancyToComment", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.VacancyToComment", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyToCity", "CityId", "dbo.City");
            DropForeignKey("dbo.VacancyToCity", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyStageInfo", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.VacancyStageInfo", "StageId", "dbo.Stage");
            DropForeignKey("dbo.VacancyStageInfo", "Comment_Id", "dbo.Comment");
            DropForeignKey("dbo.Comment", "AuthorId", "dbo.User");
            DropForeignKey("dbo.UserToComment", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.UserToComment", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "RoleId", "dbo.Role");
            DropForeignKey("dbo.PermissionRole", "Role_Id", "dbo.Role");
            DropForeignKey("dbo.PermissionRole", "Permission_Id", "dbo.Permission");
            DropForeignKey("dbo.User", "Photo_Id", "dbo.File");
            DropForeignKey("dbo.UserToPhoneNumber", "PhoneNumberId", "dbo.PhoneNumber");
            DropForeignKey("dbo.UserToPhoneNumber", "UserId", "dbo.User");
            DropForeignKey("dbo.Note", "User_Id", "dbo.User");
            DropForeignKey("dbo.Note", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "CityId", "dbo.City");
            DropForeignKey("dbo.VacancyStageInfo", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Candidate", "CityId", "dbo.City");
            DropForeignKey("dbo.RelocationPlace", "CountryId", "dbo.Country");
            DropForeignKey("dbo.RelocationPlace", "CityId", "dbo.City");
            DropForeignKey("dbo.City", "CountryId", "dbo.Country");
            DropIndex("dbo.CandidateToTag", new[] { "TagId" });
            DropIndex("dbo.CandidateToTag", new[] { "CandidateId" });
            DropIndex("dbo.CandidateToSkill", new[] { "SkillId" });
            DropIndex("dbo.CandidateToSkill", new[] { "CandidateId" });
            DropIndex("dbo.CandidateToRelocationPlace", new[] { "RelocationPlaceId" });
            DropIndex("dbo.CandidateToRelocationPlace", new[] { "CandidateId" });
            DropIndex("dbo.CandidateToPhoneNumber", new[] { "PhoneNumberId" });
            DropIndex("dbo.CandidateToPhoneNumber", new[] { "CandidateId" });
            DropIndex("dbo.CandidateToLanguageSkill", new[] { "LanguageSkillId" });
            DropIndex("dbo.CandidateToLanguageSkill", new[] { "CandidateId" });
            DropIndex("dbo.CandidateToLogUnit", new[] { "LogUnitId" });
            DropIndex("dbo.CandidateToLogUnit", new[] { "CandidateId" });
            DropIndex("dbo.FileToCandidate", new[] { "FileId" });
            DropIndex("dbo.FileToCandidate", new[] { "CandidateId" });
            DropIndex("dbo.CandidateToComment", new[] { "CommentId" });
            DropIndex("dbo.CandidateToComment", new[] { "CandidateId" });
            DropIndex("dbo.VacancyToTag", new[] { "TagId" });
            DropIndex("dbo.VacancyToTag", new[] { "VacancyId" });
            DropIndex("dbo.VacancyToSkill", new[] { "SkillId" });
            DropIndex("dbo.VacancyToSkill", new[] { "VacancyId" });
            DropIndex("dbo.VacancyToLevel", new[] { "LevelId" });
            DropIndex("dbo.VacancyToLevel", new[] { "VacancyId" });
            DropIndex("dbo.LogUnitToVacancy", new[] { "LogUnitId" });
            DropIndex("dbo.LogUnitToVacancy", new[] { "VacancyId" });
            DropIndex("dbo.LogUnitToPastLogValue", new[] { "LogUnitId" });
            DropIndex("dbo.LogUnitToPastLogValue", new[] { "PastLogValueId" });
            DropIndex("dbo.LogUnitToNewLogValue", new[] { "LogUnitId" });
            DropIndex("dbo.LogUnitToNewLogValue", new[] { "NewLogValueId" });
            DropIndex("dbo.FileToVacancy", new[] { "FileId" });
            DropIndex("dbo.FileToVacancy", new[] { "VacancyId" });
            DropIndex("dbo.VacancyToComment", new[] { "CommentId" });
            DropIndex("dbo.VacancyToComment", new[] { "VacancyId" });
            DropIndex("dbo.VacancyToCity", new[] { "CityId" });
            DropIndex("dbo.VacancyToCity", new[] { "VacancyId" });
            DropIndex("dbo.UserToComment", new[] { "CommentId" });
            DropIndex("dbo.UserToComment", new[] { "UserId" });
            DropIndex("dbo.PermissionRole", new[] { "Role_Id" });
            DropIndex("dbo.PermissionRole", new[] { "Permission_Id" });
            DropIndex("dbo.UserToPhoneNumber", new[] { "PhoneNumberId" });
            DropIndex("dbo.UserToPhoneNumber", new[] { "UserId" });
            DropIndex("dbo.CandidateSource", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateSource", new[] { "SourceId" });
            DropIndex("dbo.CandidateSocial", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateSocial", new[] { "SocialNetworkId" });
            DropIndex("dbo.Event", new[] { "CandidateId" });
            DropIndex("dbo.Event", new[] { "VacancyId" });
            DropIndex("dbo.Event", new[] { "EventTypeId" });
            DropIndex("dbo.Event", new[] { "ResponsibleId" });
            DropIndex("dbo.VacancyState", new[] { "VacancyId" });
            DropIndex("dbo.ExtendedStage", new[] { "Vacancy_Id" });
            DropIndex("dbo.ExtendedStage", new[] { "StageId" });
            DropIndex("dbo.LanguageSkill", new[] { "LanguageId" });
            DropIndex("dbo.LogUnit", new[] { "UserId" });
            DropIndex("dbo.Department", new[] { "DepartmentGroupId" });
            DropIndex("dbo.Note", new[] { "User_Id" });
            DropIndex("dbo.Note", new[] { "UserId" });
            DropIndex("dbo.User", new[] { "Photo_Id" });
            DropIndex("dbo.User", new[] { "CityId" });
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.Comment", new[] { "AuthorId" });
            DropIndex("dbo.VacancyStageInfo", new[] { "Comment_Id" });
            DropIndex("dbo.VacancyStageInfo", new[] { "CandidateId" });
            DropIndex("dbo.VacancyStageInfo", new[] { "VacancyId" });
            DropIndex("dbo.VacancyStageInfo", new[] { "StageId" });
            DropIndex("dbo.Vacancy", new[] { "LanguageSkill_Id" });
            DropIndex("dbo.Vacancy", new[] { "CurrencyId" });
            DropIndex("dbo.Vacancy", new[] { "ResponsibleId" });
            DropIndex("dbo.Vacancy", new[] { "DepartmentId" });
            DropIndex("dbo.Vacancy", new[] { "IndustryId" });
            DropIndex("dbo.Vacancy", new[] { "ParentVacancyId" });
            DropIndex("dbo.Vacancy", new[] { "ClosingCandidateId" });
            DropIndex("dbo.RelocationPlace", new[] { "CityId" });
            DropIndex("dbo.RelocationPlace", new[] { "CountryId" });
            DropIndex("dbo.City", new[] { "CountryId" });
            DropIndex("dbo.Candidate", new[] { "Photo_Id" });
            DropIndex("dbo.Candidate", new[] { "IndustryId" });
            DropIndex("dbo.Candidate", new[] { "LevelId" });
            DropIndex("dbo.Candidate", new[] { "CityId" });
            DropIndex("dbo.Candidate", new[] { "MainSourceId" });
            DropIndex("dbo.Candidate", new[] { "CurrencyId" });
            DropIndex("dbo.Candidate", new[] { "CreatorId" });
            DropTable("dbo.CandidateToTag");
            DropTable("dbo.CandidateToSkill");
            DropTable("dbo.CandidateToRelocationPlace");
            DropTable("dbo.CandidateToPhoneNumber");
            DropTable("dbo.CandidateToLanguageSkill");
            DropTable("dbo.CandidateToLogUnit");
            DropTable("dbo.FileToCandidate");
            DropTable("dbo.CandidateToComment");
            DropTable("dbo.VacancyToTag");
            DropTable("dbo.VacancyToSkill");
            DropTable("dbo.VacancyToLevel");
            DropTable("dbo.LogUnitToVacancy");
            DropTable("dbo.LogUnitToPastLogValue");
            DropTable("dbo.LogUnitToNewLogValue");
            DropTable("dbo.FileToVacancy");
            DropTable("dbo.VacancyToComment");
            DropTable("dbo.VacancyToCity");
            DropTable("dbo.UserToComment");
            DropTable("dbo.PermissionRole");
            DropTable("dbo.UserToPhoneNumber");
            DropTable("dbo.MailContent");
            DropTable("dbo.CandidateSource");
            DropTable("dbo.SocialNetwork");
            DropTable("dbo.CandidateSocial");
            DropTable("dbo.Source");
            DropTable("dbo.EventType");
            DropTable("dbo.Event");
            DropTable("dbo.Tag");
            DropTable("dbo.VacancyState");
            DropTable("dbo.ExtendedStage");
            DropTable("dbo.Skill");
            DropTable("dbo.Level");
            DropTable("dbo.Language");
            DropTable("dbo.LanguageSkill");
            DropTable("dbo.Industry");
            DropTable("dbo.LogValue");
            DropTable("dbo.LogUnit");
            DropTable("dbo.DepartmentGroup");
            DropTable("dbo.Department");
            DropTable("dbo.Currency");
            DropTable("dbo.Stage");
            DropTable("dbo.Permission");
            DropTable("dbo.Role");
            DropTable("dbo.File");
            DropTable("dbo.PhoneNumber");
            DropTable("dbo.Note");
            DropTable("dbo.User");
            DropTable("dbo.Comment");
            DropTable("dbo.VacancyStageInfo");
            DropTable("dbo.Vacancy");
            DropTable("dbo.RelocationPlace");
            DropTable("dbo.Country");
            DropTable("dbo.City");
            DropTable("dbo.Candidate");
        }
    }
}
