using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{

    public float arrowDamage;
    public float arrowSpeed;
    public float arrowLifeTime;

    public bool freeze;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, arrowLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * arrowSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Enemy")){
            collision.GetComponent<EnemyScript>().TakeDamage(arrowDamage);
            if (freeze) collision.GetComponent<EnemyScript>().Freeze();
            Destroy(gameObject);
        }

        if (collision.CompareTag("Spider"))
        {
            collision.GetComponent<SpiderScript>().TakeDamage(arrowDamage);
            if (freeze) collision.GetComponent<SpiderScript>().Freeze();
            Destroy(gameObject);
        }

        if (collision.CompareTag("EnemyRanged")){
            collision.GetComponent<EnemyRangedScript>().TakeDamage(arrowDamage);
            if (freeze) collision.GetComponent<EnemyRangedScript>().Freeze();
            Destroy(gameObject);
        }

        if (collision.CompareTag("Thing"))
        {
            collision.GetComponent<ThingScript>().TakeDamage(arrowDamage);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Boss")){
            collision.GetComponent<BossScript>().TakeDamage(arrowDamage);
            Destroy(gameObject);
        }
    }
}
