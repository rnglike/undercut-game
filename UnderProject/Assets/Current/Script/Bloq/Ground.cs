using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : BaseEnemy
{
    public Bloquito bloq;
    private bool key = false;
    // Start is called before the first frame update
    void Start()
    {
        //bloq = GetComponentInParent<Bloquito>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!key)
        {
            CircleAttack(0);
        }        
    }

    protected new void CircleAttack(int dmg)
    {
        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll((Vector2)transform.position, circleAttackRadius, whatIsEnemy);
        if (enemiesToHit.Length > 0)
        {
            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                // bloq.setMode(1);
                key = true;
            }
        }
    }
}
