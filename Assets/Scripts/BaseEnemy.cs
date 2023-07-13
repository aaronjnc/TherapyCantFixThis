using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private PlayerCharacter character;
    // Start is called before the first frame update
    void Start()
    {
        character = FindObjectOfType<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
