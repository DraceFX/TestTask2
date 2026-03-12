using System.Collections.Generic;

public class SelectableRegistry
{
    public static readonly List<ObjectScript> Objects = new List<ObjectScript>();

    public static void Register(ObjectScript obj)
    {
        if (!Objects.Contains(obj))
            Objects.Add(obj);
    }

    public static void Unregister(ObjectScript obj)
    {
        Objects.Remove(obj);
    }
}

