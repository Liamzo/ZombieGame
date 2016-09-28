using UnityEngine;
using System.Collections;

public class PivotScript : MonoBehaviour {

    GameObject player;
    Transform playerTransform;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("MainGuy");
        playerTransform = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
        lookatMouse();
        if ((transform.rotation.eulerAngles.z > 50) && (transform.rotation.eulerAngles.z < 310)) {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 1, playerTransform.position.z - 1);
        } else {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - 1, playerTransform.position.z + 1);
        }
        //transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 1, playerTransform.position.z - 1);
    }

    void lookatMouse() {
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        // This will be the point that the object must look towards to be looking at the mouse.
        // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
        //   then find the point along that ray that meets that distance.  This will be the point
        //   to look at.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(ray, out hitdist)) {
            // Get the point along the ray that hits the calculated distance.
            Vector3 targetPoint = ray.GetPoint(hitdist);

            // Determine the target rotation.  This is the rotation if the transform looks at the target point.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            //transform.rotation = Quaternion.Euler(0, 0, -targetRotation.eulerAngles.y);         
            if ((targetRotation.eulerAngles.y >= 0) && (targetRotation.eulerAngles.y <= 180)) {
                transform.rotation = Quaternion.Euler(0, 0, -targetRotation.eulerAngles.y);
            } else if ((targetRotation.eulerAngles.y > 180) && (targetRotation.eulerAngles.y < 360)) {
                transform.rotation = Quaternion.Euler(0, 180, targetRotation.eulerAngles.y);
            }
        }

    }
}
