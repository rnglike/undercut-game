using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplode : MonoBehaviour
{
    // Área de agarrão
    public GameObject explosionArea;

    // Velocidade do inimigo
    public float startSpeed;
    private float speed;

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
        distanceVision = initialDistanceVision;

        currentSize = enemyGFX.localScale;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (locked && player.GetComponent<PlayerController>().playerMovementLock)
        {
            sprite.color = new Color(0, sprite.color.g, sprite.color.b, sprite.color.a);
        }
        else
        {
            locked = false;
            sprite.color = new Color(255, sprite.color.g, sprite.color.b, sprite.color.a);
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
            
        }
    }
}

