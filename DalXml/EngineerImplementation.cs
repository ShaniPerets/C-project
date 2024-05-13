using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;


namespace Dal;

internal class EngineerImplementation : IEngineer
{
    //direct path to file engineers.xml:
    private const string _s_engineers = "engineers";

    static DO.Engineer? createEngineerfromXElement(XElement e)
    {
        return new DO.Engineer
        {
            Id = (int)e.Element("Id"),
            Name = (string)e.Element("Name"),
            EMail = (string)e.Element("Email"),
            Level = (EngineerExperience)e.ToEnumNullable<DO.EngineerExperience>("Level"),
            Cost = (double)e.Element("Cost")
        };

    }

    //create new engineer  by using the xml files

    public int Create(Engineer item)
    {
        XElement engRoot = XMLTools.LoadListFromXMLElement(_s_engineers);

        if (Read(item.Id) != null)
        {
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        }


        XElement eng = new XElement("Engineer",
                                   new XElement("Id", item.Id),
                                   new XElement("Name", item.Name),
                                   new XElement("Email", item.EMail),
                                   new XElement("Level", item.Level),
                                   new XElement("Cost", item.Cost)
                                   );

        engRoot.Add(eng);

        XMLTools.SaveListToXMLElement(engRoot, _s_engineers);

        return item.Id;
    }

    //delete engineer by using the xml files
    public void Delete(int id)
    {
        XElement engRoot = XMLTools.LoadListFromXMLElement(_s_engineers);

        XElement? eng = (from e in engRoot.Elements()
                         where (int?)e.Element("Id") == id
                         select e).FirstOrDefault() ?? throw new DalDoesNotExistException($"Engineer with Id = {id} is not exist");

        eng.Remove(); //<==>   Remove engineer from engRoot

        XMLTools.SaveListToXMLElement(engRoot, _s_engineers);
    }

    //return aengineer by its id. null if does not find
    public Engineer? Read(int id)
    {
        XElement engRoot = XMLTools.LoadListFromXMLElement(_s_engineers);

        return (from e in engRoot.Elements()
                let eng = createEngineerfromXElement(e)
                where eng.Id == id
                select (DO.Engineer?)eng).FirstOrDefault();
    }

    //return a engineer matches a function's rule called filter. null if does not find
    public Engineer? ReadByFilter(Func<Engineer, bool> filter)
    {
        XElement engRoot = XMLTools.LoadListFromXMLElement(_s_engineers);

        return (from e in engRoot.Elements()
                let eng = createEngineerfromXElement(e)
                where filter(eng)
                select (DO.Engineer?)eng).FirstOrDefault();
    }

    //returns all the engineers  by using the xml files

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        XElement engRoot = XMLTools.LoadListFromXMLElement(_s_engineers);

        if (filter != null)
        {
            return (from e in engRoot.Elements()
                    let eng = createEngineerfromXElement(e)
                    where filter(eng)
                    select (DO.Engineer?)eng);
        }
        else
        {
            return (from e in engRoot.Elements()
                    select createEngineerfromXElement(e));
        }
    }

    //uptade the engineer details  by using the xml files

    public void Update(Engineer item)
    {

        Delete(item.Id);
        Create(item);
    }

    /// <summary>
    /// remove all engineers from database
    /// </summary>
    public void RemoveAll()
    {
        XElement engRoot = XMLTools.LoadListFromXMLElement(_s_engineers);

        engRoot.Elements().Remove();

        XMLTools.SaveListToXMLElement(engRoot, _s_engineers);

    }
}
