using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Item IDs:
// 0 - nothing
// 1 - camera
// 2 - key
// 3 - lever

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject camUI;
    private bool haveCamera;

    [SerializeField] private GameObject[] itemIconPool;

    [SerializeField] private Transform posFirstItem;
    [SerializeField] private Vector3 iconOffset;
    
    private class Item{
        public GameObject uiIcon;
        public int id;
    }
    private List<Item> currentItems = new List<Item>();

    void AddCamera(){
        haveCamera = true;
        camUI.SetActive(true);
    }

    public bool GetHaveCamera(){
        return haveCamera;
    }

    public void AddItem(int id, PickupableItem.ItemIconData itemIconData){
        if (id == 1){
            AddCamera();
            return;
        }
        
        GameObject freeIcon = null;
        for(int i = 0; i < itemIconPool.Length; i++){
            if (!itemIconPool[i].activeSelf){
                freeIcon = itemIconPool[i];
                break;
            }
        }
        if (freeIcon == null){
            ////////////////
        }

        freeIcon.SetActive(true);

        Image img1 = freeIcon.transform.Find("black").GetComponent<Image>();
        Image img2 = freeIcon.transform.Find("main").GetComponent<Image>();
        RectTransform rect1 = freeIcon.transform.Find("black").GetComponent<RectTransform>();
        RectTransform rect2 = freeIcon.transform.Find("main").GetComponent<RectTransform>();
        rect2.sizeDelta = new Vector2(itemIconData.widthHeight.x, itemIconData.widthHeight.y);
        rect2.localScale = Vector3.one * itemIconData.scale;
        rect1.sizeDelta = new Vector2(itemIconData.widthHeight.x + itemIconData.blackOffset, itemIconData.widthHeight.y + itemIconData.blackOffset);
        rect1.localScale = Vector3.one * itemIconData.scale;
        img1.sprite = itemIconData.sprite;
        img2.sprite = itemIconData.sprite;

        Item newItem = new Item();
        newItem.id = id;
        newItem.uiIcon = freeIcon;

        freeIcon.transform.localPosition = posFirstItem.localPosition + iconOffset * currentItems.Count;
        
        currentItems.Add(newItem);
    }
}
