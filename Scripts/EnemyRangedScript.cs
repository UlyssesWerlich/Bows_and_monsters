using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedScript : MonoBehaviour
{

    private float shotTimer;
    private bool canShoot = true;
    private bool canMove = true;
    private Vector3 healthBarScale;
    private float healthPercent;

    public Animator enemyAnim;

    public Transform player;
    public GameObject healthBar;
    public GameObject damageBar;
    public float moveSpeed;
    public float shotCooldown;
    public float stoppingDistance;
    public float retreatDistance;

    public float health;

    public GameObject arrow;
    public int changeDropArrow;

    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        healthBarScale = healthBar.transform.localScale;
        healthPercent = healthBarScale.x / health;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shotTimer = shotCooldown;
    }

    void UpdateHealthbar(){
        healthBarScale.x = healthPercent * health;
        healthBar.transform.localScale = healthBarScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && canMove){
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance){

                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            } else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance ){

                transform.position = this.transform.position;
                
                if (canShoot) Shoot();

            } else if (Vector2.Distance(transform.position, player.position) < retreatDistance){

                transform.position = Vector2.MoveTowards(transform.position, player.position, -moveSpeed * Time.deltaTime);
            }

            if (shotTimer <= 0 ){
                canShoot = true;
            } else {
                canShoot = false;
                shotTimer -= Time.deltaTime;
            }
        }
    }

    void Shoot(){
        Instantiate(projectile, transform.position, Quaternion.identity);
        shotTimer = shotCooldown;
    }

    public void TakeDamage(float damage){
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                StartCoroutine("Death");
            }
            UpdateHealthbar();
        }
    }

    public void Freeze()
    {
        StartCoroutine("IsFreeze");
    }

    IEnumerator IsFreeze()
    {
        canMove = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.6f, 1, 1);
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        canMove = true;
    }

    IEnumerator Death()
    {
        moveSpeed = 0;
        Destroy(healthBar);
        Destroy(damageBar);
        enemyAnim.SetTrigger("Death");
        yield return new WaitForSeconds(0.4f);
        DropArrow();

        Destroy(gameObject);
    }

    private void DropArrow()
    {
        if (Random.Range(0, changeDropArrow) == 1)
        {
            Instantiate(arrow, transform.position, Quaternion.identity);
        }
    }
}
