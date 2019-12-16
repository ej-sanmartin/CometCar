using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
  [Tooltip("Speed of rotation in Z direction")]
  public int speed; // speed of rotation

    // Update is called once per frame
    void Update(){
      transform.Rotate(0, 0, speed * Time.deltaTime, Space.Self);
    }
}
