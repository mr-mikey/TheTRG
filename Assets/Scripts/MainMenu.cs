using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public Canvas CampaignMenuCanvas;
    public Canvas MainMenuCanvas;
    public Text highscoreLevel1Text;
    public Text highscoreLevel2Text;
    private IEnumerator StartGame(string levelName)
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene(levelName);
    }
    public void onCampaignButtonPressed()
    {
        MainMenuCanvas.enabled = false;
        CampaignMenuCanvas.enabled = true;
    }
    public void onLevel1ButtonPressed()
    {
        StartCoroutine(StartGame("Scena1"));
    }
    public void onEndlessButtonPressed()
    {
        StartCoroutine(StartGame("Endless"));
    }
    public void onExitButtonPressed()
    {
#if (UNITY_EDITOR)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
        Application.Quit();
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
    private void Awake()
    {
        if(!PlayerPrefs.HasKey("HighscoreLevel1"))
            PlayerPrefs.SetInt("HighscoreLevel1", 0);
        if (!PlayerPrefs.HasKey("HighscoreLevel2"))
            PlayerPrefs.SetInt("HighscoreLevel2", 0);
        highscoreLevel1Text.text = PlayerPrefs.GetInt("HighscoreLevel1").ToString();
        highscoreLevel2Text.text = PlayerPrefs.GetInt("HighscoreLevel2").ToString();

    } 
}
