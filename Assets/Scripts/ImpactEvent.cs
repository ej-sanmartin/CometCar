using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEvent : MonoBehaviour {
  [Tooltip("Expects a particle effect")]
  public GameObject explosion;

  void OnCollisionEnter2D(Collision2D collided){
    gameObject.GetComponent<Rotate>().enabled = false;

    if (explosion){
      Instantiate(explosion,transform.position,transform.rotation);
    }

    // will kill player and end game
    if(collided.gameObject.tag == "Player"){
      collided.gameObject.GetComponent<PlayerContoller>().ImpactPlayer();
    }

    // will add score if meteor hits planet
    if(collided.gameObject.tag == "Planet"){
      collided.gameObject.GetComponent<Planet>().AddScore();
    }

    // does crude clean up
    // TODO: find more efficient way to clean up game after ever collision
    Object.Destroy(this.gameObject);
  }
}
