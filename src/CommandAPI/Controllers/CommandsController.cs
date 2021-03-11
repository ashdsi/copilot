using AutoMapper;
using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;


namespace CommandAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandAPIRepo _repository;
        public CommandsController(ICommandAPIRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{id}", Name="GetCommandbyId")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id); 
            if (commandItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto); 
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel); //one form of mapping
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id); 
            
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            
            _mapper.Map(commandUpdateDto, commandModelFromRepo); //another form of mapping
            
            _repository.UpdateCommand(commandModelFromRepo);     //just kept in case you swap out the repo implementation in future
            _repository.SaveChanges();
            
            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id,JsonPatchDocument<CommandUpdateDto> patchDoc) //specific to PATCH
        {
            //similar to PUT
            var commandModelFromRepo = _repository.GetCommandById(id); 
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            //specific to PATCH
            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo); 
            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }
            
            //similar to PUT
            _mapper.Map(commandToPatch, commandModelFromRepo); 

            _repository.UpdateCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id); 
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            _repository.DeleteCommand(commandModelFromRepo); 
            _repository.SaveChanges();
            return NoContent();
        }

       
    }

}
