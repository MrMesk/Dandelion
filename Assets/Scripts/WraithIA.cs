﻿using UnityEngine;
using System.Collections;

public class WraithIA : MonoBehaviour
{
    [Space(10)]
    [Range(5f, 15f)]
    public float nearRange;
    [Range(5f, 15f)]
    public float farRange;
    [Range(0.5f, 100f)]
    public float speed;
    [Range(0.5f, 2f)]
    public float wakingTime;
    [Space(10)]
    public LayerMask aggroLayer;
    [Space(10)]
    public Material[] mats;
    [Space(10)]
    public GameObject particleDeath;
    [Space(10)]
    [FMODUnity.EventRef]
    public string laughSound = "event:/Wraiths/Laugh";
    [FMODUnity.EventRef]
    public string explodeSound = "event:/Wraiths/Explode";
    [FMODUnity.EventRef]
    public string askSound = "event:/Wisps/Ask";

    Material[] materials;
    bool isFar = false;
    bool isNearby = false;
    bool targetSet = false;

    MeshRenderer my_Renderer;

    Transform target;
    Transform targetCheck;

    // Use this for initialization
    void Start ()
    {
        my_Renderer = transform.Find("Model").GetComponent<MeshRenderer>();
        materials = my_Renderer.materials;
        materials[0] = mats[2];
        my_Renderer.materials = materials;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isNearby)
        {
            if (!isFar)
            {
                if (NearbyCheck(farRange, 1))
                {
                    isFar = true;
                    StartCoroutine(Awaking());
                    FMODUnity.RuntimeManager.PlayOneShot(askSound, transform.position);
                }
            }
            else if (NearbyCheck(nearRange, 0))
            {
                SetTarget(targetCheck);
                isNearby = true;
                FMODUnity.RuntimeManager.PlayOneShot(laughSound, transform.position);
            }
        }
        if (targetSet)
        {
            Attack();
        }
    }

    bool NearbyCheck(float range, int matID)
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, range, aggroLayer);
        foreach (Collider c in nearbyObjects)
        {
            if (c.transform.tag == "Player" ||c.transform.tag == "Wisp")
            {
                materials[0] = mats[matID];
                my_Renderer.materials = materials;
                targetCheck = c.transform;
                return true;
            }
            
        }
        return false;
    }

    public void SetTarget(Transform t)
    {
        target = t;
        targetSet = true;
    }

    void Attack()
    {
   
        if(target)
        {
            Vector3 moveDir = target.position - transform.position;
            moveDir = moveDir.normalized * speed * Time.deltaTime;
            // transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.Translate(moveDir, Space.World);
        }
        else
        {
            targetSet = false;
            isNearby = false;
            isFar = false;
            materials[0] = mats[2];
            my_Renderer.materials = materials;
        }
    }

    IEnumerator Awaking()
    {
        yield return new WaitForSeconds(wakingTime);
        if (isNearby == false)
        {
            if (NearbyCheck(farRange, 0))
            {
                isNearby = true;
                isFar = true;
                SetTarget(targetCheck);
                FMODUnity.RuntimeManager.PlayOneShot(laughSound, transform.position);
            }
            else
            {
                // materials[0] = mats[2];
                // my_Renderer.materials = materials;
                wakingTime = wakingTime * 2 / 3;
                isFar = false;
                isNearby = false;
            }
        }
    }

    public void Death()
    {
        Instantiate(particleDeath, transform.position, Quaternion.identity);
        FMODUnity.RuntimeManager.PlayOneShot(explodeSound, transform.position);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Player")
        {
            other.GetComponent<Followers>().PlayerKill();
            Death();
        }
        else if (other.tag == "Wisp")
        {
            other.GetComponent<WispIA>().BreakFollowers();
            Death();
        }
    }
}
