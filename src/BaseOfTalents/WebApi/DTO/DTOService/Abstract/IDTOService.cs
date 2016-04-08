using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DTO.DTOService.Abstract
{
    public interface IDTOService<TEntity, YEntity>
        where TEntity : BaseEntity, new ()
        where YEntity : new ()
    {
         YEntity ToDTO(TEntity entity);
         TEntity ToEntity(YEntity dto);
    }
}