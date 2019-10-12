using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public GameObject spawnObject;
    public Player playerType;
    //private float timeBetweenSpawns = 10;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = Instantiate(spawnObject, gameObject.transform);
        Pickup pickup = obj.GetComponent<Pickup>();

        pickup.PlayerType = playerType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
