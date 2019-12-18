using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEvent : MonoBehaviour {
  [Tooltip("Expects a particle effect")]
  public GameObject explosion;
  public int damage = 1;

  void OnCollisionEnter2D(Collision2D other){
    gameObject.GetComponent<Rotate>().enabled = false;

    if (explosion){
      Instantiate(explosion,transform.position,transform.rotation);
    }

    // if(other.tag == "player"){
    //  other.gameObject.GetComponent<CharacterController>().killCar(damage);
    //}

    // if(other.tag == "planet"){
    //  other.gameObject.GetComponent<CharacterController>().AddScore();
    //}

    other.gameObject.GetComponent<AudioSource>().Play();

    Object.Destroy(this.gameObject);
  }
}
