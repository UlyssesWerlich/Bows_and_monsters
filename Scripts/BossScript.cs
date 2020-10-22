using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool rageMode;
    public float ragePercent;

    public Transform player;
    public GameObject healthBar;
    public GameObject damageBar;
    public Animator bossAnim;
    public bool canMove = true;
    public int damage;
    public float moveSpeed;
    public float health;
    public GameObject bossProjectile;
    public float projectileCount;

    public float shotCooldown;

    private float shotTimer;
    private bool canShoot;
    private Vector3 healthBarScale;
    private float healthPercent;
    private float rageHealth;

    private Vector2 targetSpot;
    public Vector2[] movePoints;

    void Start()
    {
        healthBarScale = healthBar.transform.localScale;
        healthPercent = healthBarScale.x / health;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetNextSpot();
        rageHealth = (ragePercent * health) / 100;
    }

    void UpdateHealthbar(){
        healthBarScale.x = healthPercent * health;
        healthBar.transform.localScale = healthBarScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null){

            if (transform.position.x != targetSpot.x && transform.position.y != targetSpot.y){

                if (canMove) transform.position = Vector2.MoveTowards(transform.position, targetSpot, moveSpeed * Time.deltaTime);

            } else {
                GetNextSpot();
            }

            if (shotTimer <= 0){
                Shoot();

            } else {
                shotTimer -= Time.deltaTime;
            }

            if (health <= rageHealth && !rageMode) Enrage();
        }
    }

    void Enrage(){
        rageMode = true;
        moveSpeed *= 3;
        shotCooldown *= .75f;
        projectileCount *= 3;
        bossAnim.SetTrigger("Rage");
    }

    void Shoot() {
        shotTimer = shotCooldown;
        float angleStep = 360f / projectileCount;
        float angle = 0f;

        for (int i = 0; i <= projectileCount - 1; i++){
            float xPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * 360;
            float yPosition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * 360;

            Vector2 projectileDirection = new Vector2(xPosition, yPosition);

            var projectile = Instantiate(bossProjectile, transform.position, Quaternion.identity);
            projectile.GetComponent<BossProjectileScript>().SetDirection(projectileDirection * 3);
            angle += angleStep;
        } 
    }

    private void GetNextSpot (){
        int randomSpot = Random.Range(0, movePoints.Length);
        targetSpot = movePoints[randomSpot];
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){

            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
        }
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

    IEnumerator Death()
    {
        moveSpeed = 0;
        Destroy(healthBar);
        Destroy(damageBar);
        bossAnim.SetTrigger("Death");
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
