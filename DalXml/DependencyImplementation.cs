namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";
    public int Create(Dependency item)
    {
        int id = Config.NextDependencyId;
        XElement newItem = new XElement("Dependency",
            new XElement ("Id", id),
            new XElement ("DependentTask", item.DependentTask),
            new XElement ("DependsOnTask", item.DependsOnTask)
            );
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        root.Add(newItem);
        XMLTools.SaveListToXMLElement(root, s_dependencies_xml);
        return id;
    }

    public void Delete(int id)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? item = root.Elements().FirstOrDefault(x => int.Parse(x.Element("Id").Value) == id);
        if(item == null)
            throw new DalDoesNotExistException($"Task with ID={id} doesn't exist");
        item.Remove();
        XMLTools.SaveListToXMLElement(root, s_dependencies_xml);
    }

    public Dependency? Read(int id)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? item = root.Elements().FirstOrDefault(x => int.Parse(x.Element("Id").Value) == id);
        if (item == null)
            return null;
        return new Dependency(int.Parse(item.Element("Id").Value),
                    int.Parse(item.Element("DependentTask").Value),
                    int.Parse(item.Element("DependsOnTask").Value)); 
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        Dependency t;
        foreach (XElement x in XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements()) 
        {
            t = new Dependency(int.Parse(x.Element("Id").Value),
                    int.Parse(x.Element("DependentTask").Value),
                    int.Parse(x.Element("DependsOnTask").Value));
            if (filter(t)) return t;
        }
        return null;
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if(filter == null)
        {
            return from x in XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements()
                   select new Dependency(int.Parse(x.Element("Id").Value),
                        int.Parse(x.Element("DependentTask").Value),
                        int.Parse(x.Element("DependsOnTask").Value));
        }
        return from x in XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements()
               where filter(new Dependency(int.Parse(x.Element("Id").Value),
                    int.Parse(x.Element("DependentTask").Value),
                    int.Parse(x.Element("DependsOnTask").Value)))
               select new Dependency(int.Parse(x.Element("Id").Value),
                    int.Parse(x.Element("DependentTask").Value),
                    int.Parse(x.Element("DependsOnTask").Value));

    }

    public void Update(Dependency item)
    {
        XElement newItem = new XElement("Dependency",
            new XElement("Id", item.Id),
            new XElement("DependentTask", item.DependentTask),
            new XElement("DependsOnTask", item.DependsOnTask)
            );
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? t = root.Elements().FirstOrDefault(x => int.Parse(x.Element("Id").Value) == item.Id);
        if (t == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} doesn't exist");
        t.ReplaceWith(newItem);
        XMLTools.SaveListToXMLElement(root, s_dependencies_xml);
    }
}