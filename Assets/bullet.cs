using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) { 
        
        if(other.gameObject.CompareTag("Gegner"))
        {
            Destroy(other.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}

