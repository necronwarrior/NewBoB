using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFactory : MonoBehaviour {

	public float[] SpawnTimes;
	public Object Bomber;
	public AudioClip[] CommandEnemyEast, CommandBomberEast;
	public System.Collections.BitArray SpawnOn;
	AudioSource FactoryCommand;

	// Use this for initialization
	void Start () {
		//SpawnOn = new bool[SpawnTimes.Length];
		SpawnOn = new System.Collections.BitArray(SpawnTimes.Length, true);
		FactoryCommand = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		for(int SpawnLevel = 0;SpawnLevel<SpawnTimes.Length;SpawnLevel++)
		{
			if(Time.timeSinceLevelLoad> SpawnTimes[SpawnLevel] 
				&& SpawnOn[SpawnLevel]==true){
			
				SpawnEnemies (SpawnLevel);
				SpawnOn [SpawnLevel] = false;
			}
		}
	}

	void SpawnEnemies(int Level)
	{
		switch (Level) {
		case 0: 
			SpawnBomber1 ();
			FactoryCommand.PlayOneShot (CommandBomberEast [Random.Range (0, 3)]);
			break;
		case 1: 
			SpawnBomber1();
			FactoryCommand.PlayOneShot (CommandEnemyEast [Random.Range (0, 4)]);
			break;
		case 2: 
			SpawnBomber1();
			FactoryCommand.PlayOneShot (CommandBomberEast [Random.Range (0, 3)]);
			break;
		default: 
			break;
		}
	}

	void SpawnBomber1()
	{
		GameObject NewBomb = (GameObject)Instantiate (Bomber);
		NewBomb.transform.position = transform.position;
		NewBomb.GetComponent<EnemyBomber> ().shotDistance = 15.0f;
	}
}
