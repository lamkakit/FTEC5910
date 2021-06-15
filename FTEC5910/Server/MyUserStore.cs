using FTEC5910.Server.Data;
using FTEC5910.Server.Properties;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FTEC5910.Server
{
    //public class MyUserStore : UserStore<MyIdentityUser, IdentityRole, DataContext, string>
    public class MyUserStore : UserStore<MyIdentityUser, IdentityRole<string>, DataContext, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>> 
    {
        public MyUserStore(DataContext context, IdentityErrorDescriber describer = null) : base(context, describer) {
            //Context1 = context;
        }

        public override async Task<IList<string>> GetRolesAsync(MyIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var UserRoles = await Context.UserRoles.ToListAsync();
            var Roles = await Context.Roles.ToListAsync();
            var userId = user.Id;
            var query = from userRole in UserRoles
                        join role in Roles on userRole.RoleId equals role.Id
                        where userRole.UserId.Equals(userId)
                        select role.Name;
            return query.ToList();
        }

        public override async Task<bool> IsInRoleAsync(MyIdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
            }
            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role != null)
            {
                try
                {
                    var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
                    return userRole != null;
                }
                catch (CosmosException ex)
                {
                    var response = JsonDocument.Parse(ex.ResponseBody);
                    var count = response.RootElement.GetProperty("Errors").EnumerateArray().Where(a=>a.GetString().StartsWith("Resource Not Found.")).Count();
                    if (count > 0)
                        return false;
                    else
                        throw new Exception(ex.Message);
                }                
            }
            return false;
        }

    }
}
