using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotificationStopFabrics : MonoBehaviour {

    [SerializeField] private Text text = null;

    private IEnumerator coroutineClearText = null;

    private void Start() {
        //Подключаемся к эвенту остановки каждой фабрики, чтоб вывести уведомление на экран
        Factory[] allFactories = UnityEngine.Object.FindObjectsOfType<Factory>();
        foreach (Factory factory in allFactories) {
            factory.onStop?.AddListener(OnStopFactory);
        }
    }

    private void OnStopFactory(Factory.ReasonStop reasonStop, Item.TypeItem typeItem) {

        //Собираем текст для вывода на экран
        var strNotif = new System.Text.StringBuilder();

        strNotif.Append("Фабрика по производству ");
        if (typeItem == Item.TypeItem.Red) {
            strNotif.Append("красных прямоугольников ");
        } else if (typeItem == Item.TypeItem.Green) {
            strNotif.Append("зеленых прямоугольников ");
        } else if (typeItem == Item.TypeItem.Blue) {
            strNotif.Append("синих прямоугольников ");
        } else {
            strNotif.Append("неизвестно чего ");
        }
        strNotif.Append("остановилась");

        if (reasonStop == Factory.ReasonStop.FullStorage) {
            strNotif.Append(" так как склад с произведенными прямоугольниками полон");
        } else if (reasonStop == Factory.ReasonStop.NoResource) {
            strNotif.Append(" так как нехватает ресурсов для дальнейшего производства");
        }

        text.text = strNotif.ToString();

        //Запускаем очистку текста через время. Если она уже запущеная, перезапускаем чтоб сбросить таймер
        if (coroutineClearText != null) {
            StopCoroutine(coroutineClearText);
            coroutineClearText = null;
        }

        coroutineClearText = ClearTextDelay(5F);
        StartCoroutine(coroutineClearText);
    }

    private IEnumerator ClearTextDelay(float delay) {
        yield return new WaitForSeconds(delay);
        text.text = "";
    }
}
