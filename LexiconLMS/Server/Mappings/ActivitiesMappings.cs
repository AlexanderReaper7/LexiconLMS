using AutoMapper;
using LexiconLMS.Shared.Dtos.ActivitiesDtos;
using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Server.Mappings
{
	public class ActivitiesMappings : Profile
	{
        public ActivitiesMappings()
        {
            CreateMap<Activity, ActivityDto>()
                .ForMember(
                dest => dest.Type,
                from => from.MapFrom(
                    a => a.Type.Name
                    )
                );
			CreateMap<Activity, AssignmentDto>()
				.ForMember(
				dest => dest.DueDate,
				from => from.MapFrom(
					a => a.EndDate
					)
				);
		}
    }
}
