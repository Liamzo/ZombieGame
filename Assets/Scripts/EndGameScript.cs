using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour {

    GameObject zombiesText;
    GameObject pointsText;

	// Use this for initialization
	void Start () {
        zombiesText = GameObject.Find("ZombiesText");
        pointsText = GameObject.Find("PointsText");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("return")) {
            SceneManager.LoadScene(0);
        }
        
    }

    public void showScore () {
        UIScript ui = (UIScript)GameObject.FindGameObjectWithTag("Manager").GetComponent("UIScript");
        zombiesText.GetComponent<Text>().text += ui.getKilledZom();
        pointsText.GetComponent<Text>().text += ui.getTotalScore();
    }
}
