using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int enemyIndex;
    public int enemyHealth;

    private Transform player;
    public Transform Enemy;

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

    private Pickable pickable;
    public ParticleScript puft;

    void Start()
    {
        
        //animator = GetComponent<Animator>();  (será usado depois)
        source = GetComponent<AudioSource>();
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //takeDmg = takedmgObject.GetComponent<AudioSource>(); (será usado depois)
        pickable = GetComponent<Pickable>();

    }

    void Update()
    {
        if (enemyHealth <= 0)
        {
            if(isDead == false)
            {
                isDead = true;

                //animator.SetBool("Quebrei", true);  (será usado depois)
                //source.Play();  (será usado depois)
                GetComponent<Enemy>().sprite.color = new Color(255,255,255,255);
                puft.PlayParticle();
                Destroy(GetComponent<SpriteRenderer>());
                Destroy(GetComponent<BoxCollider2D>());
            }

            if(!puft.GetComponent<ParticleSystem>().IsAlive())
            {
                player.GetComponent<PlayerController>().killCount[enemyIndex] += 1;
                Destroy(gameObject);
            }
        }

        // Literalmente mata o inimigo se o hp dele ficar zero.
        
        if (transform.tag != "Doguinho")
        {
            if (enemyHealth <= 0)
            {

                pickable.status = true;

            }
        }  
        
        if (transform.tag != "Doguinho")
        {
            if (!pickable)
            {
                // Controla um Stun que o inimigo toma após receber um ataque.
                if (dazedTime <= 0)
                {

                    transform.GetComponent<Enemy>().search = true;

                }

                if (dazedTime > 0)
                {

                    transform.GetComponent<Enemy>().search = false;
                    dazedTime -= Time.deltaTime;

                    // Solta o player do agarrão se ele acertar o alvo (TALVEZ NÃO SEJA NECESSÁRIO)
                    //if (gameObject.CompareTag("Enemy Terrestre"))
                    {
                        //player.GetComponent<PlayerController>().FreeThePlayer();
                        //player.GetComponent<PlayerController>().getPlayerLock();
                    }
                }
            }
        }

    }

    // Controla o efeito do ataque do Player.
    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;

        transform.GetComponent<Rigidbody2D>().AddForce((new Vector2((8*(player.localScale.x/Mathf.Abs(player.localScale.x))),8)),ForceMode2D.Impulse);
        
        player.GetComponent<PlayerController>().FreeThePlayer();

        //Enemy.GetComponent<Enemy>().LockedFalse();

        if (transform.tag != "Doguinho")
        {
            Enemy.GetComponent<Enemy>().LockedFalse();
        }

        dazedTime = startDazedTime;
        
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
