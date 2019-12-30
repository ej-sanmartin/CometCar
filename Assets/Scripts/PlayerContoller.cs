using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerContoller : MonoBehaviour {
  public GameObject playerExplosion;

  // player controls and other variables that handle player movement via touch control
  [Range(0.0f, 10.0f)] // create a slider in the editor and set limits on moveSpeed
  public float moveSpeed = 3f;
  Vector3 _touchPosition;
  Rigidbody2D _rb;
  Vector3 _direction;

  // handles sprite rotation along planet surface
  RaycastHit2D _hitOnPlanet;

  // handles restraining movement to surface of planet
  public GameObject planet;
  private float _radius = 1.2f;

  // player can move?
  // we want this public so other scripts can access it but we don't want to show in editor as it might confuse designer
  [HideInInspector]
  public bool playerCanMove = true;

  // SFX
  public AudioClip crashSFX;

  	// store references to components on the gameObject
  RectTransform _rectTransform;
  Animator _animator;
  AudioSource _audio;

  // player tracking
  bool facingRight = true;

  // store the layer the player is on (setup in Awake)
  int _playerLayer;

  // number of layer that Platforms are on (setup in Awake)
  int _platformLayer;

  void Awake(){
    _rb = GetComponent<Rigidbody2D>();
    if(_rb==null) { // if AudioSource is missing
      Debug.LogWarning("Rigidbody2D component missing from this gameobject. Adding one.");
      // let's just add the AudioSource component dynamically
      _rb = gameObject.AddComponent<Rigidbody2D>();
    }

    _audio = GetComponent<AudioSource>();
    if (_audio==null) { // if AudioSource is missing
      Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
      // let's just add the AudioSource component dynamically
      _audio = gameObject.AddComponent<AudioSource>();
    }

    _rectTransform = GetComponent<RectTransform>();
    if(_rectTransform==null){
      Debug.LogWarning("Rect Transform component is missing from this game object. Adding one.");
      _rectTransform = gameObject.AddComponent<RectTransform>();
    }
  }

  void Update(){
    // exit update if player cannot move or game is paused
    if (!playerCanMove || (Time.timeScale == 0f)){
      return;
    }

    // handles touch movement
    if(Input.touchCount > 0){
      Touch touch = Input.GetTouch(0);
      _touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
      _touchPosition.z = 0;
      _direction = (_touchPosition - transform.position);
      _rb.velocity = new Vector2(_direction.x, _direction.y) * moveSpeed;

      if(touch.phase == TouchPhase.Ended){
        _rb.velocity = Vector2.zero;
      }
    }

    // handles rotating sprite along planet
    _hitOnPlanet = Physics2D.Raycast(_rectTransform.anchoredPosition, -Vector2.up, 1f);
    _rectTransform.rotation = Quaternion.Lerp(_rectTransform.rotation, Quaternion.FromToRotation(_rectTransform.anchoredPosition, _hitOnPlanet.normal), Time.deltaTime);

    // handles constrained movement along planet. Without it, player will follow touch and break laws of physics
    Vector2 offset = _rectTransform.anchoredPosition - (Vector2)planet.transform.position;
    offset.Normalize();
    offset = offset * _radius;
    _rectTransform.anchoredPosition = offset;
  }

  // play sound through the audiosource on the gameobject
  void PlaySound(AudioClip clip) {
    _audio.PlayOneShot(clip);
  }

  void FreezeMotion() {
    playerCanMove = false;
        _rb.velocity = new Vector2(0,0);
    _rb.isKinematic = true;
  }

  // public function to kill the player when they have a fall death
  public void ImpactPlayer () {
    if (playerCanMove) {
      PlaySound(crashSFX);
      StartCoroutine (KillPlayer ());
    }
  }

  // this code runs when meteor hits player
  IEnumerator KillPlayer(){
    if(playerCanMove){
      FreezeMotion();
      Instantiate(playerExplosion,transform.position,transform.rotation);
      Object.Destroy(this.gameObject);

      // After waiting tell the GameManager to reset the game
			yield return new WaitForSeconds(3.0f);
    }
  }
}
