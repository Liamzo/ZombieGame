using UnityEngine;
using System.Collections;

public class SniperBulletScript : BulletScript {

    int lives = 2;

    void OnTriggerEnter (Collider coll) {
        if (coll.gameObject.tag == "Enemy") {
            EnemyScript enemyscipt = (EnemyScript)coll.gameObject.GetComponent("EnemyScript");
            enemyscipt.takeDamage(baseDamage);

            lives--;
            if (lives <= 0) {
                Destroy(this.gameObject);
            }
        }
        if (coll.gameObject.tag == "Wall") {
            Destroy(this.gameObject);
        }
    }
}
