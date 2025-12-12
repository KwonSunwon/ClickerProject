using System.Collections.Generic;
using UnityEngine;

public class MineResultPanel : MonoBehaviour
{
    private GameObject _prefab;
    private List<GameObject> _items = new List<GameObject>();

    private void OnEnable()
    {
        _prefab ??= Resources.Load<GameObject>("Prefabs/UI/MineResult/MineResultItem");

        var mineralData = MineManager.Instance.ConsumeMineralBuffer();

        foreach (var itemData in mineralData) {
            var item = Instantiate(_prefab, transform);
            item.GetComponent<MineResultItem>().Set(itemData.Key, itemData.Value);
            _items.Add(item);
        }
    }

    private void OnDisable()
    {
        _items.ForEach(item => Destroy(item));
        _items.Clear();
    }
}
