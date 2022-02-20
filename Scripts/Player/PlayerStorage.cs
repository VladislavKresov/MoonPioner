using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerStorage : Storage
{
    [Header("Storage")]
    [SerializeField] private float _unloadingDelay;
    [SerializeField] private float _grabDistance;

    private float _timer;

    private void OnTriggerStay(Collider other)
    {
        _timer += Time.fixedDeltaTime;

        if (_timer >= _unloadingDelay)
        {
            if (other.TryGetComponent(out BuildingStorage storage))
            {
                var building = storage.ParentBuilding;
                if (building != null)
                {
                    if (!storage.IsFull && storage.PutAllowed)
                    {
                        Resource resource;
                        foreach (var requiredResource in building.RequiredResources)
                        {
                            if (storage.CountResource(requiredResource.Resource) < requiredResource.ResourceMaxStoreCount)
                            {
                                resource = PullFromEnd(requiredResource.Resource);
                                if (resource != null)
                                {
                                    building.TakeResource(resource, _unloadingDelay);
                                    _timer = 0;
                                    break;
                                }
                            }
                        }
                    }
                    if (!IsFull && storage.GrabAllowed)
                    {
                        Put(building.GetResource(), _unloadingDelay);
                        _timer = 0;
                    }
                }
            }
        }
    }
}
