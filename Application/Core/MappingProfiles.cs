using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>()
                .ForMember(a => a.HostUsername, 
                            o => o.MapFrom(
                                s => s.Attendees.FirstOrDefault(
                                    x => x.IsHost).AppUser.UserName));
            CreateMap<ActivityAttendee, Profiles.Profile>()
                .ForMember(a => a.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(a => a.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(a => a.Bio, o => o.MapFrom(s => s.AppUser.Bio));
        }
    }
}