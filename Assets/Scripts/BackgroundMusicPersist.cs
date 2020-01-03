using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPersist : MonoBehaviour{
  private AudioSource _backgroundMusic;

  public void Awake(){
    _backgroundMusic = GetComponent<AudioSource>();
    int numberOfBackGroundMusicObjects = FindObjectsOfType<BackgroundMusicPersist>().Length;

    // checks if object already exists in game so it doesn't duplicate background music
    if(numberOfBackGroundMusicObjects != 1){
      Destroy(this.gameObject);
    } else {
      _backgroundMusic.Play();
      DontDestroyOnLoad(this.gameObject);
    }
  }
}
