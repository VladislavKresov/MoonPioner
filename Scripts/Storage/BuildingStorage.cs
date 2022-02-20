using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BuildingStorage : Storage
{
    [SerializeField] private Building _parentBuilding;
    [SerializeField] private StorageInfoView _storageInfo;
    [SerializeField] private bool _allowGrab;
    [SerializeField] private bool _allowPut;

    public Building ParentBuilding => _parentBuilding;
    public bool GrabAllowed => _allowGrab;
    public bool PutAllowed => _allowPut;

    public override void DestroyFromEnd(Resource resource, int count)
    {
        base.DestroyFromEnd(resource, count);
        UpdateStorageInfo();
    }

    public override void DestroyLast()
    {
        base.DestroyLast();
        UpdateStorageInfo();
    }

    public override Resource PullFromEnd(Resource resource)
    {
        var res = base.PullFromEnd(resource);
        UpdateStorageInfo();
        return res;
    }

    public override Resource PullLast()
    {
        var res = base.PullLast();
        UpdateStorageInfo();
        return res;
    }

    public override bool Put(Resource resource, float time)
    {
        var res = base.Put(resource, time);
        UpdateStorageInfo();
        return res;
    }

    private void UpdateStorageInfo()
    {
        _storageInfo.MessageVisible(IsFull || IsEmpty && _allowPut);
        if (_allowPut)
        {
            Building.RequiredResource[] requireds = _parentBuilding.RequiredResources;
            if (requireds != null)
            {
                StorageInfoView.RequiredResource[] resources = new StorageInfoView.RequiredResource[requireds.Length];
                for (int i = 0; i < requireds.Length; i++)
                {
                    resources[i].Resource = requireds[i].Resource;
                    resources[i].Count = CountResource(requireds[i].Resource);
                    resources[i].Required = requireds[i].RequiredCount;
                }
                _storageInfo.SetResources(resources);
            }
        }
        else
        {
            StorageInfoView.RequiredResource[] resources = new StorageInfoView.RequiredResource[1];
            resources[0].Resource = _parentBuilding.OutputResource;
            resources[0].Count = CountResource(_parentBuilding.OutputResource);
            resources[0].Required = StorageCapacity();
            _storageInfo.SetResources(resources);
        }
    }
}