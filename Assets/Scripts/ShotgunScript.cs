using UnityEngine;
using System.Collections;

public class ShotgunScript : GunScript {

    int pellets;

    float spread = 5f;
    float thisSpread = 0;

    // Use this for initialization
    void Start () {
        baseDamage = 12;
        damageFallOff = 60;
        clipSize = 6;
        clipRemaining = clipSize;
        reserveAmmo = 60;
        reloadSpeed = 0.7f;
        reloadTime = reloadSpeed;
        fireRate = 1f;
        fireTime = 0;
        state = IsReloading.WAITING;
        bulletLife = 0.25f;
        bulletSpeed = 4000f;

        pellets = 10;
    }

    override
    public void reload () {
        // If able to reload, update the state of the gun
        if ((reserveAmmo > 0) && (clipRemaining < clipSize)) {
            state = IsReloading.RELOADING;
        }

        // If the reload has been completed, fill the ammo
        if (reloadTime <= 0) {
            // If resever ammo is anough for a full reload
            if (reserveAmmo >= 1) {
                clipRemaining += 1;
                reserveAmmo -= 1;
            }

            // Reset timer and state
            reloadTime = reloadSpeed;

            if (clipRemaining >= clipSize) {
                state = IsReloading.WAITING;
            }
        }
    }

    override
    public void fire () {
        if ((clipRemaining <= 0) && (fireTime <= 0)) {
            reload();
            return;
        } else if ((fireTime <= 0) && (clipRemaining > 0)) {
            state = IsReloading.WAITING;
            reloadTime = reloadSpeed;

            GameObject bulletGO;
            clipRemaining--;
            fireTime = fireRate;

            for (int i = 0; i < pellets; i++) {
                thisSpread = Random.Range(-spread, spread);

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
}
