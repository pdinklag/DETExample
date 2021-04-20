using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impfbar : MonoBehaviour
{
    public Renderer renderer;

    public int distanceInfizierung=50;
    public bool geimpft = false;
    
    public bool Impfgegner=false;

    public bool infiziert=false;
    public bool wuetend=false;

    private static Color colorImpfgegner=new Color(1f, 0.92f, 0.016f, 1f);

    private static Color colorInfiziert=Color.red;

    private static Color colorWuetend=new Color(1f, 0f, 1f, 1f);

    private Color colorGeheilt=Color.blue;

    private Color colorNormal=new Color(0f,1f,0f,1f);
    // Start is called before the first frame update
    void Start()
    {
         System.Random random=new System.Random();
        if(random.Next(10)<=1)
        {
            Impfgegner=true;
        }
        if(random.Next(10)<=1)
        {
            infiziert=true;
        }
       setColor();
    }
    private void setColor()
    {
        Color nextColor;
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
        else if(geimpft)
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
                geimpft=true;
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

    public void moeglicheInfektion(Impfbar infizierter)
    {
        if(!infiziert&&!geimpft&&inInfektionsDistanz(infizierter))
        {
            infizieren();
        }
    }
    public bool inInfektionsDistanz(Impfbar infizierterAnderer)
    {
        Vector3 positionOther=infizierterAnderer.gameObject.transform.position;
        Vector3 position=gameObject.transform.position;
        Vector3 abstandsvector=positionOther-position;
        int abstand=(int)abstandsvector.magnitude;
        Debug.Log(abstand);
        if(abstand<=distanceInfizierung)
        {
            
            return true;
        }
        return false;
    }
    public void infizieren()
    {
        infiziert=true;
        setColor();
    }
}
