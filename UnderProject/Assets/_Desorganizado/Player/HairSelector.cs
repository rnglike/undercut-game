using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairSelector : MonoBehaviour
{
    public GameObject[] hairs;
    private int hairCode = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int aux = gameObject.GetComponentInParent<PlayerItems>().GetHairCode();
        if (aux != hairCode) {
            hairShift(aux);
        }
    }


    private void hairShift(int aux) {
        hairs[hairCode].SetActive(false);

        hairs[aux].SetActive(true);
        hairCode = aux;
    }
}
