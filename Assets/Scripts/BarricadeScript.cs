using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarricadeScript : Damagable {

    int maxHealth;
    public int curHealth;
    Transform[] bars;

    // For repairing
    float repairCD = 1f;
    float curRepairCD;

    public GameObject infoText;

    void OnTriggerEnter (Collider coll) {
        if ((curHealth < maxHealth) && (coll.tag == "MainGuy")) {
            infoText.GetComponent<Text>().text = "Press SPACE to repair";
        }
    }

    void OnTriggerExit (Collider coll) {
        infoText.GetComponent<Text>().text = "";
    }

    // Use this for initialization
    void Start () {
        maxHealth = 5;
        curHealth = maxHealth;

        bars = gameObject.GetComponentsInChildren<Transform>();

        curRepairCD = repairCD;
	}
	
	// Update is called once per frame
	void Update () {
        curRepairCD -= Time.deltaTime;
	}

    public int getCurHealth() {
        return curHealth;
    }

    override
    public void takeDamage (float damage) {
        bars[curHealth].GetComponent<MeshRenderer>().enabled = false;
        curHealth--;
    }

    public void healDamage () {
        if (curHealth < maxHealth && curRepairCD <= 0) {
            curHealth++;
            bars[curHealth].GetComponent<MeshRenderer>().enabled = true;
            curRepairCD = repairCD;

            UIScript ui = (UIScript)GameObject.FindGameObjectWithTag("Manager").GetComponent("UIScript");
            ui.addScore(10);
        }
    }
}
