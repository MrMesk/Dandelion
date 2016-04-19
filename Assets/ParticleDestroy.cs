using UnityEngine;
using System.Collections;

public class ParticleDestroy : MonoBehaviour
{
    public float lifeTime;
	// Use this for initialization
	void Start ()
    {
	
	}
    IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
