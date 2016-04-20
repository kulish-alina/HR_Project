using Data.EFData.Design;
using Domain.Entities;
using Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.DTO.DTOService;

namespace WebApi.Controllers
{
    public abstract class BoTController<DomainEntity, ViewModel> : ApiController
        where DomainEntity : BaseEntity, new()
        where ViewModel : new()
    {
        protected IRepositoryFacade _repoFacade;
        protected IRepository<DomainEntity> _currentRepo;

        protected static int ENTITIES_PER_PAGE = 30;
        protected static JsonSerializerSettings SERIALIZER_SETTINGS = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd"
        };
        
        public BoTController(IRepositoryFacade facade)
        {
            _repoFacade = facade;
        }

        public BoTController()
        {

        }

        [HttpGet]
        // GET: api/Entities/
        [Route("api/{controller}{pageNumber:int}")]
        public virtual IHttpActionResult All(int? pageNumber = 1)
        {
            var entitiesQuery = _currentRepo.GetAll().OrderBy(x => x.Id);
            if (pageNumber.HasValue)
            {
                var totalCount = _currentRepo.GetAll().Count();
                var totalPages = Math.Ceiling((double)totalCount / ENTITIES_PER_PAGE);

                var entities = entitiesQuery
                                    .Skip((pageNumber.Value - 1) * ENTITIES_PER_PAGE)
                                    .Take(ENTITIES_PER_PAGE)
                                    .ToList()
                                    .Select(x => DTOService.ToDTO<DomainEntity, ViewModel>(x));
                return Json(new
                {
                    totalCount = totalCount,
                    totalPages = totalPages,
                    queryResult = entities
                }, SERIALIZER_SETTINGS);
            }
            return Json(entitiesQuery, SERIALIZER_SETTINGS);
        }

        [HttpGet]
        public virtual IHttpActionResult Get(int id)
        {
            var foundedEntity = _currentRepo.Get(id);
            if (foundedEntity != null)
            {
                var foundedEntityDto = DTOService.ToDTO<DomainEntity, ViewModel>(foundedEntity);
                return Json(foundedEntityDto, SERIALIZER_SETTINGS);
            }
            return NotFound();
        }

        [HttpDelete]
        public virtual IHttpActionResult Remove(int id)
        {
             var entityToRemove = _currentRepo.Get(id);
             if (entityToRemove != null)
             {
                _currentRepo.Remove(entityToRemove);
                 return Ok();
             }
             return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public virtual IHttpActionResult Add([FromBody]ViewModel entity)
        {
             if (ModelState.IsValid)
             {
                var newEntity = DTOService.ToEntity<ViewModel, DomainEntity>(entity);
                _currentRepo.Add(newEntity);
                return Json(DTOService.ToDTO<DomainEntity, ViewModel>(_currentRepo.GetAll().Last()));
            }
             return BadRequest();
        }

        [HttpPut]
        public virtual IHttpActionResult Put(int id, [FromBody]ViewModel changedEntity)
        {
            if (ModelState.IsValid)
             {
                 if (changedEntity != null)
                 {
                    var changedDomainEntity = DTOService.ToEntity<ViewModel, DomainEntity>(changedEntity);
                    _currentRepo.Update(changedDomainEntity);
                    return Json(DTOService.ToDTO<DomainEntity, ViewModel>(_currentRepo.Get(changedDomainEntity.Id)), SERIALIZER_SETTINGS);
                }
                return NotFound();
             }
             return BadRequest();
        }

        
    }
}