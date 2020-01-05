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

  public static void SetSelectedCar(string carColor){
    PlayerPrefs.SetString("SelectedCar", carColor);
  }

  public static string GetSelectedCar(){
    if(PlayerPrefs.HasKey("SelectedCar")){
      return PlayerPrefs.GetString("SelectedCar");
    } else {
      return "red"; // if no car was selected, selected string will return default color, red.
    }
  }
}
