using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    GS_PAUSEMENU,
    GS_GAME,
    GS_GAME_OVER,
    GS_LEVELCOMPLETED,
    GS_OPTIONS
}
public class GameManager : MonoBehaviour {
    public GameState currentGameState;
    public static GameManager instance;
    public Canvas LevelCompletedCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas inGameCanvas;
    public Canvas gamoverCanvas;
    public Canvas optionsCanvas;
    public Text coinsText;
    public Text coinsText2;
    public Text timer_Game;
    public Text EnemyCount;
    public Text EnemyCount2;
    public Text Score;
    public Text HeartsText;
    public Text Distance;
    public Text HighScoreText;
    private int enemydeaths=0;
    private int coins = 0;
    public Image[] keysTab;
    public Image[] heartsTab;
    private int keys = 0;
    public int hearts = 0;
    public float timer = 0;
    public float enemymax;
    public float coinsmax;
    public float heartsmax;
    public float levelscore;
    public float playerscore;
    public Slider volumeSlider;


    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if (newGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if(currentScene.name == "Scena1")
            {
                playerscore = ((hearts * 21 + enemydeaths * 11 + coins * 6) - timer);
                if (PlayerPrefs.GetInt("HighscoreLevel1") < playerscore)
                {
                    
                    PlayerPrefs.SetInt("HighscoreLevel1", (int)playerscore);
                    HighScoreText.text = PlayerPrefs.GetInt("HighscoreLevel1").ToString();
                    playerscore = (int)playerscore;
                    Score.text = playerscore.ToString();
                }
                else
                {
                    HighScoreText.text = PlayerPrefs.GetInt("HighscoreLevel1").ToString();
                    playerscore = (int)playerscore;
                    Score.text = playerscore.ToString();
                }
            }
            else
            {
                if (currentScene.name == "Endless")
                {
                    playerscore = ((hearts * 21 + enemydeaths * 11 + coins * 6) - timer);
                    if (PlayerPrefs.GetInt("HighscoreLevel2") < playerscore)
                    {
                        PlayerPrefs.SetInt("HighscoreLevel2", (int)playerscore);
                        HighScoreText.text = PlayerPrefs.GetInt("HighscoreLevel2").ToString();
                        playerscore = (int)playerscore;
                        Score.text = playerscore.ToString();
                    }
                    else
                    {
                        HighScoreText.text = PlayerPrefs.GetInt("HighscoreLevel2").ToString();
                        playerscore = (int)playerscore;
                        Score.text = playerscore.ToString();
                    }
                }
            }
        }
        inGameCanvas.enabled = (newGameState == GameState.GS_GAME);
        pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);        LevelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
        gamoverCanvas.enabled = (currentGameState == GameState.GS_GAME_OVER);
        optionsCanvas.enabled = (currentGameState == GameState.GS_OPTIONS);
        inGameCanvas.gameObject.SetActive((currentGameState == GameState.GS_GAME));
        pauseMenuCanvas.gameObject.SetActive((currentGameState == GameState.GS_PAUSEMENU));
        gamoverCanvas.gameObject.SetActive((currentGameState == GameState.GS_GAME_OVER));
        LevelCompletedCanvas.gameObject.SetActive((currentGameState == GameState.GS_LEVELCOMPLETED));        optionsCanvas.gameObject.SetActive((currentGameState == GameState.GS_OPTIONS));
    }
    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
    public void onResumeButtonPressed()
    {
        InGame();
    }
    public void onRestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void onNextLevelButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void onExitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void onOptionsButtonPressed()
    {
        Options();
    }
    public void onMinusButtonPressed()
    {
        QualitySettings.DecreaseLevel(false);
        
    }
    public void onPlusButtonPressed()
    {
        QualitySettings.IncreaseLevel();
    }
    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
        Time.timeScale = 1;

    }
    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
        Time.timeScale = 0;
    }
    public void GameOver()
    {

        SetGameState(GameState.GS_GAME_OVER);
        Time.timeScale = 0;
    }
    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
        Time.timeScale = 0;
    }
    public void LevelCompleted()
    {
        
        SetGameState(GameState.GS_LEVELCOMPLETED);
        Time.timeScale = 0;
    }
    public void addCoins()
    {
        coins++;
        coinsText.text = coins.ToString();
        coinsText2.text = coins.ToString();
    }
    public void zombieDied()
    {
        enemydeaths++;
        EnemyCount.text = enemydeaths.ToString();
        EnemyCount2.text = enemydeaths.ToString();
    }
    public void addKeys()
    {
        keysTab[keys++].color = Color.white;
    }
    public void Hearts(bool plus)
    {
        if (plus)
        {
            hearts++;
            heartsTab[hearts].enabled = true;
            
            
        }
        else
        {
            heartsTab[hearts].enabled = false;
            if(hearts>0)
            hearts--;
            else
            
            GameOver();
        }
        
    }
   /* public void SumScore()
    {
        playerscore = ((hearts*21+ enemydeaths*11 + coins*6)-timer);
        playerscore = (int)playerscore;
        Score.text = playerscore.ToString();
    }*/
    public void GetMax()
    {
    enemymax= GameObject.FindGameObjectsWithTag("Enemy").Length;
    coinsmax= GameObject.FindGameObjectsWithTag("Coin").Length;
    heartsmax= GameObject.FindGameObjectsWithTag("Life").Length;
        levelscore = enemymax * 21 + coinsmax * 11 + heartsmax*6;
}
    // Use this for initialization
    void Awake()
    {
        SetVolume();
        instance = this;
        InGame();
        for (int i = 0; i < keysTab.Length; i++)
            keysTab[i].color = Color.grey;
        for (int i = 1; i < heartsTab.Length; i++)
            heartsTab[i].enabled = false;
        GetMax();
        if (!PlayerPrefs.HasKey("HighscoreLevel1"))
            PlayerPrefs.SetInt("HighscoreLevel1", 0);
        if (!PlayerPrefs.HasKey("HighscoreLevel2"))
            PlayerPrefs.SetInt("HighscoreLevel2", 0);
    }

    void Start ()
    {
        
    }
    private float secs;
    private float mins;
    
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        secs = Mathf.FloorToInt(timer % 60f);
        mins = Mathf.FloorToInt(timer / 60);
        
        timer_Game.text = string.Format("{0:00}:{1:00}", mins, secs);


        if (currentGameState == GameState.GS_PAUSEMENU && Input.GetKeyDown(KeyCode.Escape))
            InGame();
        else
           if (currentGameState == GameState.GS_GAME && Input.GetKeyDown(KeyCode.Escape))
            PauseMenu();
        HeartsText.text = (hearts + 1).ToString();
    }
}
