using UnityEngine;
using System.Collections;

abstract public class GunScript : MonoBehaviour {

    // For damage calulations, not sure if I'll use damage fall off
    public float baseDamage;
    public float damageFallOff;
    // For ammo and reloading
    public int clipSize;
    public int clipRemaining;
    public int reserveAmmo;
    public double reloadSpeed;
    public double reloadTime;
    public enum IsReloading {
        RELOADING,
        WAITING
    }
    public IsReloading state;
    // For firing the gun
    public double fireRate;
    public double fireTime;
    // For the bullet
    public GameObject bullet;
    public float bulletLife;
    public float bulletSpeed;

    // For aiming
    public enum GunSide {
        LEFT,
        RIGHT
    }
    public GunSide gunSide;

    protected virtual void Update () {
        if (fireTime > 0) {
            fireTime -= Time.deltaTime;
        }

        if ((state == IsReloading.RELOADING) && (reloadTime > 0)) {
            reloadTime -= Time.deltaTime;

            if (reloadTime <= 0) {
                reload();
            }
        }
        if ((transform.eulerAngles.y >= 0) && (transform.eulerAngles.y <= 180)) {
            gunSide = GunSide.RIGHT;
        } else if ((transform.eulerAngles.y > 180) && (transform.eulerAngles.y < 350)) {
            gunSide = GunSide.LEFT;
        }

    }

    // Completed for now, don't think I'll need to update until weapon switching is a thing
    public virtual void reload() {
        // If able to reload, update the state of the gun
        if ((reserveAmmo > 0) && (clipRemaining < clipSize)) {
            state = IsReloading.RELOADING;
        }

        // If the reload has been completed, fill the ammo
        if (reloadTime <= 0) {
            // If resever ammo is anough for a full reload
            if (reserveAmmo >= clipSize) {
                clipRemaining = clipSize;
                reserveAmmo -= clipSize;
            // Not enough for a full reload
            } else if (reserveAmmo < clipSize) {
                clipRemaining = reserveAmmo;
                reserveAmmo = 0;
            }

            // Reset timer and state
            reloadTime = reloadSpeed;
            state = IsReloading.WAITING;
        }
    }

    public virtual void fire() {
        if ((clipRemaining <= 0) && (fireTime <= 0)) {
            reload();
            return;
        } else if ((fireTime <= 0) && (state != IsReloading.RELOADING)) {
            GameObject bulletGO;
            clipRemaining--;
            fireTime = fireRate;

            // Actuall fire code
            if (gunSide == GunSide.RIGHT) {
                bulletGO = (GameObject)Instantiate(bullet, transform.position, Quaternion.Euler(0, -transform.eulerAngles.z, 0));
            } else {
                bulletGO = (GameObject)Instantiate(bullet, transform.position, Quaternion.Euler(0, transform.eulerAngles.z, 0));
            }
            //GameObject bulletGO = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            BulletScript bulletScript = (BulletScript)bulletGO.GetComponent("BulletScript");
            bulletScript.bulletLife = bulletLife;
            bulletScript.bulletSpeed = bulletSpeed;
            bulletScript.baseDamage = baseDamage;
            bulletScript.damageFallOff = damageFallOff;
        }
    }

    public int getClip () {
        return clipRemaining;
    }

    public int getReserve () {
        return reserveAmmo;
    }

    public void addAmmo (int i) {
        reserveAmmo += i;
    }

}
