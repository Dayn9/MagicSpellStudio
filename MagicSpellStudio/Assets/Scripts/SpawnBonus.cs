using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBonus : MonoBehaviour
{
    public GameObject spawnObject;
    public Transform[] possibleLocations;

    public int spawnTime;
    private float timer;

    public AudioSource potionSpawn;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < spawnTime)
        {
            timer += Time.deltaTime;
            if (timer > spawnTime)
            {
                Instantiate(spawnObject, possibleLocations[Random.Range(0, possibleLocations.Length)]);
                potionSpawn.Play();
            }
        }
    }
}
