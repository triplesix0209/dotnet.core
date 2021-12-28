using AutoMapper;
using CpTech.Core.Events;
using Microsoft.Extensions.Configuration;

namespace CpTech.Core.Services
{
    public abstract class BaseService
        : IService
    {
        public IConfiguration Configuration { get; set; }

        public IEventPublisher EventPublisher { get; set; }

        public IMapper Mapper { get; set; }
    }
}