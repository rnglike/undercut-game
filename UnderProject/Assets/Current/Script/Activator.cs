using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
	public bool inside;
	public bool active;

	void Update()
	{
		if(inside)
		{
			if(Input.GetKeyDown("q"))
			{
				active = true;
			}
			else
			{
				active = false;
			}
		}
	}

    void OnTriggerEnter2D()
    {
    	inside = true;
    }

    void OnTriggerExit2D()
    {
    	inside = false;
    }
}
