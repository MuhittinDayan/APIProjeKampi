using APIProjeKampi_WepApi.Dtos.AboutDtos;
using APIProjeKampi_WepApi.Dtos.CategoryDtos;
using APIProjeKampi_WepApi.Dtos.FeatureDtos;
using APIProjeKampi_WepApi.Dtos.MessagedDtos;
using APIProjeKampi_WepApi.Dtos.NotificationDtos;
using APIProjeKampi_WepApi.Dtos.ProductDtos;
using APIProjeKampi_WepApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APIProjeKampi_WepApi.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Feature,ResultFeatureDto>().ReverseMap();
            CreateMap<Feature,CreateFeatureDto>().ReverseMap();
            CreateMap<Feature,UpdateFeatureDto>().ReverseMap();
            CreateMap<Feature,GetByIdFeatureDto>().ReverseMap();

            CreateMap<Message, ResultFeatureDto>().ReverseMap();
            CreateMap<Message,CreateMessageDto>().ReverseMap();
            CreateMap<Message,UpdateMessageDto>().ReverseMap();
            CreateMap<Message,GetByIdMessageDto>().ReverseMap();

            CreateMap<Product , CreateProductDto>().ReverseMap();
            CreateMap<Product, ResultProductWithCategoryDto>().ForMember(x => x.CategoryName,
                y => y.MapFrom(z => z.Category.CategoryName)).ReverseMap();

            CreateMap<Notification,ResultNotificationDto>().ReverseMap();
            CreateMap<Notification,CreateNotificationDto>().ReverseMap();
            CreateMap<Notification,UpdateNotificationDto>().ReverseMap();
            CreateMap<Notification,GetNotificationByIdDto>().ReverseMap();

            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category , UpdateCategoryDto>().ReverseMap();

            CreateMap<About , CreateAboutDto>().ReverseMap();
            CreateMap<About , ResultAboutDto>().ReverseMap();
            CreateMap<About , GetAboutByIdDto>().ReverseMap();
            CreateMap<About , UpdateAboutDto>().ReverseMap();
            

        }
    }
}
