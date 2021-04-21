using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schlagstock : MonoBehaviour
{

    private Vector2 moveDirection;

    public Rigidbody2D rigidbody;

    public Rigidbody2D userRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        if(userRigidbody==null)
        {
            Debug.Log("No User Rigidbody");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
       Move();
    }
    /*void setMovementToUser()
    {
         Vector3 positionBesitzer=transform.parent.position;
        Vector3 position=gameObject.transform.position;
        moveDirection.x=positionPlayer.x-position.x;
        moveDirection.y=positionPlayer.y-position.y;
    }*/
    void Move() {
        rigidbody.velocity = userRigidbody.velocity;
    }
}
