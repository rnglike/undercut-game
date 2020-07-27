using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ebm : MonoBehaviour
{
    public bool active;
    public string state;
    public string lastState;
    public Rigidbody2D rb;
    public int rand;
    public float vel;
    public float stateTime;
    public float idleTime;
    public float walkTime;

    public bool rightFree;
    public bool leftFree;
    public float minFallHeight;

    public float inc;
    public float aux;

    void Start()
    {
        idleTime = ToSeconds(idleTime);
        walkTime = ToSeconds(walkTime);

        rand = Random.Range(1,10);
        aux = vel;
    }

    void Update()
    {
        CheckState();

        if(rightFree || leftFree)
        {
            if(rand%2 == 0)
            {
                vel = aux;
            }
            else
            {
                vel = -aux;
            }
        }
        else
        {
            vel = 0;
        }

        if(active)
        {
            if(stateTime < 0)
            {
                if(stateTime < -(idleTime * rand/4)) stateTime = walkTime * rand/4;
                else rb.velocity = new Vector2(0,0);
            }
            else
            {
                rb.velocity = transform.right * vel;

                if((rightFree && !leftFree) && vel/Mathf.Abs(vel) == -1) aux = -aux;
                else if((!rightFree && leftFree) && vel/Mathf.Abs(vel) == 1) aux = -aux;
            }
        }

        if(state != lastState)
        {
            lastState = state;
            rand = Random.Range(1,10);
        }

        stateTime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        LayerMask layerMask = LayerMask.GetMask("Ground");

        RaycastHit2D hitR = Physics2D.Raycast((transform.position + Vector3.up),((Vector2.right * (1 - inc)) + (Vector2.down * inc)),layerMask);
        RaycastHit2D hitL = Physics2D.Raycast((transform.position + Vector3.up),((Vector2.left * (1 - inc)) + (Vector2.down * inc)),layerMask);

        if(hitR.collider != null || hitL.collider != null)
        {
            Debug.DrawRay(transform.position,((Vector2.right * (1 - inc)) + (Vector2.down * inc)) * hitR.distance,Color.blue);
            Debug.DrawRay(transform.position,((Vector2.left * (1 - inc)) + (Vector2.down * inc)) * hitL.distance,Color.blue);

            if(hitR.distance <= minFallHeight && hitR.distance != 0)
            {
                rightFree = true;
            }
            else
            {
                rightFree = false;
            }

            if(hitL.distance <= minFallHeight && hitL.distance != 0)
            {
                leftFree = true;
            }
            else
            {
                leftFree = false;
            }
        }
        else
        {
            rightFree = false;
            leftFree = false;
        }
    }

    float ToSeconds(float num){return (num*(Time.deltaTime*60));}

    void CheckState()
    {
        if(stateTime < 0)
        {
            state = "Idle";
        }
        else
        {
            state = "Walking";
        }
    }
}
