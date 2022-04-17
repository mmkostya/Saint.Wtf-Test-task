using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    [Tooltip("Сколько предметов можно поместить в этот инвентарь")]
    [SerializeField] private int limitItems = 10;

    [Header("Sort 3D objects")]
    [Tooltip("Позиция спавна предмета")]
    [SerializeField] private Vector3 posFirstItem = Vector3.zero;

    [Tooltip("Позиция спавна предмета")]
    [SerializeField] private Vector3 offsetPosItem = Vector3.zero;

    [Tooltip("Поворот спавна предмета")]
    [SerializeField] private Vector3 rotationItem = Vector3.zero;

    //Все 3D предметы
    private List<GameObject> allObjectsItems = new List<GameObject>();

    public int countItems { get { return allObjectsItems.Count; } }

    public virtual bool IsCanAddItem(GameObject objItem) {
        Item item = objItem.GetComponent<Item>();
        if (item == null) {
            Debug.LogError("Error. No exist component Item");
            return false;
        }

        MoverGameObjectByLocalPos mover = objItem.GetComponent<MoverGameObjectByLocalPos>();
        if (mover == null) {
            Debug.LogError("Error. No exist component MoverGameObject");
            return false;
        }

        if (allObjectsItems.Count >= limitItems) return false;

        return true;
    }

    public bool AddItem(GameObject objItem) {
        Item item = objItem.GetComponent<Item>();
        if (item == null) {
            Debug.LogError("Error. No exist component Item");
            return false;
        }

        MoverGameObjectByLocalPos mover = objItem.GetComponent<MoverGameObjectByLocalPos>();
        if (mover == null) {
            Debug.LogError("Error. No exist component MoverGameObject");
            return false;
        }

        int index = allObjectsItems.Count;
        allObjectsItems.Add(objItem);
        objItem.transform.SetParent(gameObject.transform);

        MoveItemToIndexPos(mover, index);

        return true;
    }

    public GameObject GetLastItem(Item.TypeItem typeItem) => GetLastItem(new Item.TypeItem[]{typeItem});

    public GameObject GetLastItem(Item.TypeItem[] typesItems) {
        for(int i = allObjectsItems.Count - 1; i >= 0; --i) {
            GameObject objItem = allObjectsItems[i];
            Item item = objItem.GetComponent<Item>();
            if (Array.IndexOf(typesItems, (item.type)) != -1) return objItem;
        }

        return null;
    }

    public GameObject GetLastItem() {
        int lastIndex = allObjectsItems.Count - 1;
        if (lastIndex < 0) return null;
        GameObject objItem = allObjectsItems[lastIndex];
        return objItem;
    }

    public void RemoveItem(GameObject objItem) {
        allObjectsItems.Remove(objItem);
        UpdatePosItems();//Сдвигаем все 3D предметы на свои позиции. Это нужно если мы вытащили предмет из середины
    }

     public void UpdatePosItems() {

        for (int i = 0; i < allObjectsItems.Count; ++i) {
            GameObject objItem = allObjectsItems[i];

            MoverGameObjectByLocalPos mover = objItem.GetComponent<MoverGameObjectByLocalPos>();
            MoveItemToIndexPos(mover, i);
        }
    }

    private void MoveItemToIndexPos(MoverGameObjectByLocalPos mover, int index) {
        //Отправляем обрабатывать движение объекта
        mover.MoveTo(posFirstItem + (offsetPosItem * index), rotationItem);
    }
}
