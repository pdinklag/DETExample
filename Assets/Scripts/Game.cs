using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Game : MonoBehaviour
{
    /// <summary>
    /// The singleton instance.
    /// </summary>
    public static Game Instance { get; private set; }
    private GameObject normaloPrefab;


    /// <summary>
    /// The game settings.
    /// </summary>
    public GameSettings Settings { get; private set; }

    [Tooltip("Where to spawn the player.")]
    public Transform SpawnPoint;

    ///allNormalos
    public Impfbar[] impfbare;

    ///zeigt auf erstes freies Feld vom Array normalos
    private int zeigerImpfbare;
    private void Awake()
    {
        // there can be only one...
        if (Instance)
        {
            Debug.LogError("only one Level instance allowed");
            Destroy(gameObject); // exercise: what would be different if we used Destroy(this) instead?
            return;
        }
        else
        {
            Instance = this;
            Debug.Log("registered Level instance", Instance);
        }

        
        // load settings
        Settings = GameSettings.Load();
        impfbare=new Impfbar[Settings.getNumberOfImpfbare()];
        zeigerImpfbare=0;
        
        Labyrinth labyrinth = ScriptableObject.CreateInstance<Labyrinth>();
        labyrinth.SetSettings(Settings);
        labyrinth.Generate();

        System.Random rand = new System.Random();

        normaloPrefab = AssetDatabase.LoadAssetAtPath("Assets/Scenes/Normalo.prefab", typeof(GameObject)) as GameObject;

        for (int i = 0; i<Settings.size; i++) {
            for (int j = 0; j<Settings.size; j++) {
                if (rand.NextDouble() <= Settings.personDensity) {
                    GameObject normalo = Instantiate(normaloPrefab, labyrinth.GetPosOfCell(i, j), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
                    if (rand.NextDouble() <= Settings.probInfected) {
                        normalo.GetComponent<Impfbar>().infiziert = true;
                    }
                }
            }
        }
        
    }

    private void Start()
    {
        //???
    }

    private void FixedUpdate() {

    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Debug.Log("unregistered Level instance", Instance);
        }
    }

    public void addImpfbar(Impfbar impfbar)
    {
        impfbare[zeigerImpfbare]=impfbar;
        zeigerImpfbare++;
    } 
}