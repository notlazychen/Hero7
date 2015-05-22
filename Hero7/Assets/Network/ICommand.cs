
using System;
using System.IO;
using ProtoBuf;
using RoyNet.GameServer.Entity;

public interface ICommand
{
    int Name { get; }

    void Execute(object package);

    object DeserializePackage(MemoryStream ms);
}


public abstract class Command<T> : ICommand
    where T : class,  global::ProtoBuf.IExtensible
{
    public abstract int Name { get; }

    public void Execute(object package)
    {
        _onExecute(package as T);
    }

    private readonly Action<T> _onExecute;

    public object DeserializePackage(MemoryStream ms)
    {
        return Serializer.Deserialize<T>(ms);
    }

    public Command(Action<T> onExecute)
    {
        _onExecute = onExecute;
    }
}
