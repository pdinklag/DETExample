using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rigidbody;
    private Vector2 moveDirection;

    private int timeToNextMovementChange;
    // Update is called once per frame
    void Start()
    {
        timeToNextMovementChange=0;
    }
    void setRandomMovement()
    {
        System.Random random=new System.Random();
        moveDirection.x=(float)random.NextDouble()*2-1;
        moveDirection.y=(float)random.NextDouble()*2-1;
        timeToNextMovementChange=random.Next(60);
    }
    void Update() {
    }

    void FixedUpdate() {
        if(timeToNextMovementChange==0)
        {
            setRandomMovement();
        }
        else
        {
             timeToNextMovementChange--;
        }
        Move();
    }


    void Move() {
        rigidbody.velocity = new Vector2(moveDirection.x, moveDirection.y).normalized * moveSpeed;
    }
}
