using DAL.DTO;
using DAL.Infrastructure;
using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Extensions
{
    public static class RoleExtension
    {
        /// <summary>
        /// Maps the existing Role DTO to domain Role entity
        /// </summary>
        /// <param name="role">An object to assign data</param>
        /// <param name="source">A DTO to take data from</param>
        /// <param name="uow">A facade of repositories</param>
        public static void Update(this Role role, RoleDTO source, IUnitOfWork uow)
        {
            role.CreatedOn = source.CreatedOn;
            role.State = source.State;
            role.Title = source.Title;
            role.Permissions = MatchPermissions(source.Permissions, uow.PermissionRepo).ToList();
        }

        /// <summary>
        /// The method that matches incoming number to a collection of permissions
        /// </summary>
        /// <param name="value">The number of bit-vector that comes and needs to be converted to list</param>
        /// <param name="repository">A layer to get permissions from database</param>
        /// <returns>A collection of permissions</returns>
        private static IEnumerable<Permission> MatchPermissions(int value, IPermissionRepository repository)
        {
            AccessRight vector = (AccessRight)value;
            var accessRights = vector
                .ToString()
                .Split(',')
                .Select(x => Enum.Parse(typeof(AccessRight), x))
                .Cast<AccessRight>();

            var result = repository.Get(new List<Expression<Func<Permission, bool>>>()
            {
                perm => accessRights.Contains(perm.AccessRights)
            });
            return result;
        }
    }
}
