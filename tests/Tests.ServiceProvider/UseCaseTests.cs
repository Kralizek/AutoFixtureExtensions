using Microsoft.Extensions.DependencyInjection;

namespace Tests;

[TestFixture]
public class UseCaseTests
{
    [Test]
    public void Fixture_can_create_instance_of_registered_service()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IEcho, EchoService>()
            .BuildServiceProvider();
        
        var fixture = new Fixture();

        fixture.AddServiceProvider(serviceProvider);

        var result = fixture.Create<IEcho>();
        
        Assert.That(result, Is.InstanceOf<EchoService>());
    }

    [Test]
    public void Fixture_uses_AutoMoq_for_non_registered_services()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IEcho, EchoService>()
            .BuildServiceProvider();

        var fixture = new Fixture();

        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });

        fixture.AddServiceProvider(serviceProvider);
        
        var result = fixture.Create<IAnotherService>();
        
        Assert.Multiple(() =>
        {
            Assert.That(() => Mock.Get(result), Throws.Nothing);

            Assert.That(result, Is.InstanceOf<IAnotherService>());
        });
    }

    [Test]
    public void Registered_services_can_be_overridden()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IEcho, EchoService>()
            .BuildServiceProvider();
        
        var fixture = new Fixture();

        fixture.AddServiceProvider(serviceProvider);
        
        fixture.Inject(Mock.Of<IEcho>());

        var result = fixture.Create<IEcho>();
        
        Assert.Multiple(() =>
        {
            Assert.That(() => Mock.Get(result), Throws.Nothing);

            Assert.That(result, Is.InstanceOf<IEcho>());
        });
    }

    [Test]
    public void Fixture_uses_service_provider_if_customization_is_after_AutoMoq()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IEcho, EchoService>()
            .BuildServiceProvider();
        
        var fixture = new Fixture();
        
        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });

        fixture.AddServiceProvider(serviceProvider);

        var result = fixture.Create<IEcho>();
        
        Assert.That(result, Is.InstanceOf<EchoService>());
    }
    
    [Test]
    public void Fixture_uses_service_provider_if_customization_is_before_AutoMoq()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IEcho, EchoService>()
            .BuildServiceProvider();
        
        var fixture = new Fixture();

        fixture.AddServiceProvider(serviceProvider);
        
        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });

        var result = fixture.Create<IEcho>();
        
        Assert.That(result, Is.InstanceOf<EchoService>());
    }

    [Test]
    public void Fixture_respects_singleton_lifestyle_registration()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IEcho, EchoService>()
            .BuildServiceProvider();
        
        var fixture = new Fixture();

        fixture.AddServiceProvider(serviceProvider);
        
        var first = fixture.Create<IEcho>();
        
        var second = fixture.Create<IEcho>();
        
        Assert.That(first, Is.SameAs(second));
    }
    
    [Test]
    public void Fixture_respects_transient_lifestyle_registration()
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<IEcho, EchoService>()
            .BuildServiceProvider();
        
        var fixture = new Fixture();

        fixture.AddServiceProvider(serviceProvider);
        
        var first = fixture.Create<IEcho>();
        
        var second = fixture.Create<IEcho>();
        
        Assert.That(first, Is.Not.SameAs(second));
    }

    private class MyAutoDataAttribute : AutoDataAttribute
    {
        public MyAutoDataAttribute() : base(() => new Fixture()
            .AddServiceProvider(
                new ServiceCollection()
                    .AddTransient<IEcho, EchoService>()
                    .BuildServiceProvider()))
        {
        }
    }

    [Test, MyAutoData]
    public void Transient_services_can_be_frozen([Frozen] IEcho echo, Holder<IEcho> holder)
    {
        Assert.That(echo, Is.SameAs(holder.Dependency));
    }
}