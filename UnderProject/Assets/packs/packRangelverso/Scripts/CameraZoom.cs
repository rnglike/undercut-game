using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [System.Serializable]
    public struct nearAndFar
    {
        [Range(0.0f,50.0f)]
        public float near;
        [Range(0.0f,50.0f)]
        public float far;
    }

    //Reference
    private Camera cam;

    //Camera's z-range
    [SerializeField]
    public nearAndFar distance;

    //Camera's smoothness while changing
    public float rate;

    //Boolean
    public bool aproximated;

    void Start()
    {
        cam = GetComponent<Camera>();

        if(!aproximated)
        {
            cam.orthographicSize = distance.far;
        }
    }

    void Update()
    {
        float temp_oldSize = cam.orthographicSize;
        float temp_newSize;

        if(aproximated)
        {
            temp_newSize = distance.near;
        }
        else
        {
            temp_newSize = distance.far;
        }
        
        float temp_smoothFixSize = Mathf.MoveTowards(temp_oldSize,temp_newSize,(rate * Time.deltaTime));
        cam.orthographicSize = temp_smoothFixSize;
    }
}
