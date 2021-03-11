using AutoMapper;
using CommandAPI.Dtos;
using CommandAPI.Models;

namespace CommandAPI.Profiles
{
    public class CommandsProfile : Profile
    {
            public CommandsProfile()
            {
               //Source ➤ Target
               CreateMap<Command, CommandReadDto>();        //GET
               
               CreateMap<CommandCreateDto, Command>();      //POST
               
               CreateMap<CommandUpdateDto, Command>();      //PUT

               CreateMap<Command, CommandUpdateDto>();      //PATCH


        }
        

    }
}
