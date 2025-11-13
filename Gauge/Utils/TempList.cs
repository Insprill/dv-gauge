using System;
using System.Collections.Generic;

namespace Gauge.Utils;

public class TempList<T> : IDisposable
{
    static readonly TempList<T> _instance = new();
    public static TempList<T> Get
    {
        get
        {
            if (!_instance.isFree)
            {
                throw new InvalidOperationException(
                    "Cannot get another TempList<" + typeof(T).FullName + "> before the previous one is disposed!"
                );
            }
            _instance.isFree = false;
            return _instance;
        }
    }

    public List<T> List { get; } = [];
    bool isFree = true;

    public void Dispose()
    {
        isFree = true;
        List.Clear();
    }

    public static implicit operator List<T>(TempList<T> tmp) => tmp.List;
}
