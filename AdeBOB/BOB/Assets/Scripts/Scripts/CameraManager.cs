using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraManager : MonoBehaviour {

    public Button zoomInBtn;
    public Button zoomOutBtn;

    public float zoomStrength = 10;

	// Use this for initialization
	void Start () {

        zoomInBtn.onClick.AddListener(ZoomIn);
        zoomOutBtn.onClick.AddListener(ZoomOut);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ZoomIn()
    {
        transform.position += Vector3.up * zoomStrength;
    }

    void ZoomOut()
    {
        transform.position += Vector3.down * zoomStrength;
    }
}
