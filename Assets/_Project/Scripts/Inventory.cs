using System;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.UI;

// Item IDs:
// 0 - nothing
// 1 - camera
// 2 - key
// 3 - lever
// 4 - bucket

public class Inventory : SaveableBehaviour
{
    [SerializeField] private GameObject camUI;
    private bool haveCamera;

    [SerializeField] private GameObject[] itemIconPool;

    [SerializeField] private Transform posFirstItem;
    [SerializeField] private Vector3 iconOffset;

    [SerializeField] private Sprite bucketSprite;
    
    private class Item{
        public GameObject uiIcon;
        public int id;
    }
    private List<Item> currentItems = new List<Item>();

    [SerializeField] private GameObject newItemTip;
    [SerializeField] private TMP_Text textNewItem;

    void AddCamera(){
        haveCamera = true;
        camUI.SetActive(true);
        SetState("Item_1");
    }

    public bool GetHaveCamera(){
        return haveCamera;
    }

    public void AddItem(int id, PickupableItem.ItemIconData itemIconData){
        if (id == 1){
            AddCamera();
            ShowNewItemText(itemIconData.name);
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

        ShowNewItemText(itemIconData.name);

        Item newItem = new Item();
        newItem.id = id;
        newItem.uiIcon = freeIcon;

        freeIcon.transform.localPosition = posFirstItem.localPosition + iconOffset * currentItems.Count;
        
        currentItems.Add(newItem);
        SetState($"Item_{id}", itemIconData);
    }

    void ShowNewItemText(string name){
        CancelInvoke("HideNewItemTip");
        Invoke("HideNewItemTip", 3);
        newItemTip.SetActive(true);
        textNewItem.text = name;
    }

    void HideNewItemTip(){
        newItemTip.SetActive(false);
    }

    public bool RemoveItem(int id) {
        if (id == 1) {
            if (haveCamera) {
                haveCamera = false;
                camUI.SetActive(false);
            }
            return true;
        }

        int removeIndex = currentItems.FindIndex(item => item.id == id);
        if (removeIndex == -1) {
            return false;
        }
        
        currentItems[removeIndex].uiIcon.SetActive(false);
        currentItems.RemoveAt(removeIndex);
        
        for (int i = 0; i < currentItems.Count; i++) {
            currentItems[i].uiIcon.transform.localPosition = posFirstItem.localPosition + iconOffset * i;
        }
        return true;
    }

    public void AddBucket(){
        PickupableItem.ItemIconData itemIconData = new PickupableItem.ItemIconData();
        itemIconData.name = "КРАСКА";
        itemIconData.sprite = bucketSprite;
        itemIconData.widthHeight = new Vector2(393, 519);
        itemIconData.scale = 0.4f;
        itemIconData.blackOffset = 50;

        AddItem(4, itemIconData);
    }

    private void Start() => Initialize();

    public override void ResetState(Dictionary<string, object> states)
    {
        foreach (var item in currentItems)
            item.uiIcon.SetActive(false);
        currentItems.Clear();
    }

    public override void ApplyState(Dictionary<string, object> states)
    {
        foreach (var (key, value) in 
                 states.Where(pair => pair.Key.StartsWith("Item_")))
            AddItem(int.Parse(key[5..]), (PickupableItem.ItemIconData)value);
    }
}
