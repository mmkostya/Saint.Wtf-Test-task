using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Factory : MonoBehaviour {

    public enum ReasonStop { NoResource, FullStorage }

    [Header("Created Item")]
    [Tooltip("Какой предмет будем производить")]
    [SerializeField] private GameObject createdItem = null;

    [Tooltip("Локальная позиция спавна предмета")]
    [SerializeField] private Vector3 createdPosItem = Vector3.zero;

    [Tooltip("Локальный поворот спавна предмета")]
    [SerializeField] private Vector3 createdRotationItem = Vector3.zero;

    [Header("Storage")]
    [Tooltip("Склад куда помещать созданый предмет")]
    [SerializeField] private AStorageBase storageCretedItems = null;
    [Tooltip("Склад от куда забирать предметы предмет (Если нужен)")]
    [SerializeField] private AStorageBase storageTakeItems = null;

    [Header("Process create")]
    [Tooltip("Локальная позиция, куда переместятся предметы их переработки")]
    [SerializeField] private Vector3 removedPosItem = Vector3.zero;

    [Tooltip("Время создания предмета в секундах")]
    [SerializeField] private float timeCreatedItem = 10F;

    [Tooltip("Типы предметов которые нужны для создания предмета")]
    [SerializeField] private Item.TypeItem[] receipt = null;

    private bool isWork = false;
    public UnityEvent<ReasonStop, Item.TypeItem> onStop = new UnityEvent<ReasonStop, Item.TypeItem>();

    private Item.TypeItem typeCreatedItem;

    private void Start() {

        Item item = createdItem.GetComponent<Item>();
        typeCreatedItem = item.type;

        StartCoroutine(HandleFactory());
    }

    //Бесконечный цикл который запускает и останавливает производство
    private IEnumerator HandleFactory() {

        while (true) {

            if (storageCretedItems.IsCanAddItem(createdItem)) {

                if (RemoveItemsReceipt()) {
                    isWork = true;

                    yield return new WaitForSeconds(timeCreatedItem);
                    CreteItem();

                } else {
                    if (isWork) {
                        onStop?.Invoke(ReasonStop.NoResource, typeCreatedItem);
                        isWork = false;
                    }

                    yield return new WaitForSeconds(0.1F);
                }

            } else {
                if (isWork) {
                    onStop?.Invoke(ReasonStop.FullStorage, typeCreatedItem);
                    isWork = false;
                }

                yield return new WaitForSeconds(0.1F);
            }

        }
    }

    //Пытаемся забрать ресрсы для производства, если не хватает возвращаем false
    protected virtual bool RemoveItemsReceipt() {
        if (storageTakeItems != null && receipt != null) {

            if (storageTakeItems.countItems < receipt.Length) return false;

            var removedItemsFromInv = new List<GameObject>();
            foreach (Item.TypeItem typeItem in receipt) {

                GameObject objItem = storageTakeItems.GetLastItem(typeItem);

                if (objItem == null) {

                    foreach (GameObject remObjItem in removedItemsFromInv) {
                        storageTakeItems.AddItem(remObjItem);
                    }

                    return false;
                }

                storageTakeItems.RemoveItem(objItem);
                removedItemsFromInv.Add(objItem);
            }

            Transform transform = gameObject.transform;
            foreach (GameObject objItem in removedItemsFromInv) {
                objItem.transform.SetParent(transform);
                MoverGameObjectByLocalPos mover = objItem.GetComponent<MoverGameObjectByLocalPos>();
                
                //Отправляем обрабатывать движение объекта
                mover.MoveTo(removedPosItem, Vector3.zero); 

                //Удаляем GameObject предмета после того как предмет дойдет до нужной позиции
                mover.onEndMove.AddListener(delegate () { Destroy(objItem);  });
            }
        }

        return true;
    }

    //Создаем производимый предмет и добавляем его на склад
    protected virtual void CreteItem() {
        Transform transform = GetComponent<Transform>();

        //Создаем предмет в игровом мире
        GameObject objItem = Instantiate(createdItem);

        //Переводим позицию и поворот из локальных в глобальные координаты и ставим на них наш новый предмет
        Transform transformItem = objItem.transform;
        transformItem.position = transform.TransformPoint(createdPosItem);
        transformItem.rotation = transform.rotation * Quaternion.Euler(createdRotationItem);

        if (!storageCretedItems.AddItem(objItem)) {
            Debug.LogError("Error add item to storage");
        }
    }

}
