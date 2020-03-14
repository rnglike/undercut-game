using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
    private bool isRewinding = false;
    public float recordTime = 5f;
    List<PointInTime> pointsInTime;
    Rigidbody2D rb;


    void Start()
    {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.Return)) {
            StopRewind();
        }
    }

    void FixedUpdate() {
        if (isRewinding) {
            Rewind();
        } else {
            Record();
        }
    }


    private void Rewind() {
        if (pointsInTime.Count > 0) {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            rb.velocity = pointInTime.rbVelocity;
            pointsInTime.RemoveAt(0);
        } else {
            StopRewind();
        }
    }


    private void Record() {
        if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime)) {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, rb.velocity));
    }


    public void StartRewind() {
        isRewinding = true;
        rb.isKinematic = true;
    }


    public void StopRewind() {
        isRewinding = false;
        rb.isKinematic = false;
    }
}
