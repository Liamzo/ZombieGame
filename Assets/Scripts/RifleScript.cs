using UnityEngine;
using System.Collections;

public class RifleScript : GunScript {

    float spread = 0;
    float maxSpread = 8;
    float thisSpread = 0;

    float spreadCD = 0.3f;
    float spreadTimer;

    // Use this for initialization
    void Start()
    {
        baseDamage = 20;
        damageFallOff = 60;
        clipSize = 30;
        clipRemaining = clipSize;
        reserveAmmo = 120;
        reloadSpeed = 2;
        reloadTime = reloadSpeed;
        fireRate = 0.2;
        fireTime = 0;
        state = IsReloading.WAITING;
        bulletLife = 10f;
        bulletSpeed = 4000f;

        spreadTimer = spreadCD;
    }

    new void Update () {
        base.Update();

        spreadTimer -= Time.deltaTime;

        if (spreadTimer <= 0) {
            if (spread > 0) {
                spread -= Time.deltaTime * 4;
            } else {
                spread = 0;
            }
        } 
    }

    override
    public void fire () {
        if ((clipRemaining <= 0) && (fireTime <= 0)) {
            reload();
            return;
        } else if ((fireTime <= 0) && (state != IsReloading.RELOADING)) {
            GameObject bulletGO;
            clipRemaining--;
            fireTime = fireRate;

            thisSpread = Random.Range(-spread, spread);
            if (spread < maxSpread) {
                spread += 1;
            }

            spreadTimer = spreadCD;

            // Actuall fire code
            if (gunSide == GunSide.RIGHT) {
                bulletGO = (GameObject)Instantiate(bullet, transform.position, Quaternion.Euler(0, -transform.eulerAngles.z + thisSpread, 0));
            } else {
                bulletGO = (GameObject)Instantiate(bullet, transform.position, Quaternion.Euler(0, transform.eulerAngles.z + thisSpread, 0));
            }
            //GameObject bulletGO = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            BulletScript bulletScript = (BulletScript)bulletGO.GetComponent("BulletScript");
            bulletScript.bulletLife = bulletLife;
            bulletScript.bulletSpeed = bulletSpeed;
            bulletScript.baseDamage = baseDamage;
            bulletScript.damageFallOff = damageFallOff;
        }
    }
}
