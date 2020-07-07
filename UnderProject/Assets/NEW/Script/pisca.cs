using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pisca : MonoBehaviour
{
	TextMeshProUGUI texto;
	float alpha;

    // Start is called before the first frame update
    void Start()
    {
        texto = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
    	texto.color = new Color(texto.color.r,texto.color.g,texto.color.b,Mathf.PingPong(Time.time,1));
    }
}
