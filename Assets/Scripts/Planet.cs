using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
  // SFX
  public AudioClip crashSFX;

  AudioSource _audio;

  void Awake(){
    _audio = GetComponent<AudioSource>();
    if (_audio==null) { // if AudioSource is missing
      Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
      // let's just add the AudioSource component dynamically
      _audio = gameObject.AddComponent<AudioSource>();
    }
  }

  void PlaySound(AudioClip clip){
    _audio.PlayOneShot(clip);
  }

  // plays collision sound from being hit by a meteor and calls add score function
  public void AddScore(){
    PlaySound(crashSFX);
  }
}
