using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {
  public float startTime;
  public float minRandomTime;
  public float maxRandomTime;
  public GameObject meteor;

  private Transform _spawnPositon;
  private float _timeBetweenSpawns;
  private float _nextSpawnTime;

  void Awake(){
    _spawnPositon = GetComponent<Transform>();
  }

  void Start(){
    Invoke("Spawner", startTime);
    _nextSpawnTime = Time.time + Random.Range(minRandomTime, maxRandomTime);
  }

  void FixedUpdate(){
    if(Time.time >= _nextSpawnTime){
      Spawner();
      _timeBetweenSpawns = Random.Range(minRandomTime, maxRandomTime);
      _nextSpawnTime = Time.time+_timeBetweenSpawns;
    }
  }

  void Spawner(){
    GameObject newMeteor = Instantiate(meteor, _spawnPositon);
  }
}
