using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class PlayerPrefManager {
  public static void SetHighscore(int highscore){
    PlayerPrefs.SetInt("Highscore", highscore);
  }

  public static int GetHighscore() {
    if (PlayerPrefs.HasKey("Highscore")) {
      return PlayerPrefs.GetInt("Highscore");
    } else {
      return 0;
    }
  }
}
