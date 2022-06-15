namespace Generator.Tests.Extensions
{
    public static class SourceBuildingExtensions
    {
        private const string SingletonServicePlaceholder = "ISingleton";
        private const string SingletonImplementationPlaceholder = "Singleton";

        private const string ScopedServicePlaceholder = "IScoped";
        private const string ScopedImplementationPlaceholder = "Scoped";

        private const string TransientServicePlaceholder = "ITransient";
        private const string TransientImplementationPlaceholder = "Transient";

        public static string UseSingleton<TService, TImplementation>(this string code)
            where TImplementation : TService
        {
            return code?.UseService<TService, TImplementation>(SingletonServicePlaceholder, SingletonImplementationPlaceholder);
        }

        public static string UseScoped<TService, TImplementation>(this string code)
            where TImplementation : TService
        {
            return code?.UseService<TService, TImplementation>(ScopedServicePlaceholder, ScopedImplementationPlaceholder);
        }

        public static string UseTransient<TService, TImplementation>(this string code)
            where TImplementation : TService
        {
            return code?.UseService<TService, TImplementation>(TransientServicePlaceholder, TransientImplementationPlaceholder);
        }

        public static string UseService<TService, TImplementation>(this string code, string servicePlaceholder, string implementationPlaceholder)
            where TImplementation : TService
        {
            return code?.UseService(servicePlaceholder, implementationPlaceholder, typeof(TService).Name, typeof(TImplementation).Name);
        }

        public static string UseService(this string code, string servicePlaceholder, string implementationPlaceholder, string serviceImplementation, string implementationImplementation)
        {
            if (string.IsNullOrEmpty(code))
            {
                return code;
            }

            string placeholder = $"<{servicePlaceholder}, {implementationPlaceholder}>";
            string implementation = $"<{serviceImplementation}, {implementationImplementation}>";

            if (placeholder == implementation)
            {
                return code;
            }

            return code.Replace(placeholder, implementation);
        }
    }
}
