using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPersist : MonoBehaviour{
  private AudioSource _backgroundMusic;

  public void Awake(){
    _backgroundMusic = GetComponent<AudioSource>();
    if(_backgroundMusic==null){
      Debug.LogError("Background music not attatched. Please add one.");
    }
    _backgroundMusic.Play();
    DontDestroyOnLoad(this.gameObject);
  }
}
