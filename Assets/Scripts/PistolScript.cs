using UnityEngine;
using System.Collections;

public class PistolScript : GunScript {

	// Use this for initialization
	void Start () {
        baseDamage = 30;
        damageFallOff = 60;
        clipSize = 12;
        clipRemaining = clipSize;
        reserveAmmo = 120;
        reloadSpeed = 2;
        reloadTime = reloadSpeed;
        fireRate = 0.5;
        fireTime = 0;
        state = IsReloading.WAITING;
        bulletLife = 10f;
        bulletSpeed = 4000f;
    }

    new void Update () {
        base.Update();
    }
}
