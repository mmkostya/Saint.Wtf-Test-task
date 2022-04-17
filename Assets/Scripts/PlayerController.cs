using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    [SerializeField] private Joystick joystick = null;
    
    [SerializeField] private float speed = 10f;

    private Rigidbody rbPlayer = null;

    private void Awake() {
        rbPlayer = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        rbPlayer.velocity = new Vector3(joystick.Horizontal * speed, rbPlayer.velocity.y, joystick.Vertical * speed);
    }

}
