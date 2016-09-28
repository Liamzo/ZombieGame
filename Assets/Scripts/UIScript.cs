using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    GameObject player;
    PlayerScript ps;

    GameObject ammoPanel;
    GameObject activeGun;
    GunScript gs;
    GameObject wavePanel;

    GameObject scorePanel;

    int curScore = 0;
    int totalScore = 0;

    int zomKilled = 0;

    int wave = 0;

    

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("MainGuy");
        ps = (PlayerScript) player.GetComponent("PlayerScript");

        scorePanel = GameObject.Find("Score");

        ammoPanel = GameObject.Find("Ammo");
        activeGun = ps.getActiveGun();
        gs = (GunScript) activeGun.GetComponent<GunScript>();

        wavePanel = GameObject.Find("Wave");
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Image>().CrossFadeAlpha((255f - ((255f / 100f) * ps.getHealth())), 0.1f, false);

        activeGun = ps.getActiveGun();
        gs = (GunScript)activeGun.GetComponent<GunScript>();

        scorePanel.GetComponent<Text>().text = curScore.ToString();
        ammoPanel.GetComponent<Text>().text = gs.getClip().ToString() + " / " + gs.getReserve().ToString();
        wavePanel.GetComponent<Text>().text = wave.ToString();
    }

    public void addScore (int i) {
        curScore += i;
        totalScore += i;
    }

    public void subScore (int i) {
        curScore -= i;
    }

    public int getScore () {
        return curScore;
    }

    public void killedZom () {
        zomKilled += 1;
    }

    public int getWave () {
        return wave;
    }

    public void nextWave () {
        wave += 1;
    }

    public int getKilledZom () {
        return zomKilled;
    }

    public int getTotalScore () {
        return totalScore;
    }

}
