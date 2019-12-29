using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerContoller : MonoBehaviour {
  // set at 1 since player can't survive a hit from a meteor
  private int playerHealth = 1;
  public GameObject playerExplosion;

  // player controls and other variables that handle player movement via touch control
  [Range(0.0f, 10.0f)] // create a slider in the editor and set limits on moveSpeed
  public float moveSpeed = 3f;
  private Vector3 touchPosition;
  private Rigidbody2D rb;
  private Vector3 direction;

  // handles sprite rotation along planet surface
  RaycastHit2D hitOnPlanet;

  // handles restraining movement to surface of planet
  public GameObject planet;
  float radius = 1.2f;

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
    rb = GetComponent<Rigidbody2D>();
    if(rb==null) { // if AudioSource is missing
      Debug.LogWarning("Rigidbody2D component missing from this gameobject. Adding one.");
      // let's just add the AudioSource component dynamically
      rb = gameObject.AddComponent<Rigidbody2D>();
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
      touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
      touchPosition.z = 0;
      direction = (touchPosition - transform.position);
      rb.velocity = new Vector2(direction.x, direction.y) * moveSpeed;

      if(touch.phase == TouchPhase.Ended){
        rb.velocity = Vector2.zero;
      }
    }

    // handles rotating sprite along planet
    hitOnPlanet = Physics2D.Raycast(_rectTransform.anchoredPosition, -Vector2.up, 1f);
    _rectTransform.rotation = Quaternion.Lerp(_rectTransform.rotation, Quaternion.FromToRotation(_rectTransform.anchoredPosition, hitOnPlanet.normal), Time.deltaTime);

    // handles constrained movement along planet. Without it, player will follow touch and break laws of physics
    Vector2 offset = _rectTransform.anchoredPosition - (Vector2)planet.transform.position;
    offset.Normalize();
    offset = offset * radius;
    _rectTransform.anchoredPosition = offset;
  }

  // play sound through the audiosource on the gameobject
  void PlaySound(AudioClip clip) {
    _audio.PlayOneShot(clip);
  }

  void FreezeMotion() {
    playerCanMove = false;
        rb.velocity = new Vector2(0,0);
    rb.isKinematic = true;
  }

  // public function to kill the player when they have a fall death
  public void ImpactPlayer () {
    if (playerCanMove) {
      playerHealth = 0;
      PlaySound(crashSFX);
      StartCoroutine (KillPlayer ());
    }
  }

  // this code runs when meteor hits player
  IEnumerator KillPlayer(){
    if(playerCanMove){
      FreezeMotion();
      Instantiate(playerExplosion,transform.position,transform.rotation);

      // After waiting tell the GameManager to reset the game
			yield return new WaitForSeconds(3.0f);
    }
  }
}
