using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] protected Container[] _containers;

    public bool IsEmpty => GetCurrentContainer().IsEmpty;
    public bool IsFull => GetCurrentContainer().IsFull;

    protected virtual int StorageCapacity()
    {
        int capacity = 0;
        foreach (var container in _containers)
        {
            capacity += container.Capacity;
        }
        return capacity;
    }

    public virtual Resource PullFromEnd(Resource resource)
    {
        if (resource == null)
            return null;

        for (int i = _containers.Length - 1; i >= 0; i--)
        {
            var container = _containers[i];
            var res = container.PullFromEnd(resource);
            Sort();
            if (res != null)
            {
                return res;
            }
        }
        return null;
    }

    public virtual bool Put(Resource resource, float time)
    {
        if (resource == null || IsFull)
            return false;

        GetCurrentContainer().Put(resource, time);
        return true;
    }

    public virtual Resource PullLast()
    {
        if (!IsEmpty)
        {
            for (int i = _containers.Length - 1; i >= 0; i--)
            {
                var container = _containers[i];
                if (!container.IsEmpty)
                {
                    var last = container.PullLast();
                    return last;
                }
            }
        }
        return null;
    }

    public virtual void DestroyLast()
    {
        if (!IsEmpty)
        {
            GetCurrentContainer().DestroyLast();
        }
    }

    public virtual void DestroyFromEnd(Resource resource, int count)
    {
        if (!IsEmpty)
        {
            int needDestroy = count;
            for (int i = _containers.Length - 1; i >= 0; i--)
            {
                var container = _containers[i];
                var available = container.CountResource(resource);
                if (available < needDestroy)
                {
                    needDestroy -= available;
                    container.DestroyFromEnd(resource, needDestroy);
                }
                else
                {
                    container.DestroyFromEnd(resource, needDestroy);
                    break;
                }
            }
        }
    }

    public virtual int CountResource(Resource resource)
    {
        int count = 0;
        foreach (var container in _containers)
        {
            count += container.CountResource(resource);
        }

        return count;
    }

    protected virtual void Sort()
    {
        if (!IsEmpty)
        {
            foreach (var container in _containers)
            {
                container.Sort();
            }
        }
    }

    protected virtual Container GetNotFullContainer()
    {
        foreach (var container in _containers)
        {
            if (!container.IsFull)
            {
                return container;
            }
        }
        return null;
    }

    protected virtual Container GetCurrentContainer()
    {
        var container = GetNotFullContainer();
        return container != null ? container : _containers[_containers.Length - 1];
    }
}
