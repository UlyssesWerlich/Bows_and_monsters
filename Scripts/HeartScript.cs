using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{

    public float timeLifeStatic;
    public float timeLifeTwinkle;
    public float twinkleTime;

    private float twinkleCounter = 0;
    private float timeLifeCounter = 0;
    private bool isTwinkling = false;
    private bool isVisible = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeLifeStatic());
    }

    void Update()
    {
        if (isTwinkling)
        {
            twinkleCounter += Time.deltaTime;
            if (twinkleCounter >= twinkleTime)
            {
                Twinkle();
                twinkleCounter = 0;
            }

            timeLifeCounter += Time.deltaTime;
            if (timeLifeCounter >= timeLifeTwinkle)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator TimeLifeStatic()
    {
        yield return new WaitForSeconds(timeLifeStatic);
        isTwinkling = true;
    }

    void Twinkle()
    {
        if (isVisible)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        isVisible = !isVisible;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().GetHeart();
            Destroy(gameObject);
        }
    }
}
