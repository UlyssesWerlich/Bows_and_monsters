using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDroppedScript : MonoBehaviour
{
    public float timeLifeStatic;
    public float timeLifeTwinkle;
    public float twinkleTime;
    public Color[] colorArrows;

    private int selectedArrow;
    private float twinkleCounter = 0;
    private float timeLifeCounter = 0;
    private bool isTwinkling = false;
    private bool isVisible = true;

    // Start is called before the first frame update
    void Start()
    {
        SelectArrow();
        StartCoroutine(TimeLifeStatic());
    }

    void SelectArrow()
    {
        selectedArrow = Random.Range(0, 2);
        if (selectedArrow == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.8f, 1, 1);
        } else if (selectedArrow == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.8f, 0, 1);
        }
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
            collision.gameObject.GetComponent<PlayerScript>().ChangeArrow(selectedArrow, colorArrows[selectedArrow]);
            Destroy(gameObject);
        }
    }
}
