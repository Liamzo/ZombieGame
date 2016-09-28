using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

    public GameObject targetBarride;
    public GameObject zombiePrefab;

    public void spawnZombie () {
        GameObject zombie = (GameObject)Instantiate(zombiePrefab, transform.position, transform.rotation);
        EnemyScript es = (EnemyScript)zombie.GetComponent("EnemyScript");
        es.setTarget(targetBarride);
    }

}
