using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class Utils
{
    public static T DeepClone<T>(T obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Position = 0;

            return (T)formatter.Deserialize(stream);
        }
    }

    public static bool IsPrimitive(Type type)
    {
        return type.IsPrimitive
            || type.Equals(typeof(string));
    }

    public static dynamic TryParse(dynamic obj, string str, out bool sucess)
    {
        if (obj is string)
        {
            sucess = true;
            return str;
        }
        else if (obj is int)
        {
            int t;
            sucess = int.TryParse(str, out t);
            return sucess ? t : obj;
        }
        else if (obj is float)
        {
            float t;
            sucess = float.TryParse(str, out t);
            return sucess ? t : obj;
        }
        else if (obj is double)
        {
            double t;
            sucess = double.TryParse(str, out t);
            return sucess ? t : obj;
        }
        else if (obj is char)
        {
            sucess = true;
            return str.First();
        }
        sucess = false;
        return obj;
    }
}

