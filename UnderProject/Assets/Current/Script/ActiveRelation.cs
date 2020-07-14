using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRelation : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        if(target != null)
        {
            if(transform.tag == "button")
            {
                this.transform.Find("Area").GetComponent<Pressurable>().door = target.gameObject;
            }
            else
            {
                target.transform.Find("Area").GetComponent<Pressurable>().door = this.gameObject;
            }
        }
    }
}
