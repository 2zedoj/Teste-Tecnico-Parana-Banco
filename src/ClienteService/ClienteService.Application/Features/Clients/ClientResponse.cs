using AutoMapper;
using ClienteService.Domain.Abstraction;
using ClienteService.Domain.Entities.Clients;
using ClienteService.Domain.Entities.Clients.InputCommands;
using ClienteService.Domain.Entities.Clients.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Application.Features.Clients
{
    public class ClientResponse : IResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Document { get; set; } = null!;
        public string Email { get; set; } = null!;
        public double Renda { get; set; }
        public int Score { get; set; }
    }

    public class ClientMapper : Profile
    {
        public ClientMapper() 
        { 
            CreateMap<Client, ClientResponse>()
                .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.Document.Value));
            CreateMap<CreatedClientDto, CreatedClientInputCommand>()
                .ForMember(dest => dest.Document, opt => opt.MapFrom(src => new Document(src.Document)));
        }
    }
}
