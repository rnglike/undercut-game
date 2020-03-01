using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 aux, aux2;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aux.x = 0;
        aux.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
			//StartRewind();
            Debug.Log("WRY");
            aux2 = rb.velocity;
            rb.velocity = aux;
            rb.isKinematic = true;
		if (Input.GetKeyUp(KeyCode.Return))
			//StopRewind();
            rb.velocity = aux2;
            rb.isKinematic = false;
    }
}
