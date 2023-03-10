using System.Collections;

namespace Battery.Framework;

/// <summary>
///     Represents a list of objects that are stored dynamically.
/// </summary>
public class Table<T> : IEnumerable where T : class
{
    /// <summary>
    ///     A list storing all elements of the Table.
    /// </summary>
    public List<T> Values = new List<T>();

    /// <summary>
    ///     A dictionary storing all elements of the Table, sorting by type.
    /// </summary>
    public Dictionary<Type, List<T>> Lookup = new Dictionary<Type, List<T>>();

    /// <summary>
    ///     THe number of elements in the Table.
    /// </summary>
    public int Count => Values.Count;

    /// <summary>
    ///     Add an element to the list.
    /// </summary>
    /// <param name="element">The element to be added.</param>
    public T Add(T element)
    {
        if (Lookup.ContainsKey(element.GetType()) == false)
            Lookup.Add(element.GetType(), new List<T>());
        
        // Add the element to the list and the dictionary.
        Values.Add(element);
        Lookup[element.GetType()].Add(element);

        return element;
    }

    /// <summary>
    ///     Remove an element from the list.
    /// </summary>
    /// <param name="element">The element to be removed.</param>
    public T Remove(T element)
    {
        if (Lookup.ContainsKey(element.GetType()) == false)
            throw new Exception("No exists an element of the given type in this table.");

        // Remove the element from the list and the dictionary.
        Values.Remove(element);
        Lookup[element.GetType()].Remove(element);

        return element;
    }

    /// <summary>
    ///     Gets an element in the list that matches the given argument.
    /// </summary>
    public U? Get<U>() where U : class
    {
        // Loop for every element in the table and check if the element matches the given type.
        foreach (var toCheck in Values)
        {
            if (toCheck is U result)
                return result;   
        }

        // No element with the given type in this table.
        return null;
    }

    /// <summary>
    /// Determines whether an element is in the table. 
    /// </summary>
    /// <param name="element">The element to check.</param>
    public bool Contains(T element) 
        => Values.Contains(element);

    /// <summary>
    /// Sorts the table.
    /// </summary>
    /// <param name="comparer"></param>
    public void Sort(IComparer<T> comparer) 
        => Values.Sort(comparer);

    /// <summary>
    /// Sorts the table.
    /// </summary>
    /// <param name="comparer"></param>
    public void Sort(Comparison<T> comparer) 
        => Values.Sort(comparer);

    public void Clear()
    {
        Values.Clear();
        Lookup.Clear();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    public int IndexOf(T element)
        => Values.IndexOf(element);

    /// <summary>
    /// Returns the IEnumerator function.
    /// </summary>
    public IEnumerator<T> GetEnumerator()
    {
        var list = new List<T>(Values);

        for (int i = 0; i < list.Count; i ++)
            yield return list[i];
    }

    /// <summary>
    /// IEnumerator function. Used in foreach.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() 
        => GetEnumerator();
}