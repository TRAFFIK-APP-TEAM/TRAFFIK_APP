namespace TRAFFIK_APP.Helpers
{
	public static class ServiceHelper
	{
		private static IServiceProvider? _services;

		public static void Initialize(IServiceProvider services)
		{
			_services = services;
		}

		public static T? GetService<T>() where T : class
		{
			return _services?.GetService(typeof(T)) as T;
		}

		public static IServiceProvider? Services => _services;
	}
}


