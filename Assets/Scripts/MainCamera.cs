using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    public GameObject target = null;
    public bool orbitY = false;
    public float rotSpeed = 2f;
    bool enMouvement = false;


    // Use this for initialization
    void Start () {
        
       
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1)) {
            enMouvement = true;
        }
        if (Input.GetMouseButtonUp(1)) {
            enMouvement = false;
        }
        if (enMouvement) {
            if (target != null) {
                float rotX = Input.GetAxis("Mouse X") * rotSpeed * 2.5f;
                transform.LookAt(target.transform);
                if (orbitY) {
                    transform.RotateAround(target.transform.position, Vector3.up, rotX);
                }
            }
        }
       
    }
}
