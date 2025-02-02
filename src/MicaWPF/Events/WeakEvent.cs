﻿using System.Runtime.CompilerServices;

namespace MicaWPF.Events;
internal sealed class WeakEvent<T> : IWeakEvent<T>
{
    private readonly object _locker = new();
    private readonly List<(Type EventType, Delegate MethodToCall)> _eventRegistrations = new();

    public ISubscription Subscribe(Action<T> action)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        _eventRegistrations.Add((typeof(T), action));

        return new Subscription(() =>
        {
            lock (_locker)
            {
                _ = _eventRegistrations.Remove((typeof(T), action));
            }
        });
    }

    public ConfiguredTaskAwaitable PublishAsync(T data, bool configureAwait = false)
    {
        return Task.Run(() =>
        {
            lock (_locker)
            {
                foreach (var (EventType, MethodToCall) in _eventRegistrations)
                {
                    if (EventType == typeof(T))
                    {
                        ((Action<T>)MethodToCall)(data);
                    }
                }
            }
        }).ConfigureAwait(configureAwait);
    }

    public void Publish(T data)
    {
        _ = Task.Run(() =>
        {
            lock (_locker)
            {
                foreach (var (EventType, MethodToCall) in _eventRegistrations)
                {
                    if (EventType == typeof(T))
                    {
                        ((Action<T>)MethodToCall)(data);
                    }
                }
            }
        });
    }
}
