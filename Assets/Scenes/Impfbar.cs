using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impfbar : MonoBehaviour
{
    public Renderer renderer;

    public int distanceInfizierung=10;
    public bool Geimpft = false;
    
    public bool Impfgegner=false;

    public bool infiziert=false;
    public bool wuetend=false;

    private static Color colorImpfgegner=Color.yellow;

    private static Color colorInfiziert=Color.red;

    private static Color colorWuetend=Color.magenta;

    private Color colorGeheilt=Color.blue;

    private Color colorNormal=Color.green;
    // Start is called before the first frame update
    void Start()
    {
         System.Random random=new System.Random();
        if(random.NextDouble<=0.1)
        {
            Impfgegner=true;
        }
        if(random.NextDouble<=0.1)
        {
            infiziert=true;
        }
       setColor();
    }
    private void setColor()
    {
        Color nextColor=null;
        if(Impfgegner)
        {
            if(wuetend)
            {
                nextColor=colorWuetend;
            }
            else
            {
                nextColor=colorImpfgegner;
            }
        }
        else if(infiziert)
        {
            nextColor=colorInfiziert;
        }
        else if(Geimpft)
        {
            nextColor=colorGeheilt;
        }
        else
        {
            nextColor=colorNormal;
        }
        renderer.material.SetColor("_Color", nextColor);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other) { 
        if(other.CompareTag("Bullet"))
        {
            bullet bullet=other.GetComponent<bullet>();
            if(bullet.typ==0)
            {
                Geimpft=true;
                infiziert=false;
                if(Impfgegner)
                {
                    wuetend=true;
                }
                setColor();
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

    public void moeglicheInfektion(Impfbar infiziert)
    {
        if(!infiziert&&inInfektionsDistanz(infiziert))
        {
            infiziert=true;
            setColor();
        }
    }
    public bool inInfektionsDistanz(Impfbar infizierterAnderer)
    {
        Vector3 positionOther=infizierterAnderer.gameObject.position;
        Vector3 position=gameObject.transform.position;
        Vector3 abstand=positionOther-position;
        if(abstand.magnitude<=distanceInfizierung)
        {
            return true;
        }
        return false;
    }
}
