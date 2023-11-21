using AutoMapper;

namespace TripleSix.Core.Mappers
{
    public static class Extension
    {
        /// <summary>
        /// Kiểm tra các thiết lập auto mapper có hợp lệ hay không.
        /// </summary>
        /// <param name="mapperConfiguration"><see cref="MapperConfiguration"/>.</param>
        public static void ValidateConfiguration(this MapperConfiguration mapperConfiguration)
        {
            try
            {
                mapperConfiguration.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
