using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
  public static GameManager gm;

  private int score = 0;
	public TextMeshProUGUI UIScore;

  // public GameObject gameOverScoreOutline;

  // public AudioSource musicAudioSource;

  [HideInInspector]
  public bool gameIsOver = false;

  // public GameObject playAgainButtons;
  //public string playAgainLevelToLoad;

  void Awake() {
    if (gm == null){
      gm = this.gameObject.GetComponent<GameManager>();
    }

    if (UIScore==null){
      Debug.LogError ("Need to set UIScore on Game Manager.");
    }

    // init scoreboard to 0
    UIScore.text = "0";

    // inactivate the gameOverScoreOutline gameObject, if it is set
    // if (gameOverScoreOutline){
    //  gameOverScoreOutline.SetActive (false);
    //}
  }

  void Update(){

  }

  public void AddPoints(){
    score++;
    UIScore.text = score.ToString();
  }
}
