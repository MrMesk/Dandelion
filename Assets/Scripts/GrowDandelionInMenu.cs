using UnityEngine;
using System.Collections;

public class GrowDandelionInMenu : MonoBehaviour
{
    public float maxDandelions;
    public int level;
	// Use this for initialization
	void Start ()
    {
       // Debug.Log(DataManagement.data.savedDandelions[level]);
        float scaleX = DataManagement.data.savedDandelions[level] / maxDandelions * 1.5f + 1;
        Vector3 scale = new Vector3(scaleX, scaleX, scaleX);
        transform.localScale = scale;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
