using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // check if requested activity is present
                var activity = await _context.Activities
                                        .Include(a => a.Attendees)
                                        .ThenInclude(a => a.AppUser)
                                        .SingleOrDefaultAsync(a => a.Id == request.Id);
                if (activity == null) return null;

                // check if user accessing activity is present
                var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == _userAccessor.GetUsername());
                if (user == null) return null;

                // check if user accessing activity is the host
                var hostUsername = activity.Attendees.FirstOrDefault(a => a.IsHost)?.AppUser?.UserName;
                // check if the user accessing the activity is already an attendee
                var attendance = activity.Attendees.FirstOrDefault(a => a.AppUser.UserName == user.UserName);

                // if the user attending the activity is the host -> cancel the activity
                if (attendance != null && hostUsername == user.UserName)
                    activity.IsCancelled = !activity.IsCancelled;
                // if the user attending the activity is an attendee -> remove them from the activity
                if (attendance != null && hostUsername != user.UserName)
                    activity.Attendees.Remove(attendance);

                // if the user is not an attendee of the activity -> add them to the activity
                if (attendance == null) 
                {
                    attendance = new Domain.ActivityAttendee 
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };

                    activity.Attendees.Add(attendance);
                }

                var result = await _context.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendance");
            }
        }
    }
}