// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class EvolManager : MonoBehaviour
{
    public static EvolManager instance;
    public List<GameObject> evolUnits = new List<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
