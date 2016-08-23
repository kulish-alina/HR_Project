using DAL.Mapping;
using DAL.Migrations;
using Domain.Entities;
using Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BOTContext : DbContext
    {
        private static readonly Dictionary<Type, EntitySetBase> _mappingCache =
            new Dictionary<Type, EntitySetBase>();

        public BOTContext()
        {
            Database.SetInitializer(new BOTContextInitializer());
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
        }

        public BOTContext(string connectionString) : base(connectionString)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
        }

        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<RelocationPlace> RelocationPlace { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Industry> Industries { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<DepartmentGroup> DepartmentGroups { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<CandidateSocial> CandidateSocials { get; set; }
        public virtual DbSet<LanguageSkill> LanguageSkills { get; set; }
        public virtual DbSet<CandidateSource> CandidateSources { get; set; }
        public virtual DbSet<VacancyStageInfo> VacanciesStageInfos { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<Vacancy> Vacancies { get; set; }
        public virtual DbSet<ExtendedStage> ExtendedStages { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<SocialNetwork> SocialNetworks { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new CandidateConfiguration());
            modelBuilder.Configurations.Add(new VacancyConfiguration());
            modelBuilder.Configurations.Add(new CandidateSocialConfiguration());
            modelBuilder.Configurations.Add(new CandidateSourceConfiguration());

            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new FileConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new SourceConfiguration());

            modelBuilder.Configurations.Add(new VacancyStageInfoConfiguration());

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new EventConfiguration());
            modelBuilder.Configurations.Add(new NoteConfiguration());

            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new DepartmentGroupConfiguration());
            modelBuilder.Configurations.Add(new TagConfiguration());
            modelBuilder.Configurations.Add(new StageConfiguration());
            modelBuilder.Configurations.Add(new ExtendedStageConfiguration());
            modelBuilder.Configurations.Add(new CityConfiguration());
            modelBuilder.Configurations.Add(new LanguageConfiguration());
            modelBuilder.Configurations.Add(new LanguageSkillConfiguration());
            modelBuilder.Configurations.Add(new EventTypeConfiguration());
            modelBuilder.Configurations.Add(new PermissionConfiguration());
            modelBuilder.Configurations.Add(new SkillConfiguration());
            modelBuilder.Configurations.Add(new LevelConfiguration());
            modelBuilder.Configurations.Add(new IndustryConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new SocialNetworkConfiguration());

            modelBuilder.Configurations.Add(new PhoneNumberConfiguration());
            modelBuilder.Configurations.Add(new RelocationPlaceConfiguration());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public bool Delete()
        {
            return Database.Delete();
        }

        public override int SaveChanges()
        {
            var context = ((IObjectContextAdapter)this).ObjectContext;
            var objectStateEntries =
                from e in context.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified)
                where
                    e.IsRelationship == false &&
                    e.Entity != null &&
                    typeof(BaseEntity).IsAssignableFrom(e.Entity.GetType())
                select e;
            foreach (var entry in objectStateEntries)
            {
                var entityBase = entry.Entity as BaseEntity;

                if (entry.State == EntityState.Added)
                {
                    entityBase.CreatedOn = DateTime.Now;
                }
                entityBase.LastModified = DateTime.Now;
            }

            var deletedEntries = from e in context.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted)
                                 where
                                     e.IsRelationship == false &&
                                     e.Entity != null &&
                                     typeof(BaseEntity).IsAssignableFrom(e.Entity.GetType())
                                 select e;

            foreach (var entry in ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Deleted).ToList())
                SoftDelete(entry);

            return base.SaveChanges();
        }

        private void SoftDelete(DbEntityEntry entry)
        {
            var entryEntityType = entry.Entity.GetType();

            var tableName = GetTableName(entryEntityType);
            var primaryKeyName = GetPrimaryKeyName(entryEntityType);

            var deletequery =
                string.Format(
                    "UPDATE {0} SET IsDeleted = 1, State = 1 WHERE {1} = @id",
                    tableName, primaryKeyName);

            Database.ExecuteSqlCommand(
                deletequery,
                new SqlParameter("@id", entry.OriginalValues[primaryKeyName]));

            //Marking it Unchanged prevents the hard delete
            //entry.State = EntityState.Unchanged;
            //So does setting it to Detached:
            //And that is what EF does when it deletes an item
            //http://msdn.microsoft.com/en-us/data/jj592676.aspx
            entry.State = EntityState.Detached;
        }

        private EntitySetBase GetEntitySet(Type type)
        {
            if (!_mappingCache.ContainsKey(type))
            {
                var octx = ((IObjectContextAdapter)this).ObjectContext;

                var typeName = ObjectContext.GetObjectType(type).Name;

                var es = octx.MetadataWorkspace
                    .GetItemCollection(DataSpace.SSpace)
                    .GetItems<EntityContainer>()
                    .SelectMany(c => c.BaseEntitySets
                        .Where(e => e.Name == typeName))
                    .FirstOrDefault();

                if (es == null)
                    throw new ArgumentException("Entity type not found in GetTableName", typeName);

                _mappingCache.Add(type, es);
            }

            return _mappingCache[type];
        }

        private string GetTableName(Type type)
        {
            var es = GetEntitySet(type);

            return string.Format("[{0}].[{1}]",
                es.MetadataProperties["Schema"].Value,
                es.MetadataProperties["Table"].Value);
        }

        private string GetPrimaryKeyName(Type type)
        {
            var es = GetEntitySet(type);

            return es.ElementType.KeyMembers[0].Name;
        }
    }
}