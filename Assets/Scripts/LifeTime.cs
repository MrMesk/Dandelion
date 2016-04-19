using UnityEngine;
using System.Collections;

public class LifeTime : MonoBehaviour
{
    public float lifeTime = 1f;
    float timer = 0f;

	void Update ()
    {
        timer += Time.deltaTime;
        if(timer >= lifeTime)
        {
            Destroy(gameObject);
        }
	}
}
