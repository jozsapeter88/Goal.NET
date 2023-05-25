using AutoMapper;
using Backend.Model;

namespace Backend.DTOs.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Player, PlayerDto>();
        CreateMap<PlayerDto, Player>();
        CreateMap<TeamCreateDto, Team>();
    }
}