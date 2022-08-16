namespace CaptainCoder;
using System.Collections;

public class OrderedSet<T> : IEnumerable<T>
{
    private readonly HashSet<T> _set = new ();
    private readonly List<T> _list = new ();
    public int Count => _set.Count;
    public void Add(T toAdd) {
        if (_set.Contains(toAdd)) return;
        _set.Add(toAdd);
        _list.Add(toAdd);
    }
    public void Remove(T toRemove) {
        if (_set.Contains(toRemove)) return;
        _set.Remove(toRemove);
        _list.Remove(toRemove);
    }

    public T this[int index] {
        get => _list[index];
        set => _list[index] = value;
    }


    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
}