using AutoMapper;
using TripleSix.Core.Types;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý Mapper.
    /// </summary>
    public static class MapperHelper
    {
        /// <summary>
        /// Execute a mapping from the source object to a new destination object with supplied mapping options.
        /// </summary>
        /// <typeparam name="TDestination">Destination type to create.</typeparam>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="source">Source object to map from.</param>
        /// <param name="opts">Mapping options.</param>
        /// <returns>Mapped destination object.</returns>
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

        /// <summary>
        /// Execute a mapping from the source object to a new destination object with supplied mapping options.
        /// </summary>
        /// <typeparam name="TSource">Source type to map.</typeparam>
        /// <typeparam name="TDestination">Destination type to create.</typeparam>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="source">Source object to map from.</param>
        /// <param name="opts">Mapping options.</param>
        /// <returns>Mapped destination object.</returns>
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

        /// <summary>
        /// Execute a mapping from the source object to a new destination object with supplied mapping options.
        /// </summary>
        /// <typeparam name="TSource">Source type to map.</typeparam>
        /// <typeparam name="TDestination">Destination type to create.</typeparam>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="source">Source object to map from.</param>
        /// <param name="destination">Destination object to create.</param>
        /// <param name="opts">Mapping options.</param>
        /// <returns>Mapped destination object.</returns>
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

        /// <summary>
        /// Execute a mapping from the source object to a new destination object with supplied mapping options.
        /// </summary>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="source">Source object to map from.</param>
        /// <param name="sourceType">Source type to map from.</param>
        /// <param name="destinationType">Destination type to create.</param>
        /// <param name="opts">Mapping options.</param>
        /// <returns>Mapped destination object.</returns>
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

        /// <summary>
        /// Execute a mapping from the source object to a new destination object with supplied mapping options.
        /// </summary>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="source">Source object to map from.</param>
        /// <param name="destination">Destination object to create.</param>
        /// <param name="sourceType">Source type to map from.</param>
        /// <param name="destinationType">Destination type to create.</param>
        /// <param name="opts">Mapping options.</param>
        /// <returns>Mapped destination object.</returns>
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

        /// <summary>
        /// Execute a mapping from the source object to a new destination object with supplied mapping options, only map property changed.
        /// </summary>
        /// <typeparam name="TSource">Source type to map.</typeparam>
        /// <typeparam name="TDestination">Destination type to update.</typeparam>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="source">Source object to map from.</param>
        /// <param name="destination">Destination object to create.</param>
        /// <param name="opts">Mapping options.</param>
        /// <returns>Mapped destination object.</returns>
        public static TDestination MapUpdate<TSource, TDestination>(
            this IMapper mapper,
            TSource source,
            TDestination destination,
            Action<IMappingOperationOptions<TSource, TDestination>>? opts = null)
            where TSource : class, IDto
            where TDestination : class
        {
            return mapper.Map(source, destination, context =>
            {
                context.Items["mapPropertyChangedOnly"] = "true";
                opts?.Invoke(context);
            });
        }

        /// <summary>
        /// Execute a mapping from the source object to a new destination object with supplied mapping options, only map property changed.
        /// </summary>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="source">Source object to map from.</param>
        /// <param name="destination">Destination object to create.</param>
        /// <param name="sourceType">Source type to map from.</param>
        /// <param name="destinationType">Destination type to create.</param>
        /// <param name="opts">Mapping options.</param>
        /// <returns>Mapped destination object.</returns>
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