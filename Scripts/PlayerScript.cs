using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{

    private Vector2 direction;
    private BowScript bow;
    private bool recovering;
    private float recoveryCounter;
    private bool facingRight = true;

    public int health;
    public float recoveryTime;
    public float moveSpeed;
    public Rigidbody2D playerRb;
    public GameScript gameScript;

    public Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        gameScript.UpdateHealthUI(health);
        bow = FindObjectOfType<BowScript>();
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        playerAnim.SetFloat("Speed", Mathf.Abs(direction.x) + Mathf.Abs(direction.y));

        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        
        if (dir.x > 0 && !facingRight || dir.x < 0 && facingRight){
            Flip();
        }

        if(Input.GetMouseButtonDown(0)){
            bow.Shoot();
        }

        if (recovering){
            recoveryCounter += Time.deltaTime;

            if (recoveryCounter >= recoveryTime){
                recoveryCounter = 0;
                recovering = false;
            }
        }
    }

    void Flip(){
        facingRight = !facingRight;
        transform.Rotate( 0f, 180f, 0f);
    }

    private void FixedUpdate(){
        playerRb.MovePosition(playerRb.position + (direction * moveSpeed * Time.fixedDeltaTime));
    }

    public void TakeDamage(int damage){
        if (!recovering){
            recovering = true;
            health -= damage;
            gameScript.UpdateHealthUI(health);

            if ( health <= 0){
                StartCoroutine("Death");
            }
        }
    }

    public void GetHeart()
    {
        if (health < 10)
        {
            health++;
            gameScript.UpdateHealthUI(health);
        }
    }

    public void ChangeArrow(int index, Color colorArrow)
    {
        gameScript.UpdateArrowUI(colorArrow);
        bow.ChangeArrow(index);
    }

    public void IsStuck(float stuckTime)
    {
        if (moveSpeed != 0) StartCoroutine(Stuck(stuckTime));
    }

    IEnumerator Stuck(float stuckTime)
    {
        float i = moveSpeed;
        moveSpeed = 0;
        yield return new WaitForSeconds(stuckTime);
        moveSpeed = i;
    }

    IEnumerator Death()
    {
        moveSpeed = 0;
        playerAnim.SetTrigger("Death");
        yield return new WaitForSeconds(0.4f);
        gameScript.Die();
        Destroy(gameObject);
    }

}
