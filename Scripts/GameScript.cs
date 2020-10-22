using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{

    private bool isPaused;

    public string menuScene;
    public string gameScene;

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject UIPanel;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Image arrowUI;

    public Text waveText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){
            if (!isPaused){
                isPaused = true;
                pausePanel.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0;

            } else {
                isPaused = false;
                pausePanel.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1;

            }
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void UpdateWaveUI(int indexWave)
    {
        StartCoroutine(TimeShowWaveText(indexWave));
    }

    IEnumerator TimeShowWaveText(int indexWave)
    {
        indexWave++;
        waveText.text = "Wave " + indexWave;
        yield return new WaitForSeconds(2);
        waveText.text = "";
    }

    public void UpdateArrowUI(Color colorArrow)
    {

        arrowUI.color = colorArrow;
    }

    public void Restart(){
        Time.timeScale = 1;
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame(){
        Time.timeScale = 1;
        SceneManager.LoadScene(menuScene);
    }

    public void Die(){
        Cursor.visible = true;
        gameOverPanel.SetActive(true);
    }

    public void Victory(){
        Cursor.visible = true;
        UIPanel.SetActive(false);
        victoryPanel.SetActive(true);
    }
}
