using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

  // references to Submenus
  public GameObject _MainMenu;
  public GameObject _CarsSelectMenu;

  // references to Button GameObjects
	public Button startButton;
	public Button carsButton;

  // references FadeOutEvent
  public GameObject _FadeOut;

  void Awake(){
    ShowMenu("Main");

    // setup the listener to loadlevel when clicked
    startButton.onClick.RemoveAllListeners();
    startButton.onClick.AddListener(() => StartGame());
  }

  public void ShowMenu(string menuName){
    // all menus off by default until selected
    _MainMenu.SetActive(false);
    _CarsSelectMenu.SetActive(false);

    // switches to menu that is selected
    switch(menuName){
      case "Main":
        _MainMenu.SetActive(true);
        break;
      case "Cars Select":
        _CarsSelectMenu.SetActive(true);
        break;
    }
  }

  // Starts game
  public void StartGame(){
    _FadeOut.SetActive(true); // initially set to false to avoid blocking raycast to start menu buttons
    _FadeOut.GetComponent<FadeOutEvent>().StartFade();
    StartCoroutine(LoadGameWithFade());
  }

  // lets animation play out to transition to game
  IEnumerator LoadGameWithFade() {
    yield return new WaitForSeconds(1f);
    SceneManager.LoadScene("Game");
  }
}
