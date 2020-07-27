using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFall : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool solid;

    void FixedUpdate()
    {
        if(solid)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.isKinematic = false;
        }
    }

    void OnTriggerEnter2D(Collider2D any)
    {
        if(any.transform.tag == "Player")
        {
            if(solid)
            {
                StartCoroutine("WaitToDrop");
            }
        }   
    }

    IEnumerator WaitToDrop()
    {
        yield return new WaitForSeconds(2);
        solid = false;
    }
}
