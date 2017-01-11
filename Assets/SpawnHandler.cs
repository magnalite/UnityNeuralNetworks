using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour {

	public GameObject Car;
	public static int generation = 0;

	private const int CARS_PER_GENERATION = 80;
	private const int CARS_PER_WAVE = 40;
	private GameObject[] cars = new GameObject[CARS_PER_GENERATION];
	private NeuralNetwork[] networks = new NeuralNetwork[CARS_PER_GENERATION];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < CARS_PER_GENERATION; i++) {
			networks[i] = new NeuralNetwork (5, 1, 4, 1);
		}

		StartCoroutine (StartGeneration ());
	}

	void OnGUI(){
		GUI.color = new Color (0.0f, 0.0f, 0.0f, 1.0f);
		GUI.Box (new Rect (10, 0, 200, 20), "Generation: " + generation);
	}

	IEnumerator StartGeneration(){

		if (generation > 0) {
			Debug.Log ("Generation " + generation +
			" max fitness : " + GeneticsHandler.getMaxFitness (networks)
			+ " avg fitness : " + GeneticsHandler.getAverageFitness (networks));

			networks = GeneticsHandler.breedNetworks (networks, CARS_PER_GENERATION);
		}
		generation++;

		yield return StartCoroutine(StartWave (0));
	}

	IEnumerator StartWave(int startNum){
		yield return new WaitForSeconds (2);

		for (int i = 0; i < CARS_PER_WAVE; i++) {
			cars[startNum + i] = Instantiate (Car);
			cars [startNum + i].GetComponent<DriverScript> ().network = networks [startNum + i];
			networks [startNum + i].fitness = 0;
			yield return new WaitForSeconds (0.01f);
		}

		yield return new WaitForSeconds (10 + (2 * generation));

		for (int i = 0; i < CARS_PER_WAVE; i++) {
			networks[startNum + i] = cars [startNum + i].GetComponent<DriverScript> ().network;
			Destroy (cars [startNum + i]);
		}

		if (startNum + CARS_PER_WAVE < CARS_PER_GENERATION) {
			StartCoroutine (StartWave (startNum + CARS_PER_WAVE));
		} else {
			StartCoroutine (StartGeneration());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
