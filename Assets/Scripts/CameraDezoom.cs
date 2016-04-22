using UnityEngine;
using System.Collections;

public class CameraDezoom : MonoBehaviour
{
    public float zoomSpeed;
    public float zoomPerWisp;
    Vector3 actualPos;
	// Use this for initialization
	void Start ()
    {
        actualPos = transform.localPosition;
       
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, actualPos, zoomSpeed * Time.deltaTime);
	}

    public void CameraZooming(int wispIncr)
    {
        actualPos = actualPos + new Vector3(0, zoomPerWisp, zoomPerWisp / -2) * wispIncr;
    }
}
