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

    private CheatHelper ch;
    private Pickable pickable;

    void Start()
    {
        
        //animator = GetComponent<Animator>();  (será usado depois)
        source = GetComponent<AudioSource>();
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTerrestre = GameObject.FindGameObjectWithTag("Enemy Terrestre").GetComponent<Transform>();
        //takeDmg = takedmgObject.GetComponent<AudioSource>(); (será usado depois)

        ch = GameObject.FindGameObjectWithTag("Interface").GetComponent<CheatHelper>();
        pickable = GetComponent<Pickable>();

    }

    void Update()
    {
        if (enemyHealth <= 0 && isDead == false)
        {
           
            isDead = true;
            //animator.SetBool("Quebrei", true);  (será usado depois)
            //source.Play();  (será usado depois)
            GetComponent<Enemy>().sprite.color = new Color(100,100,100,GetComponent<Enemy>().sprite.color.a);
            Destroy(GetComponent<Enemy>());

        }

        // Literalmente mata o inimigo se o hp dele ficar zero.
        if (enemyHealth <= 0)
        {
            
            pickable.status = true;

        }

        if(!pickable)
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

    // Controla o efeito do ataque do Player.
    public void TakeDamage(int damage)
    {
        
        transform.GetComponent<Rigidbody2D>().AddForce((new Vector2((8*(player.localScale.x/Mathf.Abs(player.localScale.x))),8)),ForceMode2D.Impulse);
        dazedTime = startDazedTime;
        enemyHealth -= damage;
        Debug.Log("Enemy sofre dano");
        if(ch.mvpSorry)
        {
        	source.clip = deathSong;
        	source.Play();
        }
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
