using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoverGameObjectByLocalPos : MonoBehaviour {

    [SerializeField] private float speed = 100F;

    private float state = 1;
    private Vector3 posFrom, posTo;
    private Quaternion rotationFrom, rotationTo;

    private bool isMoved = false;
    public UnityEvent onEndMove = new UnityEvent();

    public void MoveTo(Vector3 pos, Vector3 rotation) => MoveTo(pos, Quaternion.Euler(rotation));

    public void MoveTo(Vector3 newPos, Quaternion newRotation) {

        Transform transform = GetComponent<Transform>();
        posFrom = transform.localPosition;
        posTo = newPos;

        rotationFrom = transform.localRotation;
        rotationTo = newRotation;

        state = 0;
        isMoved = true;


    }

    private void Update() {
        
        if (isMoved) {

            state += speed * 0.01F * Time.deltaTime;
            if (state > 1) state = 1;

            Transform transform = GetComponent<Transform>();
            transform.localPosition = Vector3.Lerp(posFrom, posTo, state);
            transform.localRotation = Quaternion.Lerp(rotationFrom, rotationTo, state);

            if (state == 1) {
                isMoved = false;
                onEndMove?.Invoke();
            }
        }

    }
}
