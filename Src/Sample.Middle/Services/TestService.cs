using Sample.Common.Dto;
using Sample.Data.Entities;
using Sample.Data.Repositories;
using Sample.Middle.Abstracts;

namespace Sample.Middle.Services
{
    public class TestService : CommonService<TestEntity, TestAdminDto>,
        ITestService
    {
        public TestService(TestRepository repo)
            : base(repo)
        {
        }

        public TestRepository TestRepo { get; set; }
    }
}
