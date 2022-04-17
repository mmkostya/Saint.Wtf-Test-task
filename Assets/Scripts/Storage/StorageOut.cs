using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageOut : AStorageBase {
    protected override void MoveItem() {

        GameObject objItem = GetLastItem();

        //Проверяем можем ли забрать у склада предметы и если можем, перемещаем их в инвентарь к игроку
        if (objItem != null && invInTrigger.IsCanAddItem(objItem)) {
            RemoveItem(objItem);
            invInTrigger.AddItem(objItem);
        }
        
    }
}
