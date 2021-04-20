using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
     void Start()
    {
        //GetComponent<Rigidbody>().isKinematic=true;
    }
    void OnTriggerEnter2D(Collider2D other) { 
       Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}

