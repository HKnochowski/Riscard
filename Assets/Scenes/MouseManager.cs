using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("Mouse position: " + Input.mousePosition);

        //Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log("World point: " + Input.mousePosition);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject ourHitObject = hitInfo.collider.gameObject;

            Debug.Log("Raycast hit: " + ourHitObject.name);

            if (Input.GetMouseButtonDown(0))
            {
                MeshRenderer mr = ourHitObject.GetComponentInChildren<MeshRenderer>();

                if (mr.material.color == Color.red)
                {
                    mr.material.color = Color.white;
                }
                else
                {
                    mr.material.color = Color.red;
                }
            }
        }
    }
}
