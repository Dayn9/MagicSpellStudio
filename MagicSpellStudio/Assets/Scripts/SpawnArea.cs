using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public GameObject spawnObject;
    //private float timeBetweenSpawns = 10;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(spawnObject, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
