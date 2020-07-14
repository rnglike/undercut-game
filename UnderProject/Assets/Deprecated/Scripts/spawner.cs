using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
	public GameObject inimigo;
	public bool button;

	public Activator activator;

	bool once = true;

	void Awake()
	{
		activator = GameObject.FindGameObjectWithTag("acti").GetComponent<Activator>();
	}

	void Update()
	{
		if(button)
		{
			if(activator.active)
			{
				Spawn();
			}
		}
		else
		{
			if(activator.inside && once)
			{
				once = false;
				Spawn();
			}
		}
	}

	void Spawn()
	{
		Instantiate(inimigo,(transform.position + new Vector3(0,1,0)),Quaternion.identity);
	}
}