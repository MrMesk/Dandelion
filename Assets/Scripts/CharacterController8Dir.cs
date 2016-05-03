﻿using UnityEngine;
using System.Collections;

public class CharacterController8Dir : MonoBehaviour
{
    public float speed;
    Rigidbody rigid;
    Vector3 dir;
    Vector3 facingDir;
    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        facingDir = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        dir = Vector3.zero;
        if (vAxis != 0f)
        {
            dir += Vector3.forward * vAxis;
            facingDir = dir;
        }
        if (hAxis != 0f)
        {
            dir += Vector3.right * hAxis;
            facingDir = dir;
        }

    }

    void FixedUpdate()
    {
        dir = dir.normalized * speed * Time.deltaTime;
        rigid.velocity = dir;
        transform.forward = facingDir;
    }
}
