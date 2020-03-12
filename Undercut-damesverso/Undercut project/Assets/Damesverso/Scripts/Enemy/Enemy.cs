using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    // Inimigo segurar o jogador
    public float InitialLockDistance;
    public float lockDistance;
    public float starttimeLocked;
    public float freetimeLocked;

    // Velocidade do inimigo
    public float startSpeed;
    public float speed;

    // Speed Boost do Inimigo (Não sei como fazer funcionar)
    public bool speedboostActivated;
    public int speedboostOn;
    public int speedboostOff;
    public float speedBoost;
    // ****Contador para o speed Boost, é nessa parte que da problema, ou o boosTime fica sempre igual ao startboostTime, ou ao chegar a zero volta para o valor do startBoost em um loop. nunca ficando negativo**** 
    private float boostTime;
    public float startboostTime;

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


        // Parte do código que acredito está correto para o boost do inimigo. Falta ajeitar o contador e o IEnumerator
        if (speedboostActivated == true)
        {
            if (boostTime > 0)
            {

                speed = speedBoost;

            }
            
        }


        if (speedboostActivated == false)
        {
            if (boostTime <= 0)
            {
                
                speed = startSpeed;

            }
        }

    }

    // Controla o Lock do Player assim como o Unlock dele, meio desorganizado no momento, mas parece funcionar.
    private void LockDelay ()
    {

        lockDistance = 0;
        StartCoroutine(FreeDelay());

    }

    private void FreeLockDelay()
    {
        
        lockDistance = InitialLockDistance;

    }

    IEnumerator Delay()
    {
        
        yield return new WaitForSeconds(starttimeLocked);
        LockDelay();

    }

    IEnumerator FreeDelay()
    {
        
        yield return new WaitForSeconds(freetimeLocked);
        FreeLockDelay();

    }

    // Serviria para ativar e desativar o Speed Boost do inimigo, com dificuldades para por funcionando
    IEnumerator BoostEnemyOn()
    {
       
        yield return new WaitForSeconds(speedboostOn);
        
    }

    IEnumerator BoostEnemyOff()
    {
        
        yield return new WaitForSeconds(speedboostOff);

    }

}
