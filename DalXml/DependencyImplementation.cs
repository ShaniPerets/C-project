using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;


namespace Dal;

internal class DependencyImplementation : IDependency
{
    //direct path to file dependencys.xml:
    private const string _s_dependencies = "dependencies";

    //create new dependency  by using the xml files
    public int Create(Dependency item)
    {
        List<DO.Dependency?> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(_s_dependencies);
        int id = Config.NextDependencyId;
        DO.Dependency copy = item with { Id = id };
        dependencies.Add(copy);
        XMLTools.SaveListToXMLSerializer<DO.Dependency>(dependencies, _s_dependencies);
        return id;
    }
    //delete dependency by using the xml files
    public void Delete(int id)
    {
        if (Read(id) is null)
        {
            throw new DalDoesNotExistException($"Dependency with Id = {id} is not exist");
        }
        List<DO.Dependency?> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency?>(_s_dependencies);
        dependencies.Remove(Read(id));
        XMLTools.SaveListToXMLSerializer<DO.Dependency>(dependencies, _s_dependencies);
    }

    //return a dependency by its id. null if does not find
    public Dependency? Read(int id)
    {
        List<DO.Dependency?> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency?>(_s_dependencies);
        DO.Dependency? d = dependencies.Where(d => d.Id == id).FirstOrDefault();
        return d;
    }

    //return a dependency matches a function's rule called filter. null if does not find
    public Dependency? ReadByFilter(Func<Dependency, bool> filter)
    {
        List<DO.Dependency?> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency?>(_s_dependencies);

        return dependencies.FirstOrDefault(t => filter(t));
    }

    //returns all the dependencys  by using the xml files
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(_s_dependencies);

        //return only tasks meet a rule
        if (filter != null)
        {
            return (from item in dependencies
                    where filter(item)
                    select item);
        }
        //return all the list
        return (from item in dependencies
                select item);
    }

    //uptade the dependency details  by using the xml files
    public void Update(Dependency item)
    {
        Delete(item.Id);
        Create(item);
    }

    /// <summary>
    /// remove all dependencies from database
    /// </summary>
    public void RemoveAll()
    {
        List<DO.Dependency?> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(_s_dependencies);
        dependencies.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Dependency>(dependencies, _s_dependencies);

        //restart the id in config to 100
        XElement configData = XMLTools.LoadListFromXMLElement("data-config");
        configData.Element("NextDependencyId").Value = "100";
        XMLTools.SaveListToXMLElement(configData, "data-config");
    }
}