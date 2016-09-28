using UnityEngine;
using System.Collections;

public class PlayerScript : Damagable {

	private CharacterController character;

	// Movement
	public float walkSpeed = 15f;
	public float aimSpeed = 5f;
	public float curSpeed;
	private Vector3 totalMovement;

    //Gun
    public GameObject activeGun;
    public GameObject secondGun = null;

    //Health
    public float maxHealth = 100;
    public float curHealth;
    public float healthRegenTime = 3f;
    public float curRegenTime = 0f;
    public float healthRegenRate = 30f;

    //Barricades
    GameObject[] barricades;
    public float repairDist = 5f;

    //Buying
    public GameObject buy;

    // UI
    GameObject scoreScreen;
    GameObject pausePanel;

    // Use this for initialization
    void Start () {
		character = GetComponent<CharacterController>();
		curSpeed = walkSpeed;
        activeGun = GameObject.FindGameObjectWithTag("ActiveGun");

        curHealth = maxHealth;

        barricades = GameObject.FindGameObjectsWithTag("Barricade");

        scoreScreen = GameObject.Find("ScoreScreen");
        scoreScreen.SetActive(false);

        pausePanel = GameObject.Find("PausePanel");
        pausePanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, 2.52f, transform.position.z);

		// Find the current speed
		if (Input.GetAxis("Fire2") == 1){
			curSpeed = aimSpeed;
		} else {
			curSpeed = walkSpeed;
		}
		// Move
		totalMovement = new Vector3(Input.GetAxis("Horizontal") * curSpeed, 0, Input.GetAxis("Vertical") * curSpeed);
		character.Move(totalMovement * Time.deltaTime);

        // Gun
        if (Input.GetAxis("Fire1") == 1) {
            activeGun.GetComponent<GunScript>().fire();
        }

        if (Input.GetKeyDown("r")) {
            activeGun.GetComponent<GunScript>().reload();
        }

        // Health
        if (curHealth < maxHealth) {
            curRegenTime -= Time.deltaTime;

            if (curRegenTime <= 0) {
                curHealth += healthRegenRate * Time.deltaTime;
                if (curHealth > maxHealth) {
                    curHealth = maxHealth;
                }
            }

        }

        // Repair barricades
        if (Input.GetKey("space")) {
            repairBarricade();
        }

        // Buy gun
        if (Input.GetKeyDown("e")) {
            if (buy != null) {
                BuyScript bs = (BuyScript)buy.GetComponent("BuyScript");
                bs.buyWeapon();
            }
        }

        // Switch gun
        if (Input.GetKeyDown("q")) {
            switchGun();
        }

        // Pause game
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pausePanel.activeSelf == false) {
                pausePanel.SetActive(true);
                Time.timeScale = 0f;
            } else {
                pausePanel.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    void repairBarricade() {
        GameObject repairBarricade = null;
        Vector3 dir;
        float minDist = 10000f;

        foreach (GameObject barricade in barricades) {
            dir = barricade.transform.position - this.transform.position;
            if (dir.magnitude < minDist) {
                minDist = dir.magnitude;
                repairBarricade = barricade;
            }
        }

        if (minDist <= repairDist) {
            BarricadeScript bs = (BarricadeScript)repairBarricade.GetComponent("BarricadeScript");
            bs.healDamage();
        }
    }

    public float getHealth () {
        return this.curHealth;
    }

    public GameObject getActiveGun () {
        return activeGun;
    }

    override
    public void takeDamage(float damage) {
        curHealth -= damage;

        curRegenTime = healthRegenTime;
        
        if (curHealth <= 0) {
            scoreScreen.SetActive(true);
            scoreScreen.GetComponent<EndGameScript>().showScore();
            Destroy(gameObject);
        } 
    }

	void lookatMouse() {
		Plane playerPlane = new Plane(Vector3.up, transform.position);
		
		// Generate a ray from the cursor position
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		// Determine the point where the cursor ray intersects the plane.
		// This will be the point that the object must look towards to be looking at the mouse.
		// Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
		//   then find the point along that ray that meets that distance.  This will be the point
		//   to look at.
		float hitdist = 0.0f;
		// If the ray is parallel to the plane, Raycast will return false.
		if (playerPlane.Raycast (ray, out hitdist)) 
		{
			// Get the point along the ray that hits the calculated distance.
			Vector3 targetPoint = ray.GetPoint(hitdist);
			
			// Determine the target rotation.  This is the rotation if the transform looks at the target point.
			Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

			transform.rotation = targetRotation;
		}
	}

    public void setActiveGun (GameObject weapon) {
        //Destroy(activeGun);
        //activeGun = weapon;
        secondGun.SetActive(false);
        activeGun = (GameObject)Instantiate(weapon);
        activeGun.name = weapon.name;

        GameObject pivot = GameObject.FindGameObjectWithTag("Pivot");
        activeGun.transform.parent = pivot.transform;
        activeGun.transform.position = pivot.transform.position;
        activeGun.transform.localRotation = Quaternion.identity;
    }

    void switchGun () {
        if (secondGun == null) {
            return;
        }
        GameObject holdGun = activeGun;
        activeGun = secondGun;
        secondGun = holdGun;
        activeGun.SetActive(true);
        secondGun.SetActive(false);
    }

    void OnTriggerEnter (Collider coll) {
        if (coll.gameObject.tag == "Buy") {
            buy = coll.gameObject;
        }
    }

    void OnTriggerExit (Collider coll) {
        if (coll.gameObject.tag == "Buy") {
            buy = null;
        }
    }
}
