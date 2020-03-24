using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI2 : MonoBehaviour
{
    public float startSpeed;
    private float dazedTime;
    public float startDazedTime;



    // Define o objeto a ser seguido pelo inimigo
    public Transform player;

    // Velocidade do inimigo
    public float speed = 200f;

    // Distância entre as atualizações do caminho
    public float nextWaypointDistance = 3f;

    // Campo de visão do inimigo
    public float distanceVision;

    // Usado para fazer a sprite do inimigo virar dependendo da direção horizontal
    public Transform enemyGFX;

    // Geração e marcação do caminho
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private Vector2 direction;
    private Transform target;
    private Transform door;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);

    }

    void UpdatePath()
    {

        if(seeker.IsDone())
            seeker.StartPath(rb.position, (player.position + new Vector3(0,1,0)), OnPathComplete);

    }
 

    // Caso o caminho gerado não tenha erro, atualiza ele.
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {

            path = p;
            currentWaypoint = 0;
                
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        // Se o ponto atual (nº de atualizações atuais) for igual ou maior que a quantidade total, chegou ao destino (Acho q tem algum problema).
        if(currentWaypoint >= path.vectorPath.Count)
        {

            reachedEndOfPath = true;
            return;

        }
        else
        {

            reachedEndOfPath = false;

        }

        
        // Define a distância entre o inimigo e o próximo ponto de atualização.
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        // Faz o campo de visão do inimigo, comparando a distância do player para o inimigo. Executando o movimento caso esteja dentro do campo.
        if (Vector2.Distance(rb.position, player.position) <= distanceVision)
        {

            direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            // Vira a sprite dependendo da direção horizontal.
            if (force.x >= 0.01f)
            {

                enemyGFX.localScale = new Vector3(-2.5f, 2.5f, 2.5f);

            }
            else if (force.x <= -0.01f)
            {

                enemyGFX.localScale = new Vector3(2.5f, 2.5f, 2.5f);

            }

        }

        // Atualiza a marcação do próximo ponto.
        if (distance < nextWaypointDistance)
        {
            
            currentWaypoint++;

        }

    }

    // Jumpping???
    void Update() {
        if ((player.position.y + 0.3)> transform.position.y || direction.y > 0.98)
        {
            rb.gravityScale = -0.5f;
        } else
        {
            rb.gravityScale = 1.5f;
        }

    }

}
