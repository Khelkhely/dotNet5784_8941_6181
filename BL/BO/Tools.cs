using System;
using System.Reflection;

namespace BO;

static class Tools
{
    public static string ToStringProperty<T>(this T t)
    {
        string str = "";
        foreach (PropertyInfo item in t.GetType().GetProperties())
        {
            //Type type = item.Name.GetType();
            //if(type==List<T>)
            str += "\n" + item.Name + ": " + item.GetValue(t, null);
        }
        return str;
    }
}
