using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuyScript : MonoBehaviour {

    public int price;
    public int ammo;
    public GameObject weapon;
    public GameObject infoText;

    void OnTriggerEnter (Collider coll) {
        infoText.GetComponent<Text>().text = "Press E to buy - " + price.ToString();
    }

    void OnTriggerExit (Collider coll) {
        infoText.GetComponent<Text>().text = "";
    }

    public void buyWeapon () {
        UIScript ui = (UIScript)GameObject.FindGameObjectWithTag("Manager").GetComponent("UIScript");
        PlayerScript ps = (PlayerScript)GameObject.FindGameObjectWithTag("MainGuy").GetComponent("PlayerScript");

        if (price <= ui.getScore()) {
            ui.subScore(price);
            Debug.Log(ps.getActiveGun().name);
            Debug.Log(weapon.name);
            if (ps.secondGun == null) {
                ps.secondGun = ps.activeGun;
                ps.setActiveGun(weapon);
            } else if (ps.getActiveGun().name == weapon.name) {
                ps.getActiveGun().GetComponent<GunScript>().addAmmo(ammo);
            } else if (ps.secondGun.name == weapon.name) {
                ps.secondGun.GetComponent<GunScript>().addAmmo(ammo);
            } else {
                Destroy(ps.getActiveGun());
                ps.setActiveGun(weapon);
            }
            
        }
    }
}