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
  public Button backButton;

  // list the selectable cars
	public Button[] carNames;

  // UI for best score and a reference to the best score int
  // also references needed scores to unlock cars
  public Text UIBestScore;
  public Text UIBestScore_Outline;

  private int bestScore;
  [HideInInspector]
  public Text UIScoreRequirement;
  [HideInInspector]
  public int UIScoreRequirement_ToCompare;
  [HideInInspector]
  public Text UIScoreRequirement_Outline;
  [HideInInspector]
  public int UIScoreRequirement_Outline_ToCompare;

  // references FadeOutEvent
  public GameObject _FadeOut;

  void Awake(){
    setupCarSelect();

    ShowMenu("Main");

    // setup the listener to StartGame when clicked
    startButton.onClick.RemoveAllListeners();
    startButton.onClick.AddListener(() => StartGame());

    // setup the listener to Show Car Select Screen when clicked
    carsButton.onClick.RemoveAllListeners();
    carsButton.onClick.AddListener(() => ShowMenu("CarSelect"));

    // setup the listener to Show Main Menu screen when clicked
    backButton.onClick.RemoveAllListeners();
    backButton.onClick.AddListener(() => ShowMenu("Main"));
  }

  void setupCarSelect(){
    _CarsSelectMenu.SetActive(true);

    bestScore = PlayerPrefManager.GetHighscore();

    UIBestScore.text = bestScore.ToString();
    UIBestScore_Outline.text = bestScore.ToString();

    for(int i = 0; i < carNames.Length; i++){
      Button carCard = carNames[i];

      if(carCard.transform.FindDeepChild("ScoreNeededToUnlock")){
        UIScoreRequirement = carCard.transform.FindDeepChild("ScoreNeededToUnlock").GetComponent<Text>();
        UIScoreRequirement_ToCompare = int.Parse(UIScoreRequirement.text);

        UIScoreRequirement_Outline = carCard.transform.FindDeepChild("ScoreNeededToUnlock_Outline").GetComponent<Text>();
        UIScoreRequirement_Outline_ToCompare = int.Parse(UIScoreRequirement_Outline.text);
      }

      string selectedCarColor = carCard.transform.FindDeepChild("SelectedString").GetComponent<Text>().text;

      // setup the listener to set the string for the selected color car from PlayerPrefsManager.cs when clicked
      carCard.onClick.RemoveAllListeners();
      carCard.onClick.AddListener(() => CarColorButton(selectedCarColor));

      if(bestScore >= UIScoreRequirement_ToCompare){
        carCard.GetComponent<Button>().interactable = true;
        if(i != 0){
          UIScoreRequirement.enabled = false;
          UIScoreRequirement_Outline.enabled = false;
        }

      } else {
        carCard.GetComponent<Button>().interactable = false;
        carCard.GetComponent<Image>().color = Color.black;
      }
    }
  }

  public void CarColorButton(string carColorToSetInPlayerPref){
    PlayerPrefManager.SetSelectedCar(carColorToSetInPlayerPref);
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
      case "CarSelect":
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
