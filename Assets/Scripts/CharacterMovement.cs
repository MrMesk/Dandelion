using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    Rigidbody rigid;
    Vector3 dir;
	// Use this for initialization
	void Start ()
    {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        if(vAxis != 0f)
        {
            dir = transform.forward * speed * vAxis * Time.deltaTime;
        }
        else
        {
            dir = Vector3.zero;
        }

        if(hAxis != 0f)
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * hAxis);
        }

        rigid.velocity = dir;
	}
}
