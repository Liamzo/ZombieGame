using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float bulletLife;
    public float bulletSpeed;

    // For damage calulations, not sure if I'll use damage fall off
    public float baseDamage;
    public float damageFallOff;
    //Transform startLocation;

    protected virtual void Start() {
        //transform.rotation = Quaternion.Euler(0, -transform.rotation.eulerAngles.z, 0);
        Rigidbody rb = GetComponent < Rigidbody > ();
        rb.AddForce(transform.forward * bulletSpeed);

        //startLocation = this.transform;

    }

   protected virtual void Update() {
        bulletLife -= Time.deltaTime;

        if (bulletLife <= 0) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Enemy") {
            EnemyScript enemyscipt = (EnemyScript)coll.gameObject.GetComponent("EnemyScript");
            enemyscipt.takeDamage(baseDamage);

            Destroy(this.gameObject);
        }
        if (coll.gameObject.tag == "Wall") {
            Destroy(this.gameObject);
        }
    }
}
