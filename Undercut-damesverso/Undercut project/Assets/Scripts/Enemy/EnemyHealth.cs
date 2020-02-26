using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public float enemyHealth = 200.0f;
    private float dazedTime;
    public float startDazedTime;
    public Vector3 RespawnPoint;
    private Animator animator;
    private Component enemy;
    private AudioSource source;
    public AudioClip deathSong;
    //public AudioClip takeDmg;
    private bool isDead;

    //takedamake Sound
    public GameObject takedmgObject;
    private AudioSource takeDmg;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        isDead = false;

        takeDmg = takedmgObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0 && isDead == false)
        {
            isDead = true;
            //animator.SetBool("Quebrei", true);
            source.clip = deathSong;
            source.Play();
            Destroy(GetComponent<Enemy>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider2D>());
        }

        if (dazedTime <= 0)
        {
            RespawnPoint = transform.position;
            
        }
        if (dazedTime > 0)
        {
            transform.position = RespawnPoint;
            dazedTime -= Time.deltaTime;
        }
    }

    public void takeDamage(int damage)
    {
        dazedTime = startDazedTime;
        enemyHealth -= damage;
        Debug.Log("Enemy sofre dano");
        takeDmg.Play();

    }
    public void limpaCorpos()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (var i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
    }

}
