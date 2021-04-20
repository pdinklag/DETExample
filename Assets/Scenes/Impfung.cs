using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impfung : MonoBehaviour
{
    public Renderer renderer;
    public bool Geimpft = false;
    
    public bool Impfgegner=false;

    public bool wuetend=false;
    // Start is called before the first frame update
    void Start()
    {
       if(!Impfgegner)
        {
            renderer.material.SetColor("_Color", Color.cyan);
        }
        else
        {
            renderer.material.SetColor("_Color", Color.black);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other) { 
        if(other.CompareTag("Bullet"))
        {
            bullet bullet=other.GetComponent<bullet>();
            if(bullet.typ==0)
            {
                if(Impfgegner)
                {
                    renderer.material.SetColor("_Color", Color.red);
                    wuetend=true;
                }
                else
                {
                    renderer.material.SetColor("_Color", Color.green);
                    Geimpft=true;
                }
            }
            else
            {
                ruhigStellen(100);   
            }
        } 
    }
    public void ruhigStellen(int dauer)
    {
        RandomMovement randomMovement=gameObject.GetComponent<RandomMovement>();
        randomMovement.ruhigStellen(dauer);
    }
}
