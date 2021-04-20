using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WutbuergerAttacke : MonoBehaviour
{
    // Start is called before the first frame update
    public int leben;
    public Rigidbody2D rigidbody;

    void Start()
    {
        leben=100;
    }
    void OnTriggerEnter2D(Collider2D other) { 
        if(Wutbuerger(other.gameObject))
        {
            leben-=10;
            if(leben==0)
            {
                Destroy(gameObject);
            }
            else
            {
                    Vector3 positionPlayer=transform.position;
                    Vector3 positionOther=other.gameObject.transform.position;
                    Vector3 flug=(positionPlayer-positionOther);
                    rigidbody.MovePosition(rigidbody.position+new Vector2(flug.x,flug.y));
            }
        } 
    }
     void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Beruehrt");
    }
    bool Wutbuerger(GameObject other)
    {
        if(!other.CompareTag("Impfbar"))
        {
            return false;
        }
        Impfung impfungDesAnderen=other.GetComponent<Impfung>();
        return impfungDesAnderen.wuetend;
    }
}
