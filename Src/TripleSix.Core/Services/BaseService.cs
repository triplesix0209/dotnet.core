using AutoMapper;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Events;

namespace TripleSix.Core.Services
{
    public abstract class BaseService
        : IService
    {
        public IConfiguration Configuration { get; set; }

        public IEventPublisher EventPublisher { get; set; }

        public IMapper Mapper { get; set; }
    }
}