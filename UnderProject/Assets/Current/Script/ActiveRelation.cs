using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRelation : MonoBehaviour
{
    public List<GameObject> targets;

    void Start()
    {
        foreach(GameObject target in targets)
        {
            if(transform.tag == "button")
            {
                this.transform.Find("Area").GetComponent<Pressurable>().doors.Add(target);
            }
            else
            {
                target.transform.Find("Area").GetComponent<Pressurable>().doors.Add(this.gameObject);
            }
        }
    }
}
