using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int enemyHealth;

    private Transform player;
    private Transform enemyTerrestre;

    // Stun no inimigo
    public float dazedTime;
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

    void Start()
    {
        
        //animator = GetComponent<Animator>();  (será usado depois)
        //source = GetComponent<AudioSource>(); (será usado depois
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTerrestre = GameObject.FindGameObjectWithTag("Enemy Terrestre").GetComponent<Transform>();
        //takeDmg = takedmgObject.GetComponent<AudioSource>(); (será usado depois)

    }

    void Update()
    {
        if (enemyHealth <= 0 && isDead == false)
        {
           
            isDead = true;
            //animator.SetBool("Quebrei", true);  (será usado depois)
            //source.clip = deathSong;  (será usado depois)
            //source.Play();  (será usado depois)
            Destroy(GetComponent<Enemy>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider2D>());

        }

        // Literalmente mata o inimigo se o hp dele ficar zero.
        if (enemyHealth <= 0)
        {
            
            Destroy(gameObject);

        }

        // Controla um Stun que o inimigo toma após receber um ataque.
        if (dazedTime <= 0)
        {
            
            RespawnPoint = transform.position;
            
        }
        if (dazedTime > 0)
        {
            transform.position = RespawnPoint;
            dazedTime -= Time.deltaTime;

            // Solta o player do agarrão se ele acertar o alvo (TALVEZ NÃO SEJA NECESSÁRIO)
            //if (gameObject.CompareTag("Enemy Terrestre"))
            {
                //player.GetComponent<PlayerController>().FreeThePlayer();
                //player.GetComponent<PlayerController>().getPlayerLock();
            }
        }

        
    }

    // Controla o efeito do ataque do Player.
    public void TakeDamage(int damage)
    {
        
        dazedTime = startDazedTime;
        enemyHealth -= damage;
        Debug.Log("Enemy sofre dano");
        //takeDmg.Play(); (será usado depois)

    }
   
    // Só será usado caso seja necessário limpar os inimigos sobreviventes depois de um objetivo estiver concluido.
    public void limpaCorpos()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (var i = 0; i < enemies.Length; i++)
        {

            Destroy(enemies[i]);

        }
    }

}
