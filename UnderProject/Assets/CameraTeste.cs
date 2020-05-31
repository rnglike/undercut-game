using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTeste : MonoBehaviour
{
    public float sideEnclosure;
    public float playerEnclosure;

    public float xAxisVelocity; //Controls camera's x-axis velocity.

    private Camera cam;
    private GameObject player;

    private Vector3 camPos;
    private Vector3 playerPos;

    public float aux;

    void Start()
    {
        cam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        camPos = cam.transform.position;
        playerPos = player.transform.position;

        float temp_IntClampX = IntervalClamp(camPos.x,playerEnclosure,playerPos.x,xAxisVelocity);
        float temp_ClampX = Mathf.Clamp(temp_IntClampX,-sideEnclosure,sideEnclosure);

        cam.transform.position = new Vector3(temp_ClampX,camPos.y,camPos.z);
    }

    float IntervalClamp(float origin,float distance,float value,float rate)
    {
        if( (value < (origin - distance)) || (value > (origin + distance)) )
        {
            origin = Mathf.MoveTowards(origin,value,(rate * Time.deltaTime));
        }

        return origin;
    }

    void OnDrawGizmosSelected()
    {
        cam = GetComponent<Camera>();
        camPos = cam.transform.position;

        Gizmos.color = Color.white;
        Gizmos.DrawCube(new Vector3(sideEnclosure,camPos.y,camPos.z),new Vector3(1,1,0));
        Gizmos.DrawCube(new Vector3(-sideEnclosure,camPos.y,camPos.z),new Vector3(1,1,0));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(camPos.x + playerEnclosure,camPos.y,camPos.z),new Vector3(.5f,.5f,0));
        Gizmos.DrawCube(new Vector3(camPos.x - playerEnclosure,camPos.y,camPos.z),new Vector3(.5f,.5f,0));
    }
}