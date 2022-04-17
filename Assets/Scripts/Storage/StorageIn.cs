using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StorageIn : AStorageBase {

    [Tooltip("Типы предметов которые могут хранится в этом складе")]
    [SerializeField] private Item.TypeItem[] typesItems = new Item.TypeItem[0];

    public override bool IsCanAddItem(GameObject objItem) {
        if (!base.IsCanAddItem(objItem)) return false;

        Item item = objItem.GetComponent<Item>();
        if (Array.IndexOf(typesItems, item.type) == -1) return false;

        return true;
    }

    protected override void MoveItem() {

        GameObject objItem = invInTrigger.GetLastItem(typesItems);

        //Проверяем можем ли забрать у игрока предметы и если можем, перемещаем их на этот склад
        if (objItem != null && IsCanAddItem(objItem)) {
            invInTrigger.RemoveItem(objItem);
            AddItem(objItem);
        }

    }
}
