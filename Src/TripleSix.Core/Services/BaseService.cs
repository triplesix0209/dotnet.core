﻿using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Services
{
    public abstract class BaseService
        : IService
    {
        public IConfiguration Configuration { get; set; }

        public IMapper Mapper { get; set; }
    }
}