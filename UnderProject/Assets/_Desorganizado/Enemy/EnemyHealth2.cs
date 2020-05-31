using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth2 : MonoBehaviour
{
    public int hp;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        transform.GetComponent<Rigidbody2D>().AddForce((new Vector2((8 * (player.localScale.x / Mathf.Abs(player.localScale.x))), 8)), ForceMode2D.Impulse);
        hp -= damage;
        Debug.Log("Enemy sofre dano");
    }
}
