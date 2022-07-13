using AutoMapper;
using TripleSix.Core.Types;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý Mapper.
    /// </summary>
    public static class MapperHelper
    {
        public static TDestination MapData<TDestination>(
            this IMapper mapper,
            object source,
            Action<IMappingOperationOptions<object, TDestination>>? opts = null)
            where TDestination : class
        {
            if (source.GetType() == typeof(TDestination))
                return (TDestination)source;

            opts ??= _ => { };
            return mapper.Map(source, opts);
        }

        public static TDestination MapData<TSource, TDestination>(
            this IMapper mapper,
            TSource source,
            Action<IMappingOperationOptions<TSource, TDestination>>? opts = null)
            where TDestination : class
        {
#pragma warning disable CS8603 // Possible null reference return.
            if (typeof(TSource) == typeof(TDestination))
                return source as TDestination;
#pragma warning restore CS8603 // Possible null reference return.

            opts ??= _ => { };
            return mapper.Map(source, opts);
        }

        public static TDestination MapData<TSource, TDestination>(
            this IMapper mapper,
            TSource source,
            TDestination destination,
            Action<IMappingOperationOptions<TSource, TDestination>>? opts = null)
            where TDestination : class
        {
#pragma warning disable CS8603 // Possible null reference return.
            if (typeof(TSource) == typeof(TDestination))
                return source as TDestination;
#pragma warning restore CS8603 // Possible null reference return.

            opts ??= _ => { };
            return mapper.Map(source, destination, opts);
        }

        public static object MapData(
            this IMapper mapper,
            object source,
            Type sourceType,
            Type destinationType,
            Action<IMappingOperationOptions<object, object>>? opts = null)
        {
            if (sourceType == destinationType)
                return source;

            opts ??= _ => { };
            return mapper.Map(source, sourceType, destinationType, opts);
        }

        public static object MapData(
            this IMapper mapper,
            object source,
            object destination,
            Type sourceType,
            Type destinationType,
            Action<IMappingOperationOptions<object, object>>? opts = null)
        {
            if (sourceType == destinationType) return source;

            opts ??= _ => { };
            return mapper.Map(source, destination, sourceType, destinationType, opts);
        }

        public static TDestination MapUpdate<TSource, TDestination>(
            this IMapper mapper,
            TSource source,
            TDestination destination,
            Action<IMappingOperationOptions<TSource, TDestination>>? opts = null)
            where TSource : class, IDataDto
            where TDestination : class
        {
            return mapper.Map(source, destination, context =>
            {
                context.Items["mapPropertyChangedOnly"] = "true";
                opts?.Invoke(context);
            });
        }

        public static object MapUpdate(
            this IMapper mapper,
            object source,
            object destination,
            Type sourceType,
            Type destinationType,
            Action<IMappingOperationOptions<object, object>>? opts = null)
        {
            return mapper.Map(source, destination, sourceType, destinationType, context =>
            {
                context.Items["mapPropertyChangedOnly"] = "true";
                opts?.Invoke(context);
            });
        }
    }
}