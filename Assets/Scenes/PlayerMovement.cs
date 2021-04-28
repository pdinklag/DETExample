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
    public int leben;
    // Update is called once per frame
    void Start(){
        leben=100;
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

        if (Input.GetKeyDown("space")) {
            Vector2 playerPos = gameObject.transform.position;

            string tagPlayer;
            Labyrinth.Instance.GetTagFromPos(playerPos, out tagPlayer);

            string tagNext;
            Labyrinth.Instance.GetRandomCellAtMaxDistance(tagPlayer, 4, out tagNext);

            Debug.Log(tagNext);
            
            int x, y;
            Labyrinth.Instance.GetIntsFromTag(tagNext, out x, out y);

            Vector2 nextPos;
            Labyrinth.Instance.GetPosFromCell(x, y, out nextPos);

            gameObject.transform.position = nextPos;
        }
    }

    void Move() {
        rigidbody.velocity = new Vector2(moveDirection.x, moveDirection.y).normalized * moveSpeed;

        Vector2 lookDir = mousePos - rigidbody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rigidbody.rotation = angle;
    }
    void OnTriggerEnter2D(Collider2D other) { 
        if(Corona(other.gameObject))
        {
            takeDemage(5);
        }
        if(Schlagstock(other.gameObject))
        {
            
            if(!takeDemage(10))
            {
                Vector3 positionPlayer=transform.position;
                Vector3 positionOther= other.gameObject.transform.position;
                Vector3 flug=(positionPlayer-positionOther);
                rigidbody.MovePosition(rigidbody.position+new Vector2(flug.x,flug.y));
            }
        } 
    }
    /*
    returns whether the player dies or not
    */
    private bool takeDemage(int demage)
    {
        leben-=10;
        if(leben<=0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    void OnCollisionEnter2D(Collision2D collision) {
         
    }
    bool Schlagstock(GameObject other)
    {
        if(!other.CompareTag("WaffeGegner"))
        {
            return false;
        }
        Schlagstock schlagstock=other.GetComponent<Schlagstock>();
        return schlagstock!=null;
    }
    bool Corona(GameObject other)
    {
        if(!other.CompareTag("WaffeGegner"))
        {
            return false;
        }
        bullet bullet=other.GetComponent<bullet>();
        if(bullet==null)
        {
            return false;
        }
        return bullet.typ==2;
    }
}