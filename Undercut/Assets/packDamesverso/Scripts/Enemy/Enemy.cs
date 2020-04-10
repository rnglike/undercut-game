using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Área de agarrão
    public GameObject grabTrigger;

    // Velocidade do inimigo
    public float startSpeed;
    private float speed;

    // Speed Boost do Inimigo (Não sei como fazer funcionar)
    public bool isRunning;
    public float speedBoost;
    // ****Contador para o speed Boost, é nessa parte que da problema, ou o boosTime fica sempre igual ao startboostTime, ou ao chegar a zero volta para o valor do startBoost em um loop. nunca ficando negativo**** 
    public float boostTimeSet;
    public float boostTime;

    // Campo de visão do inimigo
    public float distanceVision;
    public float initialDistanceVision;

    public Transform enemyGFX;
    private Transform player;
    public Rigidbody2D rb;
    private Vector2 move;

    private Vector3 currentSize;
    public SpriteRenderer sprite;
    private bool locked;
    public bool search;

    //private Animator anim; (será usado depois)

    void Start()
    {
        
        //anim = GetComponent<Animator>(); (será usado depois)
        //anim.SetBool("isRunning", true); (será usado depois)
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        speed = startSpeed;
        boostTime = boostTimeSet;
        distanceVision = initialDistanceVision;

        currentSize = enemyGFX.localScale;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(locked && player.GetComponent<PlayerController>().playerMovementLock)
        {
            sprite.color = new Color(0,sprite.color.g,sprite.color.b,sprite.color.a);
        }
        else
        {
            locked = false;
            sprite.color = new Color(255,sprite.color.g,sprite.color.b,sprite.color.a);
        }

        // Enquanto tiver o boost de velocidade, a área de agarrão se ativa
        if (boostTime > 0)
        {

        	if (isRunning)
    		{
            
            	speed = startSpeed;
                grabTrigger.SetActive(false);
                player.GetComponent<PlayerController>().FreeThePlayer();

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
    }

    void FixedUpdate()
    {
        if(search)
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

        // Como não animação, isso serve para ver para virar as sprites dependendo a direção do inimigo (direita ou esquerda). Aumenta ou diminue a sprite tambem, se a escala for diferente do base
        if (rb.velocity.x >= 0.01f)
        {
            
            enemyGFX.localScale = new Vector3(-(currentSize.x - 1), (currentSize.y - 1), currentSize.z);

        }
        else if (rb.velocity.x <= -0.01f)
        {
            
            enemyGFX.localScale = new Vector3((currentSize.x - 1), (currentSize.y - 1), currentSize.z);

        }
        else
        {

        	enemyGFX.localScale = new Vector3(currentSize.x, currentSize.y, currentSize.z);

        }

    }

    // Se o player estiver na área, trava ele
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.GetComponent<PlayerController>().LockThePlayer();
            locked = true;
            //distanceVision = 0;
        }
    }
}
