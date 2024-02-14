using DO;
using System;
using System.Reflection;

namespace BO;

static class Tools
{
    public static string ToStringProperty<T>(this T t)
    {
        string str = "";
        if (t != null)
        {
            foreach (PropertyInfo item in t.GetType().GetProperties())
            {
                if ((item.GetValue(t,null) is System.Collections.IEnumerable) && 
                    !(item.GetValue(t, null) is string)) //if the property is a collection, print every item in it
                {
                    str += "\n" + item.Name + ":\n{";
                    foreach (var b in (System.Collections.IEnumerable)item.GetValue(t, null))
                    {
                        str += "\t" + b.ToString();
                        str += "\n";
                    }
                    str.Trim(' ', '\n');
                    str += "}";
                }
                else
                    str += "\n" + item.Name + ": " + item.GetValue(t, null);
            }
        }
        return str;
    }
}
