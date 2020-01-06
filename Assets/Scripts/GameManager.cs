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

  // variables that handle which car color spawns
  public GameObject[] selectableCars;
  public string _selectedCarString = "red";
  private Transform _carSpawnPositon;

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

  // SFX
  public AudioClip crashSFX;

  // references FadeOutEvent
  public GameObject _FadeOut;

  // handles audio
  AudioSource _audio;

  [HideInInspector]
  public bool gameIsOver = false;

  void Awake() {
    if(gm == null){
      gm = this.gameObject.GetComponent<GameManager>();
    }

    _carSpawnPositon = GetComponent<Transform>();

    _audio = GetComponent<AudioSource>();
    if (_audio==null) { // if AudioSource is missing
      Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
      // let's just add the AudioSource component dynamically
      _audio = gameObject.AddComponent<AudioSource>();
    }

    _selectedCarString = PlayerPrefManager.GetSelectedCar();
    SpawnSelectedCar(_selectedCarString);

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

    // setup the listener to Retry when clicked
    RetryButton.onClick.RemoveAllListeners();
    RetryButton.onClick.AddListener(() => LoadSceneAfterGameOverThroughButton("Game"));

    // setup the listener to ReeturnToMenu when clicked
    MenuButton.onClick.RemoveAllListeners();
    MenuButton.onClick.AddListener(() => LoadSceneAfterGameOverThroughButton("StartMenu"));
  }

  public void SpawnSelectedCar(string carColor){
    switch(carColor){
      case "red":
        SpawnCarFactory(0);
        break;
      case "green":
        SpawnCarFactory(1);
        break;
      case "orange":
        SpawnCarFactory(2);
        break;
      case "black":
        SpawnCarFactory(3);
        break;
      case "purple":
        SpawnCarFactory(4);
        break;
      case "white":
        SpawnCarFactory(5);
        break;
      case "blue":
        SpawnCarFactory(6);
        break;
      case "pink":
        SpawnCarFactory(7);
        break;
      case "yellow":
        SpawnCarFactory(8);
        break;
    }
  }

  public void SpawnCarFactory(int carNumber){
    Instantiate(selectableCars[carNumber], _carSpawnPositon);
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
    PlaySound(crashSFX);
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

  // changes scene during game over depending on button selected
  public void LoadSceneAfterGameOverThroughButton(string scene){
    ActivateFadeOut();
    StartCoroutine(LoadSceneWithFade(scene));
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
    _FadeOut.SetActive(true); // initially set to false to avoid blocking raycast
    _FadeOut.GetComponent<FadeOutEvent>().StartFade();
  }

  // play sound through the audiosource on the gameobject
  void PlaySound(AudioClip clip) {
    _audio.PlayOneShot(clip);
  }
}
