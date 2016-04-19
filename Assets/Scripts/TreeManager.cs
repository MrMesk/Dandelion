using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour
{
    [Space(10)]
    public LayerMask aggroLayer;
    [Space(10)]
    [Range(5f, 30f)]
    public float aggroRange;
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
    int wispsAscended;
    // Use this for initialization
    void Start ()
    {
        wispsAscended = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        NearCheck(aggroRange);
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
        if(wispsAscended >= step2 && !step2Reached)
        {
            step2Reached = true;
            particleChild.SetActive(true);
        }
        else if (wispsAscended >= step1 && !step1Reached)
        {
            step1Reached = true;
            lightChild.SetActive(true);
            GetComponent<Light>().range = 15f;
        }
    }
}
