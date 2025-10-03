using System;
using System.Collections.Generic;

public class Locator
{
    private static Dictionary<Type, Dictionary<int, object>> objects = new Dictionary<Type, Dictionary<int, object>>();

    public static void Register<T>(T t, int idx = 0)
    {
        Type type = typeof(T);
        if(!objects.ContainsKey(type))
        {
            objects.Add(type, new Dictionary<int, object>());
        }
        if(!objects[type].ContainsKey(idx))
        {
            objects[type].Add(idx, t);
        }
        else
        {
            objects[type][idx] = t;
        }
    }

    public static T Resolve<T>(int idx = 0)
    {
        return (T)objects[typeof(T)][idx];
    }

    public static void Initalize()
    {
        foreach(var objs in objects.Values)
        {
            foreach(var obj in objs.Values)
            {
                if(obj is ILocatorInitalizer)
                {
                    ((ILocatorInitalizer)obj).Initalize();
				}
			}
		}
	}
}