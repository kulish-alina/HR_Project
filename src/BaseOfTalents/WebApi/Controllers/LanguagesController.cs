using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Controllers
{
    public class LanguagesController : BoTController<Language, Language>
    {
        public LanguagesController(ILanguageRepository languagesRepository)
        {
            _repo = languagesRepository;
        }
    }
}