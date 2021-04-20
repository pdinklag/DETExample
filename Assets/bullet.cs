using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) { 
       
    }
    void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}

