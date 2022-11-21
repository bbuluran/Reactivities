using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {
        
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsHostRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = dbContext;
            this._httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            // check if logged in user exists
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Task.CompletedTask;
            // check if activity, based on url parameter, exists
            var activityId = Guid.Parse(_httpContextAccessor.HttpContext?
                                            .Request.RouteValues.SingleOrDefault(a => a.Key == "id")
                                            .Value?.ToString());
            // check if activity id and user id combination exists in db
            var attendee = _dbContext.ActivityAttendees.AsNoTracking()
                                .SingleOrDefaultAsync(a => a.ActivityId == activityId 
                                                        && a.AppUserId == userId).Result;
            
            if (attendee == null) return Task.CompletedTask;
            // if attendee is the host, flag as success
            if (attendee.IsHost) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}