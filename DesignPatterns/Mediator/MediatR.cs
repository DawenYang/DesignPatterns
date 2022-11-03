using Autofac;
using MediatR;

namespace DesignPatterns.Mediator;

public class PongResponse
{
    public DateTime Timestamp;

    public PongResponse(DateTime timestamp)
    {
        Timestamp = timestamp;
    }
}

public class PingCommand : IRequest<PongResponse>
{

}

public class PingCommandHandler : IRequestHandler<PingCommand, PongResponse>
{
    public async Task<PongResponse> Handle(PingCommand request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new PongResponse(DateTime.Now))
            .ConfigureAwait(false);
    }
}

public class MediatR
{
    static async Task Demo()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<global::MediatR.Mediator>()
            .As<IMediator>()
            .InstancePerLifetimeScope();

        builder.Register<ServiceFactory>(context =>
        {
            var c = context.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        });

        builder.RegisterAssemblyTypes(typeof(MediatR).Assembly)
            .AsImplementedInterfaces();

        var container = builder.Build();
        var mediator = container.Resolve<IMediator>();
        var response = await mediator.Send(new PingCommand());
        Console.WriteLine($"We got a pong at {response.Timestamp}");
    }
}