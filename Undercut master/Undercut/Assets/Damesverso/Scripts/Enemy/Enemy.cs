using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Idéia para limitar o agarrão na area do quadrado, sem sucesso no momento
    public Transform grabPosition;
    public float grabRangeX;
    public float grabRangeY;

    // Inimigo segurar o jogador
    public float InitialLockDistance;
    public float lockDistance;
    public float durationtimeLocked;
    public float cooldowntimeLocked;

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

    public Transform enemyGFX;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 move;

    //private Animator anim; (será usado depois)

    void Start()
    {
        
        //anim = GetComponent<Animator>(); (será usado depois)
        //anim.SetBool("isRunning", true); (será usado depois)
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        lockDistance = InitialLockDistance;
        speed = startSpeed;
        boostTime = boostTimeSet;
        
    }

    void Update()
    {
        if (boostTime > 0)
        {

        	if (isRunning)
    		{
            
            	speed = startSpeed;
            
            }
            else
            {

            	speed = speedBoost;

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
        
        // Pega a distância do Rigidbody do inimigo e o Transform do jogador, e compara com o Campo de Visão do inimigo.          
        if (Vector2.Distance(rb.position, player.position) <= distanceVision)
        {
            // move = Vector2.MoveTowards(transformE.position, player.position, speed * Time.deltaTime);            //Possibilidade de usar o Transform para movimentação, mas não recomendado.

            // Faz o inimigo se mover para onde o jogador está.
            move = ((Vector2)player.position - rb.position);
            move = move / move.magnitude;
            rb.velocity = new Vector2(move.x * speed, rb.velocity.y);

        }

        // Como não animação, isso serve para ver para virar as sprites dependendo a direção do inimigo (direita ou esquerda).
        if (rb.velocity.x >= 0.01f)
        {
            
            enemyGFX.localScale = new Vector3(-2.5f, 2.5f, 2.5f);

        }
        else if (rb.velocity.x <= -0.01f)
        {
            
            enemyGFX.localScale = new Vector3(2.5f, 2.5f, 2.5f);

        }

        // Pega a distância do Rigidbody do inimigo e o Transform do jogador, e compara com o alcance de agarrão do inimigo. Se positivo, ativa o Lock no Player assim como no cooldown.
        if (Vector2.Distance(rb.position, player.position) <= lockDistance)
        {
            
            player.GetComponent<PlayerController>().LockThePlayer();
            StartCoroutine(Delay());

        }

    }

    // Controla o Lock do Player assim como o Unlock dele, meio desorganizado no momento, mas parece funcionar.

    IEnumerator Delay()
    {

        yield return new WaitForSeconds(durationtimeLocked);
        LockDelay();

    }

    public void LockDelay ()
    {

        lockDistance = 0;
        StartCoroutine(FreeDelay());

    }

    IEnumerator FreeDelay()
    {

        yield return new WaitForSeconds(cooldowntimeLocked);
        FreeLockDelay();

    }

    private void FreeLockDelay()
    {
        
        lockDistance = InitialLockDistance;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(grabPosition.position, new Vector3(grabRangeX, grabRangeY, 1));
    }

}
