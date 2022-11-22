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

namespace Application.Photos
{
    public class SetMain
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            _userAccessor = userAccessor;
                this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(a => a.Photos)
                                    .FirstOrDefaultAsync(a => a.UserName == _userAccessor.GetUsername());
                if (user == null) return null;
                
                var photo = user.Photos.FirstOrDefault(a => a.Id == request.Id);
                if (photo == null) return null;

                var currentMain = user.Photos.FirstOrDefault(a => a.IsMain);
                if (currentMain != null) currentMain.IsMain = false;
                
                photo.IsMain = true;
                var result = await _context.SaveChangesAsync() > 0;
                
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem setting main image");
            }
        }
    }
}