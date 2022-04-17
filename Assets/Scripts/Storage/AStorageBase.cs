using System;
using System.Collections.Generic;
using UnityEngine;
 
public abstract class AStorageBase : Inventory {

    [Tooltip("Время которое уходит на перемещение предмета")]
    [SerializeField] private float intervalMoveItem = 0.2f;

    private float timerMoveItem = 0;
    protected Inventory invInTrigger = null;

    private void OnTriggerEnter(Collider other) {
        GameObject obj = other.gameObject;
        Inventory inv = obj.GetComponent<Inventory>();
        if (inv != null) invInTrigger = inv;
    }

    private void OnTriggerExit(Collider other) {
        GameObject obj = other.gameObject;
        Inventory inv = obj.GetComponent<Inventory>();
        if (inv != null && invInTrigger == inv) invInTrigger = null;
    }

    private void FixedUpdate() {

        if (invInTrigger != null) {

            while(timerMoveItem >= intervalMoveItem) {
                MoveItem();
                timerMoveItem -= intervalMoveItem;
            }

            timerMoveItem += Time.deltaTime;
        } else {
            timerMoveItem = 0;
        }

    }

    protected abstract void MoveItem();

}
