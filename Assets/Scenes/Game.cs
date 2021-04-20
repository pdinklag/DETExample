using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    /// <summary>
    /// The singleton instance.
    /// </summary>
    public static Game Instance { get; private set; }


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

        
        impfbare=new Impfbar[GameSettings.anzahlImpfbare()];
        zeigerImpfbare=0;
        // load settings
        Settings = GameSettings.Load();
    }

    private void Start()
    {
        //???
    }

    private void FixedUpdate(){

        foreach (Impfbar i1 in impfbare) {
            if(i1.infiziert)
            {
                foreach(Impfbar i2 in impfbare)
                {
                    i2.moeglicheInfektion(i1);
                }
            }
        }
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