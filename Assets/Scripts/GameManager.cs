using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int Level = 1;
    [HideInInspector] public int LevelLimit = 256;
    [HideInInspector] public float TotalPoint;
    int Point = 20;
    int lines;
    int highScore;

    public InputActionReference PauseButton;

    [HideInInspector] public float ShapeSpeed = 0.5f;

    UIManager uiManager;
    SpawnManager spawnManager;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        spawnManager = FindObjectOfType<SpawnManager>();

        highScore = PlayerPrefs.GetInt("HighScore");
        if (uiManager.HighScoreText)
        {
            uiManager.HighScoreText.text = highScore.ToString();
        }
    }

    void Update()
    {

    }

    private void OnEnable()
    {
        PauseButton.action.performed += PauseButtonPressEvent; // Yeni InputSystem e g�re yap�lan bir action atamas�.
                                                               // Controls ad�nda bir InputSystem var. Bunun i�inden
                                                               // TetrisControl ActionMap'i olu�turdum ve burada
                                                               // PauseButton ad�nda tu� atamas� yapt�m. InputActionReference ile bu atamay� �ekiyoruz.
    }

    private void OnDisable()
    {
        PauseButton.action.performed -= PauseButtonPressEvent;
    }

    public void AddPoint(int multiplier)
    {
        // point katlanarak arts�n diye (0.5f + multiplier / 2) ekliyorum.
        // Point 20 olursa : 20, 50, 120, 200 �eklinde olucak
        TotalPoint += Point * multiplier * (0.5f + (float)multiplier / 2);

        uiManager.TotalPointText.text = "Total Point : " + TotalPoint.ToString();

        lines += multiplier; //multipier ile rowDeleteCounter verisini yani ka� sat�r silindi�i verisini al�yoruz.
        uiManager.LinesText.text = "Lines : " + lines.ToString();
        LevelUP();
    }

    void LevelUP()
    {
        if(TotalPoint >= LevelLimit)
        {
            Level++;
            uiManager.LevelText.text = "Level : " + Level.ToString();

            if (ShapeSpeed > 0.1f)
            {
                ShapeSpeed -= 0.075f;
            }
            else if(ShapeSpeed > 0.01f)
            {
                ShapeSpeed -= 0.01f;
            }
            LevelLimit = (int)(LevelLimit * 1.5f); 
        }
    }

    void AddLines()
    {
        lines++;
        uiManager.LinesText.text = "Lines : " + lines.ToString();
    }
    public void GamePaused()
    {
        if(Time.timeScale == 1)
        {
            uiManager.PausePanel.SetActive(true);
            Time.timeScale = 0;
            spawnManager.GetOnBoardShape().GetComponent<TileManager>().enabled = false;
        }
        else 
        {
            Time.timeScale = 1;
            uiManager.PausePanel.SetActive(false);
            spawnManager.GetOnBoardShape().GetComponent<TileManager>().enabled = true;
        }
    }

    public void GameOver()
    {
        uiManager.GameOverPanel.SetActive(true);
        uiManager.GameOverScoreText.text = "Score : " + TotalPoint.ToString();

        if (PauseButton)
        {
            PauseButton.action.performed -= PauseButtonPressEvent; //�ld�kten sonra space tu�undan event almaya devam ediyordu
                                                                   //Bu y�zden �l�nmce de Pause button event'i Action atamas�nda ��kar�yoruz. 
        }

        HighScoreGetAndUpdate();

    }

    private void PauseButtonPressEvent(InputAction.CallbackContext context)
    {
        GamePaused();
    }

    void HighScoreGetAndUpdate()
    {
        if (TotalPoint > highScore)
        {
            highScore = (int)TotalPoint;
            PlayerPrefs.SetInt("HighScore", highScore);
            uiManager.NewRecordText.gameObject.SetActive(true);
        }
    }

    public void PlayGameButton()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void ExitButton()
    {
        Application.Quit();
    }

}
