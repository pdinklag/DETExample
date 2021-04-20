using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rigidbody;
    public Camera cam;
    private Vector2 moveDirection;
    private Vector2 mousePos;

    // Update is called once per frame
    void Start(){
    }
    void Update() {
        processInputs();
    }

    void FixedUpdate() {
        MoveCamera();
        Move();
    }

    void MoveCamera() {
        Vector2 relPos = 0.04f * (gameObject.transform.position - cam.transform.position);
        cam.transform.position += (Vector3) relPos;
    }

    void processInputs() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY);

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void Move() {
        rigidbody.velocity = new Vector2(moveDirection.x, moveDirection.y).normalized * moveSpeed;

        Vector2 lookDir = mousePos - rigidbody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rigidbody.rotation = angle;
    }
}