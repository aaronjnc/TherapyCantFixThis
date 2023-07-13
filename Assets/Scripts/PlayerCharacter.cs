using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter : MonoBehaviour
{
    InputController controller;
    Rigidbody2D rigidbody;
    [SerializeField]
    private float speed;
    
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        controller = new InputController();
        controller.PlayerCore.Movement.performed += Move;
        controller.PlayerCore.Movement.Enable();
    }

    void Move(CallbackContext ctx)
    {
        rigidbody.AddForce(ctx.ReadValue<Vector2>() * speed);
    }
}
