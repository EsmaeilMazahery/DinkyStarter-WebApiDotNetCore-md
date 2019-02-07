using Dinky.DataLayer.Context;
using Dinky.Services.service;
using StructureMap;
using StructureMap.Pipeline;

public class DefaultRegistry : Registry
{
    public DefaultRegistry(string connectionString)
    {
        For<IUserRepository>().LifecycleIs(Lifecycles.Container).Use<UserRepository>(context => new UserRepository(connectionString));
    }
}