using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour
{
    private PlayerCharacter character;
    [SerializeField]
    private float speed;

    public void SetPlayer(PlayerCharacter character)
    {
        this.character = character;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (character)
        {
            transform.position = Vector3.MoveTowards(transform.position, character.transform.position, Time.deltaTime*speed);
        }
    }
}
