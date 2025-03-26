using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    public GameObject gameSession;
    private static InventoryController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (inventoryPanel == null)
            {
                GameObject gameTabs = GameObject.Find("GameSession");
                if (gameTabs != null)
                {
                    inventoryPanel = gameTabs.transform.Find("GameTabs/Panel/Pages/InventoryPage")?.gameObject;
                }
            }
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    private string GetInventoryKey() => $"InventoryData_{SceneManager.GetActiveScene().buildIndex}";

    void OnEnable()
    {
        LoadInventory(); // ??m b?o khi object ???c kích ho?t l?i, inventory c?ng ???c load l?i
    }


    void Start()
    {

        // Kh?i t?o slot
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }

        Debug.Log("Inventory is full");
        return false;
    }

    public bool HasItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null && slot.currentItem.name.StartsWith(itemPrefab.name))
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null && slot.currentItem.name.StartsWith(itemPrefab.name))
            {
                Destroy(slot.currentItem);
                slot.currentItem = null;
                Debug.Log($"?ã dùng {itemPrefab.name}.");
                return;
            }
        }
    }

    public void LoadInventory()
    {
        string key = GetInventoryKey();
        if (!PlayerPrefs.HasKey(key)) return;

        string jsonData = PlayerPrefs.GetString(key);
        SerializableInventoryData savedData = JsonUtility.FromJson<SerializableInventoryData>(jsonData);

        if (savedData == null || savedData.inventoryItems == null) return;

        Debug.Log("Inventory Loaded: " + jsonData);

        // Xóa item c?
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // T?o slot m?i
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        // Load item vào slot
        foreach (InventorySaveData data in savedData.inventoryItems)
        {
            if (data.slotIndex < inventoryPanel.transform.childCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefabs(data.itemId);

                if (slot != null && itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }

    public void SaveInventory()
    {
        List<InventorySaveData> inventoryData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot?.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null)
                {
                    inventoryData.Add(new InventorySaveData
                    {
                        itemId = item.Id,
                        slotIndex = slotTransform.GetSiblingIndex(),
                    });
                }
            }
        }

        SerializableInventoryData serializedData = new SerializableInventoryData { inventoryItems = inventoryData };
        string jsonData = JsonUtility.ToJson(serializedData);

        string key = GetInventoryKey();
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();

        Debug.Log($"Inventory Saved for Scene {SceneManager.GetActiveScene().buildIndex}: {jsonData}");
    }
}

[System.Serializable]
public class SerializableInventoryData
{
    public List<InventorySaveData> inventoryItems;
}
