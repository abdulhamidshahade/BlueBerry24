using AutoMapper;
using BlueBerry24.Services.NotificationAPI.Models;
using BlueBerry24.Services.NotificationAPI.Models.DTOs;

namespace BlueBerry24.Services.NotificationAPI.Halpers.AutoMapper
{
    public class NotificationAutoMapping : Profile
    {
        public NotificationAutoMapping()
        {
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<Notification, CreateNotificationDto>().ReverseMap();
        }
    }
}
