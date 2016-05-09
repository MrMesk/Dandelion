using UnityEngine;
using System.Collections;

public class StickToPlayer : MonoBehaviour
{
    public GameObject player;
    Vector3 pos;
	// Use this for initialization
	void Start ()
    {
        pos.y = transform.position.y;
	    if(!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        pos = new Vector3(player.transform.position.x, pos.y, player.transform.position.z);
        transform.position = pos;
	}
}
