using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour
{
    [Space(10)]
    public LayerMask aggroLayer;
    [Space(10)]
    [Range(5f, 30f)]
    public float aggroRange;
    [Range(0f, 5f)]
    public float lightIncreasePerWisp;
    [Space(10)]
    [Range(5, 10)]
    public int step1;
    [Range(10, 20)]
    public int step2;
    [Space(10)]
    public GameObject lightChild;
    [Space(10)]
    public GameObject particleChild;

    bool step1Reached = false;
    bool step2Reached = false;
    float actualLightRange;
    int wispsAscended;
    // Use this for initialization
    void Start ()
    {
        actualLightRange = lightChild.GetComponent<Light>().range;
        wispsAscended = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        NearCheck(aggroRange);
        UpdateLight();
	}

    void NearCheck(float range)
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, range, aggroLayer);
        foreach (Collider c in nearbyObjects)
        {
            if (c.transform.tag == "Wisp")
            {
                c.GetComponent<WispIA>().Ascend(transform);
                //Debug.Log("Aggro a wisp");
            }
        }
    }

    public void AscendWisp()
    {
        wispsAscended++;
        SmoothLightIncrease();
        if(wispsAscended >= step2 && !step2Reached)
        {
            step2Reached = true;
            particleChild.SetActive(true);
        }
        
    }

    public void SmoothLightIncrease()
    {
        actualLightRange += lightIncreasePerWisp;
    }
    void UpdateLight()
    {
        lightChild.GetComponent<Light>().range = Mathf.Lerp(lightChild.GetComponent<Light>().range, actualLightRange, 0.5f * Time.deltaTime);
    }
}
