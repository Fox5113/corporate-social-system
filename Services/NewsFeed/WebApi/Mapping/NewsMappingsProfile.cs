using AutoMapper;
using BusinessLogic.Contracts.News;
using WebApi.Models.News;

namespace WebApi.Mapping
{
    public class NewsMappingsProfile : Profile
    {
        public NewsMappingsProfile()
        {
            CreateMap<NewsDto, NewsModel>();
            CreateMap<CreatingNewsModel, CreatingNewsDto>();
            CreateMap<UpdatingNewsModel, UpdatingNewsDto>();
            CreateMap<NewsFilterModel, NewsFilterDto>();
        }
    }
}
