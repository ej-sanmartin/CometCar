using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public static GameManager gm;

  // handles score
  private int score = 0;
  private int maxScore = 9999; // this is max since 5 digit numbers breaks UI
  private int highScore;

  // references UI elements
	public TextMeshProUGUI UIScore;
  public Canvas GameOverScreen;

  public Text FinalScore;
  public Text FinalScoreOutline;

  public Text BestScore;
  public Text BestScoreOutline;

  // references button
  public Button RetryButton;
  public Button MenuButton;

  // references FadeOutEvent
  public GameObject _FadeOut;

  [HideInInspector]
  public bool gameIsOver = false;

  void Awake() {
    if(gm == null){
      gm = this.gameObject.GetComponent<GameManager>();
    }

    if(UIScore==null){
      Debug.LogError("Need to set UIScore on Game Manager.");
    }

    if(GameOverScreen==null){
      Debug.LogError("Need to set Game Screen Canvas on Game Manager.");
    }

    // init scoreboard to 0
    UIScore.text = "0";

    // inactivate the GameOverScreen gameObject, if it is set
    if (GameOverScreen){
      GameOverScreen.enabled = false;
    }

    // setup the listener to loadlevel when clicked
    RetryButton.onClick.RemoveAllListeners();
    RetryButton.onClick.AddListener(() => RetryGame());

    // setup the listener to loadlevel when clicked
    MenuButton.onClick.RemoveAllListeners();
    MenuButton.onClick.AddListener(() => ReturnToMenu());
  }

  public void AddPoints(){
    // only updates if game is not over and score isn't at max
    if(!gameIsOver || score == maxScore){
      score++;
      UIScore.text = score.ToString();
    }
    return;
  }

  // so not an Avengers reference :P
  public void EndGame(){
    gameIsOver = true;
    highScore = PlayerPrefManager.GetHighscore();
    if(score > highScore){
      PlayerPrefManager.SetHighscore(score);
    }
    highScore = PlayerPrefManager.GetHighscore();

    // Sets the scores and best score for Game Over UI
    FinalScore.text = score.ToString();
    FinalScoreOutline.text = score.ToString();

    BestScore.text = highScore.ToString();
    BestScoreOutline.text = highScore.ToString();

    StartCoroutine(LoadGameOverScreen());
  }

  // Retry game
  public void RetryGame(){
    ActivateFadeOut();
    StartCoroutine(LoadSceneWithFade("Game"));
  }

  // Return to menu Button
  public void ReturnToMenu(){
    ActivateFadeOut();
    StartCoroutine(LoadSceneWithFade("StartMenu"));
  }

  // Allows death animation to play
  IEnumerator LoadGameOverScreen(){
    yield return new WaitForSeconds(2f);
    UIScore.enabled = false;
    GameOverScreen.enabled = true;
  }

  // lets animation play out to transition to game
  IEnumerator LoadSceneWithFade(string selectedScene) {
    yield return new WaitForSeconds(1f);
    SceneManager.LoadScene(selectedScene);
  }

  // handles fade out event
  void ActivateFadeOut(){
    _FadeOut.SetActive(true); // initially set to false to avoid blocking raycast to start menu buttons
    _FadeOut.GetComponent<FadeOutEvent>().StartFade();
  }
}
