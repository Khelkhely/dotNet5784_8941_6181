namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";
    public void Clear()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        root.RemoveAll();
        XMLTools.SaveListToXMLElement(root, s_dependencies_xml);
    }
        
    public int Create(Dependency item)
    {
        int id = Config.NextDependencyId;
        XElement newItem = DependencyToXelement(item with { Id = id });
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        root.Add(newItem);
        XMLTools.SaveListToXMLElement(root, s_dependencies_xml);
        return id;
    }

    public void Delete(int id)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? item = root.Elements().FirstOrDefault(x => int.Parse(x.Element("Id")!.Value) == id);
        if(item == null)
            throw new DalDoesNotExistException($"Task with ID={id} doesn't exist");
        item.Remove();
        XMLTools.SaveListToXMLElement(root, s_dependencies_xml);
    }

    public Dependency? Read(int id)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? item = root.Elements().FirstOrDefault(x => int.Parse(x.Element("Id")!.Value) == id);
        if (item == null)
            return null;
        return XelementToDependency(item);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        Dependency t;
        foreach (XElement x in XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements()) 
        {
            t = XelementToDependency(x);
            if (filter(t)) return t;
        }
        return null;
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if(filter == null)
        {
            return from x in XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements()
                   select XelementToDependency(x);
        }
        return from x in XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements()
               where filter(XelementToDependency(x))
               select XelementToDependency(x);

    }

    public void Update(Dependency item)
    {
        XElement newItem = DependencyToXelement(item);
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? t = root.Elements().FirstOrDefault(x => XMLTools.ToIntNullable(x, "Id") == item.Id);
        if (t == null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} doesn't exist");
        t.ReplaceWith(newItem);
        XMLTools.SaveListToXMLElement(root, s_dependencies_xml);
    }
    static Dependency XelementToDependency(XElement x)
    {
        return new Dependency(XMLTools.ToIntNullable(x, "Id") ?? 0,
                        XMLTools.ToIntNullable(x, "DependentTask") ?? 0,
                        XMLTools.ToIntNullable(x, "DependsOnTask") ?? 0);
    }
    static XElement DependencyToXelement (Dependency item)
    {
        return new XElement("Dependency",
            new XElement("Id", item.Id),
            new XElement("DependentTask", item.DependentTask),
            new XElement("DependsOnTask", item.DependsOnTask)
            );
    }
}