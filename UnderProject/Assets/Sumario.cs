using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Sumario : MonoBehaviour
{
    public float inTimer;
    public float outTimer;
    public float kills;
    public float pKills;

    public TextMeshProUGUI[] texts;

    public Image derrotas;
    public Image vitorias;
    public TextMeshProUGUI nd1;
    public TextMeshProUGUI nd2;

    public Sprite[] enemy;

    public PlayerController player;

    void Update()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            string textParent = text.transform.parent.name;

            if(textParent == "predios") text.text = " " + pKills.ToString();
            else if(textParent == "itos") text.text = " " + kills.ToString();
            else if(textParent == "infiltracao") text.text = " " + ((int)inTimer/60).ToString() + ":" + ((int)inTimer%60) + ":" + ((int)((inTimer - (int)inTimer)*1000)).ToString();
            else if(textParent == "fuga") text.text = " " + ((int)outTimer/60).ToString() + ":" + ((int)outTimer%60) + ":" + ((int)((outTimer - (int)outTimer)*1000)).ToString();
        }

        foreach (int kills in player.killCount)
        {
            int aux;

            if(Array.IndexOf(player.killCount,kills) + 1 < player.deathCount.Length) aux = Array.IndexOf(player.killCount,kills) + 1;
            else aux = 0;

            if(kills > player.killCount[aux])
            {
                vitorias.enabled = true;
                nd1.enabled = false;
                vitorias.sprite = enemy[Array.IndexOf(player.killCount,kills)];
            }
            else
            {
                vitorias.enabled = false;
                nd1.enabled = true;
            }
        }

        foreach (int death in player.deathCount)
        {
            int aux;

            if(Array.IndexOf(player.deathCount,death) + 1 < player.deathCount.Length) aux = Array.IndexOf(player.deathCount,death) + 1;
            else aux = 0;

            if(death > player.deathCount[aux])
            {
                derrotas.enabled = true;
                nd2.enabled = false;
                derrotas.sprite = enemy[Array.IndexOf(player.deathCount,death)];
            }
            else
            {
                derrotas.enabled = false;
                nd2.enabled = true;
            }
        }
    }
}