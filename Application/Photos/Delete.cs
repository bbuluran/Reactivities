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
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _context = context;
                this._photoAccessor = photoAccessor;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(a => a.Photos)
                                    .FirstOrDefaultAsync(a => a.UserName == _userAccessor.GetUsername());
                if (user == null) return null;

                var photo = user.Photos.FirstOrDefault(a => a.Id == request.Id);
                if (photo == null) return null;

                if (photo.IsMain)
                {
                    // check if there are other images, select one to be main
                    var photoTobeMain = user.Photos.FirstOrDefault(a => a.Id != request.Id);
                    if (photoTobeMain != null)
                    {
                        // select this a the new main
                        photoTobeMain.IsMain = true;
                    }
                }

                // remove in Cloudinary
                var resultCloudinary = _photoAccessor.DeletePhoto(request.Id);
                if (resultCloudinary == null) return Result<Unit>.Failure("Problem removing photo in Cloudinary");
                // remove in db
                 _context.Photos.Remove(photo);
                 var resultDb = await _context.SaveChangesAsync() > 0;
                if (!resultDb) return Result<Unit>.Failure("Problem removing photo in db");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}