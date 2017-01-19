using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour {

	public GameObject Car;
	public static int generation = 0;

	private const int CARS_PER_GENERATION = 80;
	private const int CARS_PER_WAVE = 10;
	private GameObject[] cars = new GameObject[CARS_PER_GENERATION];
	private NeuralNetwork[] networks = new NeuralNetwork[CARS_PER_GENERATION];

	private static double[] savedWeights = {
		-0.114574757185139, 0.690965042628675, -0.132837045667993, -1.27502822058953, 
		0.614687931896148, -1.3453287299737, -0.334996958614533, 0.889074284426788, 
		0.313380140625974, -0.163171630643398, 0.28556890498923, 0.600863212604651, 
		0.419768901598595, -1.27532239802125, 0.611239584195259, 0.402861908854332, 
		0.729644361575872, -0.233052775348468, 0.185888222367897, 0.907421426023003,
		-1.1303466323171, 0.398494786327946, -1.09875406097873, 0.199720897327737, -0.234038548309601};

	private static double[] savedWeights2 = {
		-0.0414035696476598, 0.844568744720901, -0.13762663061447, -1.31792493156495,
		0.808181694784069, -1.22518283508646, -0.34057496997199, 1.18237763543126, 
		0.474470174292615, -0.0841976626296649, 0.162519137330374, 0.408575102314361, 
		0.380389262578336, -1.0899680730642, 0.761723898018308, 0.583999819058532,
		0.837167589916069, -0.256811273536703, -0.109667250684895, 1.1563934433257, 
		-1.49844971781231, 0.389009475870518, -1.31176237214435, 0.14291289865847, -0.299799939617929
	};

	private static double savedBias2 = -3.20560986098054;

	private static List<double> savedWeightsList = new List<double> (savedWeights2);

	// Use this for initialization
	void Start () {
		for (int i = 0; i < CARS_PER_GENERATION; i++) {
			networks[i] = new NeuralNetwork (5, 1, 4, 1);
			//networks [i].putWeights (savedWeightsList);
			//networks [i].hiddenLayers [0].neurons [4].output = savedBias2;
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


			string best = "";

			foreach (double weight in GeneticsHandler.getMaxFitnessNetwork(networks).getWeights()){
				best += weight + ", ";
			}

			Debug.Log (best);
			Debug.Log (GeneticsHandler.getMaxFitnessNetwork (networks).hiddenLayers [0].neurons [4].output);

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
			yield return new WaitForSeconds (1f);
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
