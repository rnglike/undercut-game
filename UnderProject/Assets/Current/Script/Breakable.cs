using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public ParticleScript ps;
    bool toDestroy;

    void Update()
    {
        if(toDestroy && !ps.GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
    }

	public void TakeDamage()
    {
        ps.PlayParticle();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        toDestroy = true;
    }
}
