using UnityEngine;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class StorageInfoView : MonoBehaviour
{
    [System.Serializable]
    public struct RequiredResource
    {
        public Resource Resource;
        public int Count;
        public int Required;
    }

    [Header("UI")]
    [SerializeField] private ResourceItem _resourceItemPrefab;
    [SerializeField] private RectTransform _resourcesHolder;
    [SerializeField] private TMP_Text _messageTMP;
    [Header("Resources")]
    [SerializeField] private RequiredResource[] _resources;

    public void MessageVisible(bool isVisible)
    {
        _messageTMP.gameObject.SetActive(isVisible);
    }

    public void SetResources(RequiredResource[] resources)
    {
        _resources = resources;
        ClearResourcesView();
        InitializeResourcesView();
    }

    private void Start()
    {
        InitializeResourcesView();
    }

    private void InitializeResourcesView()
    {
        for (int i = 0; i < _resources.Length; i++)
        {
            ResourceItem item = Instantiate(_resourceItemPrefab, _resourcesHolder);
            Instantiate(_resources[i].Resource, item.ResourceHolder);
            item.ResourceText.text = _resources[i].Count + "/" + _resources[i].Required;
        }
    }

    private void ClearResourcesView()
    {
        foreach (RectTransform child in _resourcesHolder)
        {
            Destroy(child.gameObject);
        }
    }
}
