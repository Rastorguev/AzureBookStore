using System;
using BookStore.AzureSearch;
using BookStore.RedisCache;
using Unity;

namespace BookStore
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        public static IUnityContainer Container => container.Value;

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ISearch, Search>();
            container.RegisterType<ICache, Cache>();
        }
    }
}