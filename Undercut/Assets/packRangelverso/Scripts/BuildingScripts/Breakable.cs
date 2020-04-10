using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
	public string type;

	public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
