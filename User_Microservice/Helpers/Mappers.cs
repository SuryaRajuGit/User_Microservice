using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Microservice.Entity.Dto;
using User_Microservice.Entity.Models;

namespace User_Microservice.Helpers
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<User, UserDTO>().ReverseMap().ForMember(term => term.UserSecret, src => src.MapFrom(src => new UserSecret { Password = src.Password, Id = Guid.NewGuid(),
                UserId = src.Id
            }));

            CreateMap<Card, CardDTO>().ReverseMap();
        }
    }
}
