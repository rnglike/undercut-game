using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Inimigo Reconhecer/Dar dano no Player
    public LayerMask whatIsMyEnemy;
    private float cooldown;
    public float startCooldown;
    public int enemyDamage;
    //public Transform enemyAttackPos;  (Onde por a área de ataque do inimigo)
    public float startAtkAnimTime;
    private float atkAnimTime;
    private bool atkwait;

    // Tentativa futil de empurrar horizontalmente o player, mas ele salta agora
    private Vector2 auxx;

    // Dar choque
    public bool EnemyCanShock;
    public float TimeToGetShocked;
    
    // Define a força do salto da explosão
    public float pushForce;
    
    // Ativar tempo de explosão
    public float ExplosionArea;
    public float ExplosionPushArea;
    public float startExplosionTime;
    public bool EnemyCanExplode;
    
    // Área de agarrão
    public GameObject grabTrigger;
    public bool EnemyCanGrab;

    // Velocidade do inimigo
    public float startSpeed;
    private float speed;

    // Speed Boost do Inimigo
    public bool isRunning;
    public float speedBoost;
    // ****Contador para o speed Boost
    public float boostTimeSet;
    public float boostTime;

    // Campo de visão do inimigo
    public float distanceVision;
    public float initialDistanceVision;

    public Transform enemyGFX;
    private Transform player;
    private Rigidbody2D playerRb;
    private Rigidbody2D rb;
    private Vector2 move;

    private Vector3 currentSize;
    public SpriteRenderer sprite;
    public bool locked;
    public bool search;

    public Animator anim;

    // Efeitos Sonoros
    private AudioSource source;
    public GameObject explosionSound;
    private AudioSource explosionAudio;
    public AudioClip shockAudio;


    //private Animator anim; (será usado depois, para animação)

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        speed = startSpeed;
        boostTime = boostTimeSet;
        distanceVision = initialDistanceVision;
        currentSize = enemyGFX.localScale;
        sprite = GetComponent<SpriteRenderer>();
        //Audio
        source = GetComponent<AudioSource>();
        //ExplosionSound
        explosionAudio = explosionSound.GetComponent<AudioSource>();
    }

    public void LockedFalse()
    {
        locked = false;
    }

    void Update()
    {
        auxx = ((Vector2)player.position);

        if (locked && player.GetComponent<PlayerController>().playerMovementLock)
        {
            anim.SetBool("isShocking", true);
        }
        else
        {
            locked = false;
            anim.SetBool("isShocking", false);
        }

        // Enquanto tiver o boost de velocidade, a área de agarrão se ativa
        if (boostTime > 0)
        {
        	if (isRunning)
    		{ 
            	speed = startSpeed;
                grabTrigger.SetActive(false);
                //player.GetComponent<PlayerController>().FreeThePlayer();
            }
            else
            {
            	speed = speedBoost;
                grabTrigger.SetActive(true);
            }
        }
        else if (boostTime <= 0)
        {
            boostTime = boostTimeSet;
            isRunning = !isRunning;
        }

        if(boostTime > 0)
        {
            boostTime--;
        }

        // Como não animação, isso serve para ver para virar as sprites dependendo a direção do inimigo (direita ou esquerda). Aumenta ou diminue a sprite tambem, se a escala for diferente do base
        if (rb.velocity.x >= 0.01f)
        {            
            enemyGFX.localScale = new Vector3(-currentSize.x, currentSize.y, currentSize.z);
            anim.SetBool("isRunning", true);
        }
        else if (rb.velocity.x <= -0.01f)
        {           
            enemyGFX.localScale = new Vector3(currentSize.x, currentSize.y, currentSize.z);
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    void FixedUpdate()
    {
        if (search)
        {
            // Pega a distância do Rigidbody do inimigo e o Transform do jogador, e compara com o Campo de Visão do inimigo.          
            if (Vector2.Distance(rb.position, player.position) <= distanceVision)
            {
                // move = Vector2.MoveTowards(transformE.position, player.position, speed * Time.deltaTime);            //Possibilidade de usar o Transform para movimentação, mas não recomendado.
                // Faz o inimigo se mover para onde o jogador está.
                move = ((Vector2)player.position - rb.position);
                move = move / move.magnitude;
                rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
            }
        }
    }

    // Se o player estiver na área, trava ele
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {           
            if (EnemyCanGrab == true)
            {
                player.GetComponent<PlayerController>().LockThePlayer();
                locked = true;
                //distanceVision = 0;
                
                if (EnemyCanShock == true && locked == true)
                {
                    StartCoroutine(SetShockTime());
                }
            }
            
            if (EnemyCanExplode == true)
            {
                speed = 0;
                StartCoroutine(SetExplosionTime());
            }       
        }      
    }

    IEnumerator SetExplosionTime()
    {
        yield return new WaitForSeconds(startExplosionTime);

        if (Vector2.Distance(rb.position, player.position) <= ExplosionArea)
        {
            player.GetComponent<PlayerController>().DestroyPlayer();
            
        }
        else if (Vector2.Distance(rb.position, player.position) <= ExplosionPushArea)
        {
            auxx.y = auxx.y + 2;
            Vector2 aux = (auxx - (Vector2)transform.position);
            Vector2 direction = aux / aux.magnitude;
            playerRb.velocity = playerRb.velocity + (direction * pushForce);
            player.GetComponent<Playerlifes>().PlayerTakeDamage(enemyDamage);
            Debug.Log("PlayerMandadoPraEstratosfera");
            
        }        
        Destroy(gameObject);
        // explosionAudio.Play();
    }

    IEnumerator SetShockTime()
    {
        yield return new WaitForSeconds(TimeToGetShocked);
        if (locked == true)
        {
            player.GetComponent<Playerlifes>().PlayerTakeDamage(enemyDamage);
            source.clip = shockAudio;
            source.Play();
        }
    }   

}
