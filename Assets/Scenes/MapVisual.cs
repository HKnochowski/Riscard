using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisual : MonoBehaviour {


    public GameObject hexPrefab;

    int width = 4;
    int height = 8;

    float XOffset = 3.1f;
    float YOffset = 0.9f;

	// Use this for initialization
	void Start () {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = x * XOffset;

                if (y%2 == 1)
                {
                    xPos += XOffset/2f;
                }

                GameObject hex_go = (GameObject)Instantiate(hexPrefab, new Vector3(xPos, y * YOffset, 0), Quaternion.identity);

                hex_go.name = "Hex_" + x + "_" + y;

                hex_go.transform.SetParent(this.transform, false);
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
