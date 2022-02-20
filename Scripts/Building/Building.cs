using System.Collections;
using System.Linq;
using UnityEngine;

public class Building : MonoBehaviour
{
    [System.Serializable]
    public struct RequiredResource
    {
        public Resource Resource;
        public int RequiredCount;
        public int ResourceMaxStoreCount;
    }

    [Header("Building")]
    [SerializeField] private string _name;
    [SerializeField] private float _productionDelay;
    [Header("Input")]
    [SerializeField] private RequiredResource[] _requiredResources;
    [SerializeField] private BuildingStorage _inputResourceStorage;
    [Header("Output")]
    [SerializeField] private Resource _outputResource;
    [SerializeField] private BuildingStorage _outputResourceStorage;

    public Resource OutputResource => _outputResource;
    public RequiredResource[] RequiredResources => _requiredResources;

    IEnumerator SpawnResources()
    {
        while (true)
        {
            if (!_outputResourceStorage.IsFull)
            {
                _outputResourceStorage.Put(Instantiate(_outputResource), 0f);
            }
            yield return new WaitForSeconds(_productionDelay);
        }
    }

    IEnumerator ProduceResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(_productionDelay);
            if (!_inputResourceStorage.IsEmpty && !_outputResourceStorage.IsFull)
            {
                bool resourcesAvailable = false;
                foreach (var requiredResource in _requiredResources)
                {
                    resourcesAvailable = _inputResourceStorage.CountResource(requiredResource.Resource) >= requiredResource.RequiredCount;

                    if (!resourcesAvailable)
                        break;
                }

                if (resourcesAvailable)
                {
                    foreach (var requiredResource in _requiredResources)
                    {
                        _inputResourceStorage.DestroyFromEnd(requiredResource.Resource, requiredResource.RequiredCount);
                    }
                    _outputResourceStorage.Put(Instantiate(_outputResource), 0f);
                }
            }
        }
    }

    private void Start()
    {
        CheckProduction();
    }

    private void CheckProduction()
    {
        if (_outputResource != null)
        {
            if (_requiredResources.Length > 0)
            {
                StartCoroutine(ProduceResources());
            }
            else
            {
                StartCoroutine(SpawnResources());
            }
        }
    }

    public Resource GetResource()
    {
        return _outputResourceStorage != null ? _outputResourceStorage.PullLast() : null;
    }

    public bool TakeResource(Resource resource, float time)
    {
        if (_inputResourceStorage != null)
        {

            if (_inputResourceStorage.Put(resource, time))
            {
                return true;
            }
        }
        return false;
    }
}
