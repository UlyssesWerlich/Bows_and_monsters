using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebScript : MonoBehaviour
{

    private Transform player;
    private Vector2 target;

    public float stuckTime;
    public float projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);
        FixAngle();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            Destroy(gameObject);
        }
    }

    private void FixAngle()
    {
        var dir = player.position - transform.position;// - Camera.main.WorldToScreenPoint(transform.position)
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().IsStuck(stuckTime);
            Destroy(gameObject);
        }
    }
}
