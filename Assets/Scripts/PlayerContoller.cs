using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour {
  // set at 1 since player can't survive a hit from a meteor
  private int playerHealth = 1;

  // player controls
  [Range(0.0f, 10.0f)] // create a slider in the editor and set limits on moveSpeed
  public float moveSpeed = 3f;

  // player can move?
  // we want this public so other scripts can access it but we don't want to show in editor as it might confuse designer
  [HideInInspector]
  public bool playerCanMove = true;

  // SFX
  public AudioClip drivingSound;
  public AudioClip crashSound;

  	// store references to components on the gameObject
  Rigidbody2D _rigidbody;
  Animator _animator;
  AudioSource _audio;

  // player tracking
  bool facingRight = true;

  // store the layer the player is on (setup in Awake)
  int _playerLayer;

  // number of layer that Platforms are on (setup in Awake)
  int _platformLayer;

  void Awake(){

  }

  void Update(){
    // exit update if player cannot move or game is paused
    if (!playerCanMove || (Time.timeScale == 0f)){
      return;
    }

  }
}
