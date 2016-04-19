using UnityEngine;
using System.Collections;

public class Followers : MonoBehaviour
{
    public GameObject[] wisps;
    public Vector3 spawnPoint;
    public int index;
	// Use this for initialization
	void Start ()
    {
        spawnPoint = transform.position;
        wisps = new GameObject[GameObject.FindGameObjectsWithTag("SleepingWisp").Length +1];
        wisps[0] = gameObject;
        index = 0;
	}

    public Transform AddFollower(GameObject g)
    {
        if (index < wisps.Length)
        {
            index++;
            wisps[index] = g;
            g.GetComponent<WispIA>().SetIndex(index);
        }
        return wisps[index - 1].transform;
    }

    public void KillFollower(int i)
    {
        if (i < index)
        {
            wisps[i + 1].GetComponent<WispIA>().SetTarget(wisps[i - 1].transform);

            for (int j = i + 1; j < index; j++)
            {
                wisps[j - 1] = wisps[j];
                wisps[j - 1].GetComponent<WispIA>().SetIndex(j - 1);
            }
            wisps[index] = null;
            index--;
        }
        else
        {
            wisps[index] = null;
            index--;
        }
    }
    public Transform GetFollowing(int i)
    {
        return wisps[i - 1].transform;
    }
    public void PlayerKill()
    {
        for(int j = 1; j <= index; j++)
        {
            wisps[j].GetComponent<WispIA>().ClearFollowers();
        }
        index = 0;
        transform.position = spawnPoint;
    }
    public void PlayerKill2()
    {
        if(index > 0)
        {
            wisps[index].GetComponent<WispIA>().Death();
        }
        else
        {
            index = 0;
            transform.position = spawnPoint;
        }
    }
}
