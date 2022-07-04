namespace Tests;

public interface IEcho
{
    string Echo(string message);
}

public class EchoService : IEcho
{
    public string Echo(string message) => message;
}

public interface IAnotherService { }

public class Holder<T>
{
    public T Dependency { get; }

    public Holder(T dependency)
    {
        Dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
    }
}