using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public string sceneName;

    public GameObject menuPanel;
    public GameObject optionsPanel;
    public Text selectedDifficultyText;
    public Text wavesText;

    public int numberWavesEasy;
    public int numberWavesNormal;
    public int numberWavesHard;

    private int difficulty; // 1 - Easy; 2 - Normal; 3 - Hard

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OptionsButton()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        GetDifficulty();
        SetLegend();
    }

    public void MenuButton()
    {
        menuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void EasyButton()
    {
        GetDifficulty();
        difficulty--;
        if (difficulty < 1) difficulty = 1;
        SetDifficulty();

    }

    public void HardButton()
    {
        GetDifficulty();
        difficulty++;
        if (difficulty > 3) difficulty = 3;
        SetDifficulty();
    }

    void GetDifficulty()
    {
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            difficulty = PlayerPrefs.GetInt("Difficulty");
        }
        else
        {
            difficulty = 2;
        }
    }

    public void SetDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
        SetLegend();
    }

    void SetLegend()
    {
        switch (difficulty)
        {
            case 1: selectedDifficultyText.text = "Easy";
                selectedDifficultyText.color = new Color(0, 1, 0, 1);
                wavesText.text = numberWavesEasy + " waves";
                break;
            case 2: selectedDifficultyText.text = "Normal";
                selectedDifficultyText.color = new Color(1, 1, 0, 1);
                wavesText.text = numberWavesNormal + " waves";
                break;
            case 3: selectedDifficultyText.text = "Hard";
                selectedDifficultyText.color = new Color(1, 0, 0, 1);
                wavesText.text = numberWavesHard + " waves, new monsters!";
                break;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame(){

        if (Application.isEditor){
            //UnityEditor.EditorApplication.isPlaying = false;
        } else {
            Application.Quit();
        }
    }
}
