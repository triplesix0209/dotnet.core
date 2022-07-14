using System.Reflection;

namespace TripleSix.Core.OpenTelemetry.Shared
{
    internal class PropertyFetcher<T>
    {
        private readonly string _propertyName;
        private PropertyFetch? innerFetcher;

        public PropertyFetcher(string propertyName)
        {
            _propertyName = propertyName;
        }

        public T? Fetch(object? obj)
        {
            if (!TryFetch(obj, out T? value))
                throw new ArgumentException("Supplied object was null or did not match the expected type.", nameof(obj));
            return value;
        }

        public bool TryFetch(object? obj, out T? value)
        {
            if (obj == null)
            {
                value = default;
                return false;
            }

            if (innerFetcher == null)
            {
                var type = obj.GetType().GetTypeInfo();
                var property = type.DeclaredProperties.FirstOrDefault(p => string.Equals(p.Name, _propertyName, StringComparison.InvariantCultureIgnoreCase));
                if (property == null)
                    property = type.GetProperty(_propertyName);

                innerFetcher = PropertyFetch.FetcherForProperty(property);
            }

            return innerFetcher.TryFetch(obj, out value);
        }

        private class PropertyFetch
        {
            public static PropertyFetch FetcherForProperty(PropertyInfo? propertyInfo)
            {
                if (propertyInfo == null || !typeof(T).IsAssignableFrom(propertyInfo.PropertyType))
                    return new PropertyFetch();

                var typedPropertyFetcher = typeof(TypedPropertyFetch<,>);
                var instantiatedTypedPropertyFetcher = typedPropertyFetcher.MakeGenericType(
                    typeof(T), propertyInfo.DeclaringType!, propertyInfo.PropertyType);

                return (PropertyFetch)Activator.CreateInstance(instantiatedTypedPropertyFetcher, propertyInfo) !;
            }

            public virtual bool TryFetch(object obj, out T? value)
            {
                value = default;
                return false;
            }

            private class TypedPropertyFetch<TDeclaredObject, TDeclaredProperty> : PropertyFetch
                where TDeclaredProperty : T
            {
                private readonly Func<TDeclaredObject, TDeclaredProperty> propertyFetch;

                public TypedPropertyFetch(PropertyInfo property)
                {
                    propertyFetch = (Func<TDeclaredObject, TDeclaredProperty>)property.GetMethod!
                        .CreateDelegate(typeof(Func<TDeclaredObject, TDeclaredProperty>));
                }

                public override bool TryFetch(object obj, out T? value)
                {
                    if (obj is TDeclaredObject o)
                    {
                        value = propertyFetch(o);
                        return true;
                    }

                    value = default;
                    return false;
                }
            }
        }
    }
}
