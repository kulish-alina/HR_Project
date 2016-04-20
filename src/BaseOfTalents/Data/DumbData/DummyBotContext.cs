using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using System.Linq.Expressions;

namespace Data.DumbData
{
    public class DummyBotContext
    {
        List<Candidate> _candidates = new List<Candidate>();
        List<Vacancy> _vacancies = new List<Vacancy>();
        List<SocialNetwork> _socialNetworks = new List<SocialNetwork>();
        List<Language> _languages = new List<Language>();
        List<Skill> _skills = new List<Skill>();
        List<Location> _cities = new List<Location>();
        List<Department> _teams = new List<Department>();


        public DummyBotContext()
        {

            #region Candidate
            Comment candidateComment = new Comment()
            {
                CommentType = CommentType.Candidate,
                Message = "msg",
                RelativeId = 0,
            };

            File candidateFile = new File()
            {
                Description = "description",
                FilePath = "path",
            };

            CandidateSource candidateSource = new CandidateSource()
            {
                Path = "Path",
                Source = Source.HeadHunter,
            };

            Language language = new Language()
            {
                Id = 1,
                Title = "language"
            };
            _languages.Add(language);

            LanguageSkill languageSkill = new LanguageSkill()
            {
                Language = language,
                LanguageLevel = LanguageLevel.Fluent,
            };

            Country country = new Country()
            {
                Title = "name"
            };

            Location city = new Location()
            {
                Id = 1,
                Country = country,
                Title = "dnepr"
            };
            _cities.Add(city);

            Photo photo = new Photo()
            {
                Description = "descr",
                ImagePath = "path"
            };

            Skill skill = new Skill()
            {
                Id = 1,
                Title = "C#"
            };
            _skills.Add(skill);

            SocialNetwork socialNetwork = new SocialNetwork()
            {
                Id = 1,
                ImagePath = "path",
                Title = "Path"
            };
            _socialNetworks.Add(socialNetwork);
            CandidateSocial candidateSocial = new CandidateSocial()
            {
                Path = "path",
                SocialNetwork = socialNetwork,
            };

            Candidate candidate = new Candidate()
            {
                Id = 1,
                Skype = "skype",
                BirthDate = DateTime.Now,
                Comments = new List<Comment>() { candidateComment },
                Description = "descrpition",
                Education = "High",
                Email = "email",
               
                Files = new List<File>() { candidateFile },
                Sources = new List<CandidateSource>() { candidateSource },
                FirstName = "TESTNAME",
                IsMale = true,
                LanguageSkills = new List<LanguageSkill>() { languageSkill },
                LastName = "lname",
                Location = city,
                MiddleName = "mname",
                PhoneNumbers = new List<PhoneNumber>() { new PhoneNumber() { Id = 1 ,Number = "+380978762352" } },
                Photo = photo,
                PositionDesired = "architecht",
                Practice = "best",
                RelocationAgreement = true,
                SalaryDesired = 10500,
                Skills = new List<Skill>() { skill },
                SocialNetworks = new List<CandidateSocial>() { candidateSocial },
                TypeOfEmployment = TypeOfEmployment.FullTime,
                VacanciesProgress = new List<VacancyStageInfo>() { }
            };
            #endregion

            Comment vacancyComment = new Comment()
            {
                CommentType = CommentType.Vacancy,
                Message = "msg",
                RelativeId = 0,
            };

            File vacancyFile = new File()
            {
                Description = "file",
                FilePath = "path",
            };

            Permission permission = new Permission()
            {
                AccessRights = AccessRights.AddCandidateToVacancy,
                Description = "Permis",
            };

            Role role = new Role()
            {
                Title = "adm",
                Permissions = new List<Permission>() { permission },
            };

            User user = new User()
            {
                BirthDate = DateTime.Now,
                Email = "mail",
                FirstName = "fname",
                isMale = true,
                LastName = "lastname",
                Location = city,
                Login = "login",
                Password = "pass",
                MiddleName = "mname",
                PhoneNumbers = new List<PhoneNumber>() { new PhoneNumber() { Number = "+3565234662" } },
                Photo = photo,
                Role = role,
                Skype = "skype",
            };

            DepartmentGroup departmentGroup = new DepartmentGroup()
            {
                Title = "title"
            };

            Department department = new Department()
            {
                Id = 1,
                DepartmentGroup = departmentGroup,
                Title = "title"
            };
            _teams.Add(department);

            Vacancy vacancy = new Vacancy()
            {
                TypeOfEmployment = TypeOfEmployment.FullTime,
                Title = "Architecht",
                Comments = new List<Comment>() { vacancyComment },
                DeadlineDate = DateTime.Now,
                Description = "descr",
                EndDate = DateTime.Now,
                Files = new List<File>() { vacancyFile },
                LanguageSkill = languageSkill,
                Level = new List<Level>() { Level.Senior },
                Locations = new List<Location>() { city },
                ParentVacancy = null,
                RequiredSkills = new List<Skill>() { skill },
                Responsible = user,
                SalaryMax = 100500,
                SalaryMin = 15,
                StartDate = DateTime.Now,
                Department = department,
                CandidatesProgress = new List<VacancyStageInfo>()

            };

            Comment vsicomment = new Comment()
            {
                CommentType = CommentType.StageInfo,
                Message = "msg",
                RelativeId = 0,
            };

            Stage stage = new Stage()
            {
                Title = "pool"
            };

            VacancyStage vs = new VacancyStage()
            {
                IsCommentRequired = true,
                Order = 1,
                Stage = stage,
                Vacacny = vacancy
            };

            VacancyStageInfo vsi = new VacancyStageInfo()
            {
                Candidate = candidate,
                Comment = vsicomment,
                VacancyStage = vs
            };
            candidate.VacanciesProgress.Add(vsi);
            vacancy.CandidatesProgress.Add(vsi);

            _vacancies.Add(vacancy);
            _candidates.Add(candidate);
        }


        public IList<Candidate> Candidates
        {
            get
            {
                return _candidates;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Location> Cities
        {
            get
            {
                return _cities;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public IList<Department> Teams
        {
            get
            {
                return _teams;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Skill> Skills
        {
            get
            {
                return _skills;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Language> Languages
        {
            get
            {
                return _languages;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<SocialNetwork> SocialNetworks
        {
            get
            {
                return _socialNetworks;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Vacancy> Vacancies
        {
            get
            {
                return _vacancies;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
