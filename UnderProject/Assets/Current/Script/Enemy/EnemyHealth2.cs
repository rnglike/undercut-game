using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth2 : MonoBehaviour
{
    public bool isDead;
    public int enemyIndex;
    public ParticleScript puft;
    public int hp;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            if(isDead == false)
            {
                isDead = true;

                if(transform.name == "calder(Clone)") transform.Find("Sprite").GetComponent<SpriteRenderer>().color = new Color(255,255,255,255);
                else if(transform.name == "bloq(Clone)") GetComponent<SpriteRenderer>().color = new Color(255,255,255,255);
                puft.PlayParticle();
                if(transform.name == "calder(Clone)") Destroy(transform.Find("Sprite").gameObject);
                else if(transform.name == "bloq(Clone)") Destroy(GetComponent<SpriteRenderer>());
                Destroy(GetComponent<BoxCollider2D>());
            }
            
            if(!puft.GetComponent<ParticleSystem>().IsAlive())
            {
                player.GetComponent<PlayerController>().killCount[enemyIndex] += 1;
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        transform.GetComponent<Rigidbody2D>().AddForce((new Vector2((8 * (player.localScale.x / Mathf.Abs(player.localScale.x))), 8)), ForceMode2D.Impulse);
        hp -= damage;
        Debug.Log("Enemy sofre dano");
    }
}
