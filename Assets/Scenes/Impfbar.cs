using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Impfbar : MonoBehaviour
{
    public Renderer renderer;

    public static GameObject SchlagstockPrefab;

    public static GameObject virusPrefab;

    public float virusForce = 10f;

    public Rigidbody2D rigidbody;
    public int distanceInfizierung=50;
    public bool geimpft = false;

    public bool politiker=false;  
    public bool Impfgegner=false;

    public bool infiziert=false;
    public bool wuetend=false;

    public int koBulletsFromPolitics=10;

    public int timeBetweenHusten=5;

    private int timeToNextHusten=0;
    
    private static Color colorImpfgegner=new Color(1f, 0.92f, 0.016f, 1f);

    private static Color colorInfiziert=Color.red;

    private static Color colorWuetend=new Color(1f, 0f, 1f, 1f);

    private Color colorGeheilt=Color.blue;

    private Color colorNormal=new Color(0f,1f,0f,1f);

    private static Player player;

    private static System.Random  random;
    // Start is called before the first frame update
    void Start()
    {
        if(virusPrefab==null)
        {
            virusPrefab = AssetDatabase.LoadAssetAtPath("Assets/Corona.prefab", typeof(GameObject)) as GameObject;
            if(virusPrefab==null)
            {
                Debug.LogError("Virus Prefab nicht geladen");
                Destroy(gameObject);
                return;
            }
        }
        if(SchlagstockPrefab==null)
        {
            SchlagstockPrefab = AssetDatabase.LoadAssetAtPath("Assets/Schlagstock.prefab", typeof(GameObject)) as GameObject;
            if(SchlagstockPrefab==null)
            {
                Debug.LogError("Schlagstock Prefab nicht geladen");
                Destroy(gameObject);
                return;
            }
        }
        if(player==null)
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            player=playerObject.GetComponent<Player>();
            if(player==null)
            {
                Debug.LogError("No Player");
                if(playerObject==null)
                {
                    Debug.Log("Also Player not found by Tag");
                }
                Destroy(gameObject);
                return;
            }
        }
        if(rigidbody==null)
        {
            Debug.LogError("No rigidbody");
            Destroy(gameObject);
            return;
        }
        if(random==null)
        {
            random=new System.Random();
        }
        if(random.Next(10)<=1)
        {
            Impfgegner=true;
            addSchlagstock();
        }
        if(random.Next(10)<=1)
        {
            infiziert=true;
        }
        if(random.Next(10)<=1)
        {
            politiker=true;
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
        if(infiziert)
        {
            hustenOptional();
        }
    }
    void OnTriggerEnter2D(Collider2D other) { 
        if(other.CompareTag("Bullet")||other.CompareTag("WaffeGegner"))
        {
            bullet bullet=other.GetComponent<bullet>();
            if(bullet==null)
            {
                return;
            }
            if(bullet.typ==0)
            {
                if(!geimpft)
                {
                    geimpft=true;
                    infiziert=false;
                    if(Impfgegner)
                    {
                        wuetend=true;
                    }
                    else if(politiker)
                    {
                        player.addKOBullets(koBulletsFromPolitics);
                    }
                    setColor();
                }
            }
            else if(bullet.typ==1)
            {
                ruhigStellen(100);   
            }
            else
            {
                if(!infiziert)
                {
                    infizieren();
                }
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
        //Debug.Log(abstand);
        if(abstand<=distanceInfizierung)
        {
            
            return true;
        }
        return false;
    }
    public void infizieren()
    {
        if(!geimpft)
        {
            infiziert=true;
            setColor();
        }
        
    }
    private void addSchlagstock()
    {
        Vector3 positionSpawn=gameObject.transform.position;
        positionSpawn.x-=0.6f;
         GameObject schlagstock = Instantiate(SchlagstockPrefab,positionSpawn , gameObject.transform.rotation);
        if(schlagstock==null)
        {
            Debug.Log("No schlagstock");
        }
         Schlagstock script=schlagstock.GetComponent<Schlagstock>();
         if(script==null)
         {
             Debug.Log("No script");
         }
         script.userRigidbody=rigidbody;
         RandomMovement randomMovement=gameObject.GetComponent<RandomMovement>();
         script.speed=randomMovement.moveSpeed*1.2f;
    }
    void hustenOptional()
    {
        if(timeToNextHusten>0)
        {
            timeToNextHusten--;
        }
        else
        {
            husten();
            timeToNextHusten=timeBetweenHusten;
        }
    }
    void husten()
    {
        if(virusPrefab==null)
        {
            Debug.Log("No Virus Prefab");
        }
        else
        {
            Vector3 personVelocity=new Vector3(rigidbody.velocity.x,rigidbody.velocity.y,0);
           if(politiker)
           {
               personVelocity=new Vector3(1,1,0);
           }
            shootThreeVirusWithAngle(personVelocity);
        }
    }
    private void shootVirus(Vector3 positionSpawn, Vector3 force3)
    {
        //Debug.Log("Position: "+positionSpawn+" Force: "+force3);
        //Debug.Log("Position Normalo: "+gameObject.transform.position+" Normalo Velocity: "+rigidbody.velocity+" Position Virus: "+positionSpawn+" Force: "+force);
        Vector2 force=new Vector2(force3.x,force3.y);
        GameObject virus = Instantiate(virusPrefab, positionSpawn, gameObject.transform.rotation);
        Rigidbody2D rb = virus.GetComponent<Rigidbody2D>();
        rb.AddForce(force * virusForce, ForceMode2D.Impulse);
    }
    private void shootThreeVirusWithAngle(Vector3 personVelocity)
    {
        float distanceSpawnPoint=0.5f;
        Vector3 a=personVelocity;//vector straight shoot
        if(a.x==0)
        {
            a.x=0.01f;//otherwise dividing throw zero
        }
        float radian=(Mathf.PI/180)*30;//<90!
        Vector3 b1=new Vector3(0,0,0);//vector angle° right
        Vector3 b2=new Vector3(0,0,0);//vector angle° left
        float p=((-2)*a.sqrMagnitude*Mathf.Cos(radian)*a.y)/(a.y*a.y+a.x*a.x);
        float q=(a.sqrMagnitude*a.sqrMagnitude*Mathf.Cos(radian)*Mathf.Cos(radian)-a.sqrMagnitude*a.x*a.x)/(a.y*a.y+a.x*a.x);
        b1.y=-(p/2)+Mathf.Sqrt(Mathf.Abs((p/2)*(p/2)-q));
        b1.x=calculateB1(a,radian,b1.y);
        b2.y=-(p/2)-Mathf.Sqrt(Mathf.Abs((p/2)*(p/2)-q));
        b2.x=calculateB1(a,radian,b2.y);
        Vector3 positionSpawn=gameObject.transform.position+personVelocity*distanceSpawnPoint;
        shootVirus(positionSpawn,personVelocity);
        positionSpawn=gameObject.transform.position+b1*distanceSpawnPoint;
        shootVirus(positionSpawn,b1);
        positionSpawn=gameObject.transform.position+b2*distanceSpawnPoint;
        shootVirus(positionSpawn,b2);
    }
    private float calculateB1(Vector2 a, float radian, float b2)
    {
        return (a.sqrMagnitude*Mathf.Cos(radian)-a.y*b2)/a.x;
    }
}
