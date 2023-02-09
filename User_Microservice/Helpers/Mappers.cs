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
            CreateMap<Phone, PhoneDTO>().ReverseMap().ForMember(sel => sel.IsActive, act => act.MapFrom(sel => true));
            CreateMap<Address, AddressDTO>().ReverseMap().ForMember(sel => sel.IsActive, act => act.MapFrom(sel => true));
            CreateMap<UserSecret, UserSecretDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ForMember(src => src.Password, term => term.MapFrom(ter =>
                new UserSecret() { Id = Guid.NewGuid(), UserId = ter.Id, Password = ter.UserSecret.Password }
             ));

            CreateMap< PhoneDTO,Phone>();
                 CreateMap<AddressDTO, Address>();
                CreateMap<UserSecretDTO, UserSecret>();
            CreateMap<UserDetailsResponse, User>();
            CreateMap<UserDTO, User>();

            CreateMap<Payment, CardDTO>().ReverseMap();
        }
    }
}
