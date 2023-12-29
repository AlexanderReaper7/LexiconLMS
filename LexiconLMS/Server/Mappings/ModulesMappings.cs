using AutoMapper;
using LexiconLMS.Shared.Dtos.ModulesDtos;
using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Server.Mappings
{
	public class ModulesMappings : Profile
	{

        public ModulesMappings()
        {
            CreateMap<Module, ModuleDto>();
        }

    }
}
