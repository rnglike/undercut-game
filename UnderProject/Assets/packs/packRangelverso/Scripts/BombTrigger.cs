using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrigger : MonoBehaviour
{
	private Building building;
	public bool activated;

	void Start()
	{
		building = GameObject.FindGameObjectWithTag("Building").GetComponent<Building>();
	}

    void OnTriggerStay2D(Collider2D any)
    {
    	if(!building.BombsReady())
    	{
	    	if(Input.GetButtonDown("Interact"))
	    	{
	    		activated = !activated;
	    		if(transform.GetChild(1).GetChild(1).gameObject.activeSelf)
	    		{
	    			transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
	    			transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
	    		}
	    		else
	    		{
	    			transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
	    			transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
	    		}
	    	}
	    }
    }
}
