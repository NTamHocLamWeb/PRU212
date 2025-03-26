using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> items;
    private Dictionary<int, GameObject> itemDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                items[i].Id = i + 1;
            }
        }

        foreach(Item item in items)
        {
            itemDictionary[item.Id] = item.gameObject;
        }
    }

    public GameObject GetItemPrefabs(int itemId)
    {
        itemDictionary.TryGetValue(itemId, out GameObject prefabs);
        if (prefabs == null)
        {
            Debug.LogWarning($"Item with id {itemId} not found in dictionary");
        }
        return prefabs;
    }
}
