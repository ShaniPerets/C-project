using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    /// <summary>
    /// a generic interface with those action:
    /// create item, read an item by a rule, read all items, update an item and delete item by its id.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICrud<T> where T : class
    {
        int Create(T item); //Creates new entity object in DAL
        T? Read(int id); //Reads entity object by its ID
        IEnumerable<T?> ReadAll(Func <T,bool>? filter = null); //stage 1 only, Reads all entity objects
        void Update(T item); //Updates entity object
        void Delete(int id); //Deletes an object by its Id
        T ? ReadByFilter(Func<T, bool> filter); //return first element meet the filter function rule. (not only by id..)
        void RemoveAll(); //remove all items
    }

}
