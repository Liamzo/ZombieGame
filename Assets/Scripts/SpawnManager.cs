using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public GameObject[] spawns;

    // For spawning
    float spawnTimer = 1f;
    public float curSpawnTImer;

    //
    public int zomAlive;
    public int zomToSpawn;

	// Use this for initialization
	void Start () {
        spawns = GameObject.FindGameObjectsWithTag("Spawn");

        curSpawnTImer = spawnTimer;
	}
	
	// Update is called once per frame
	void Update () {
        curSpawnTImer -= Time.deltaTime;

        if (zomAlive <= 0) {
            newWave();
        } else if ((curSpawnTImer <= 0) && (zomToSpawn > 0)) {
            SpawnScript ss = (SpawnScript)spawns[(int)Random.Range(0, spawns.Length)].GetComponent("SpawnScript");
            ss.spawnZombie();
            curSpawnTImer = spawnTimer;
            zomToSpawn -= 1;
        }
	}

    void newWave () {
        UIScript ui = (UIScript)GameObject.FindGameObjectWithTag("Manager").GetComponent("UIScript");
        ui.nextWave();       
        zomToSpawn = (int)Mathf.Pow(ui.getWave(), 2) + 5;
        zomAlive = zomToSpawn;
    }

    public void killedZom () {
        zomAlive -= 1;
    }
}
