using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public GameObject screenPause, screenGameOver, screenTapToStart;
    public Text score;
    public Text highScore;
    public Text textTimeScale;

    public static PlayerManager instance;

    public float scoreFloat;
    public float highScoreFloat;
    public Animator animHighScore;
    public GameObject addScore;
    public int life;
    public Image[] imgLife;


    public void Awake()
    {
        ResumeGame();
        instance = this;
        highScore.text = PlayerPrefs.GetFloat("HighScore", 0).ToString("N0");
        
    }

    public void ScoreAddPlus(float add){
        scoreFloat += add;
        addScore.GetComponent<Text>().text = "+" + add.ToString();
        addScore.GetComponent<Animator>().SetBool("Active", true);
    }

    void Update()
    {   
        if (Time.timeScale <= 2.5f) {
            if(Time.timeScale > 0)Time.timeScale += 0.0001f; 
            textTimeScale.text = Time.timeScale.ToString("N2") + "X";
        }

        switch (life){
            case 3:
                imgLife[3].enabled = false;
            break;

            case 2:
                imgLife[2].enabled = false;
            break;
                
            case 1:
                imgLife[1].enabled = false;
            break;

            case 0:
                imgLife[0].enabled = false;
            break;
        }

         if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale > 0 ){
            PauseGame();
        }

        if(Time.timeScale > 0 && scoreFloat < 9999f){
            scoreFloat += 0.1f;
            score.text = scoreFloat.ToString("N0");
            if(scoreFloat > PlayerPrefs.GetFloat("HighScore",0)){
                animHighScore.SetBool("Active", true);
                PlayerPrefs.SetFloat("HighScore", scoreFloat);
                highScore.text = scoreFloat.ToString("N0");
            }
        }

        if(life <=0 ){
            GameOver();
        }
    }

    public void GameOver(){
        if(screenGameOver != null) screenGameOver.SetActive(true);
        Time.timeScale = 0;
    }

    public void PauseGame(){
        if(screenPause != null) screenPause.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
        if(screenPause != null) screenPause.SetActive(false);
    }

    public void RestartGame(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void ShowItens(GameObject obj){
        obj.SetActive(true);
    }

    public void HideItens(GameObject obj){
        obj.SetActive(false);
    }

    public void OpenURL(string url){
        Application.OpenURL(url);
    }

}
