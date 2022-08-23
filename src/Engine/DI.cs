using System.Collections;
using System.Collections.Generic;
using System;

public class DI
{

    private static Dictionary<Type, object> _depedencies;

    public static void Reset()
    {
        if (_depedencies != null) _depedencies.Clear();
    }

    public static T Set<T>(T dep, bool substitute = false)
    {
        if (Exists<T>())
        {
            if (substitute)
            {
                _depedencies.Remove(typeof(T));
            }
            else
            {
                return (T)_depedencies[typeof(T)];
            }
        }

        _depedencies.Add(typeof(T), dep);
        return dep;
    }

    public static T Remove<T>()
    {
        if (Exists<T>())
        {
            T comp = (T)_depedencies[typeof(T)];
            _depedencies.Remove(typeof(T));
            return comp;
        }
        else
        {
            return default(T);
        }
    }

    public static T Get<T>()
    {
        if (Exists<T>())
        {
            return (T)_depedencies[typeof(T)];
        }
        else
        {
            return default(T);
        }
    }

    public static bool Exists<T>()
    {
        if (_depedencies == null) _depedencies = new Dictionary<Type, object>();
        return _depedencies.ContainsKey(typeof(T));
    }

}
