using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schlagstock : MonoBehaviour
{

    private Vector2 moveDirection;

    public Rigidbody2D rigidbody;

    public Rigidbody2D userRigidbody;

    public float speed;//a bit higher than user speed 
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
    void setMovementToUser()
    {
         Vector3 positionBesitzer=userRigidbody.transform.position;
        Vector3 position=gameObject.transform.position;
        moveDirection.x=positionBesitzer.x-position.x+0.5f;
        moveDirection.y=positionBesitzer.y-position.y;
        moveDirection=moveDirection.normalized;
    }
    void Move() {
        setMovementToUser();
        rigidbody.velocity = moveDirection*speed;
    }
}
