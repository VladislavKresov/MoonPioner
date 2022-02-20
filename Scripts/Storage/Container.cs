using System;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private int _capacity;
    private Transform[] _transforms = new Transform[0];

    public Transform[] DockingTransforms => _transforms;

    private List<Resource> _resources = new List<Resource>();
    public int ResourcesCount => _resources.Count;
    public int Capacity => _capacity;
    public bool IsEmpty => _resources.Count == 0;
    public bool IsFull => _resources.Count == _capacity;

    private void Start()
    {
        _capacity = transform.childCount;
        _transforms = new Transform[_capacity];
        for (int i = 0; i < _capacity; i++)
        {
            _transforms[i] = transform.GetChild(i);
        }
    }

    public Resource PullFromEnd(Resource resource)
    {
        for (int i = _resources.Count - 1; i >= 0; i--)
        {
            var res = _resources[i];
            if (res.GetType().Equals(resource.GetType()))
            {
                res.transform.parent = null;
                _resources.Remove(res);
                return res;
            }
        }
        return null;
    }

    // puts to storage and places resource to empty dock
    public bool Put(Resource resource, float time)
    {
        if (_resources.Count < _capacity)
        {
            _resources.Add(resource);
            PlaceResource(resource, ResourcesCount - 1, time);
            return true;
        }
        return false;
    }

    // returns last resource and removes it from storage
    public Resource PullLast()
    {
        var resource = _resources[ResourcesCount - 1];
        resource.transform.parent = null;
        _resources.Remove(resource);
        return resource;
    }

    public void DestroyLast()
    {
        Resource resource;
        for (int i = _resources.Count - 1; i >= 0; i--)
        {
            resource = _resources[i];
            if (resource != null)
            {
                Destroy(resource.gameObject);
                _resources.RemoveAt(i);
                break;
            }
        }
    }

    public void DestroyFromEnd(Resource resourceToDestroy, int count)
    {
        Resource resource;
        int destroyed = 0;
        for (int resIndex = _resources.Count - 1; ( resIndex >= 0 && destroyed < count ); resIndex--)
        {
            resource = _resources[resIndex];
            if (resourceToDestroy != null && resource.GetType().Equals(resourceToDestroy.GetType()))
            {
                Destroy(resource.gameObject);
                _resources.RemoveAt(resIndex);
                destroyed++;
            }
        }
    }

    public int CountResource(Resource resource)
    {
        int count = 0;
        foreach (var res in _resources)
        {
            if (res.GetType().Equals(resource.GetType()))
            {
                count++;
            }
        }
        return count;
    }

    public void Sort()
    {
        for (int i = 0; i < _resources.Count; i++)
        {
            PlaceResource(_resources[i], i, 0f);
        }
    }

    private void PlaceResource(Resource resource, int dockIndex, float time)
    {
        resource.transform.SetParent(_transforms[dockIndex], true);
        resource.MoveToLocalParent(time);
    }
}
