namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";
    public void Clear()
    {
        XMLTools.SaveListToXMLSerializer<Engineer>(new List<Engineer>(), s_engineers_xml);
    }

    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (engineers.RemoveAll(x => x.Id == id) == 0)
            throw new DalDoesNotExistException($"Engineer with ID={id} doesn't exist");
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
    }

    public Engineer? Read(int id)
    {
        IEnumerable<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        Engineer? result = engineers.FirstOrDefault(x => x.Id == id);

        return result;
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        IEnumerable<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        Engineer? result = engineers.FirstOrDefault(x => filter(x));

        return result;
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (filter == null)
            return engineers;
        return engineers.Where(filter);
    }

    public void Update(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (engineers.RemoveAll(x => x.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} doesn't exist");

        engineers.Add(item);

        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
    }
}
