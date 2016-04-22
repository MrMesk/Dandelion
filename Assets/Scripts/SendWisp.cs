using UnityEngine;
using System.Collections;

public class SendWisp : MonoBehaviour
{
    Followers follow;
	// Use this for initialization
	void Start ()
    {
        follow = GetComponent<Followers>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(follow.index > 0)
                {
                    Vector3 destination = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
                    GameObject destinationFeedback = Instantiate(Resources.Load("Feedbacks/Destination") as GameObject, destination, Quaternion.identity) as GameObject;
                    follow.wisps[1].GetComponent<WispIA>().GoToLocation(destinationFeedback.transform);
                }
            }
        }
    }
}
