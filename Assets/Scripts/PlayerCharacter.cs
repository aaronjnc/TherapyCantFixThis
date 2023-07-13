using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter : MonoBehaviour
{
    InputController controller;
    Rigidbody2D rb;
    [SerializeField]
    private float speed;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float dampTime = .15f;
    [SerializeField]
    Camera cam;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = new InputController();
        controller.PlayerCore.Movement.performed += Move;
        controller.PlayerCore.Movement.canceled += StopMove;
        controller.PlayerCore.Movement.Enable();
    }

    void Move(CallbackContext ctx)
    {
        rb.velocity = ctx.ReadValue<Vector2>() * speed;
    }

    void StopMove(CallbackContext ctx)
    {
        rb.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Vector3 point = cam.WorldToViewportPoint(transform.position);
		Vector3 delta = transform.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        destination.z = cam.transform.position.z;
		cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref velocity, dampTime);
    }
}
