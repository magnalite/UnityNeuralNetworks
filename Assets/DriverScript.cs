using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DriverScript : MonoBehaviour {

	public NeuralNetwork network;
	private NeuralNetwork steerNetwork;

	private Rigidbody2D rigidBody;
	private Vector2 leftCast;
	private Vector2 frontLeftCast;
	private Vector2 frontCast;
	private Vector2 frontRightCast;
	private Vector2	rightCast;
	private int lastCheckpoint;
	private Transform lastLocation;

	private static double[] savedWeights2 = {
		-0.0414035696476598, 0.844568744720901, -0.13762663061447, -1.31792493156495,
		0.808181694784069, -1.22518283508646, -0.34057496997199, 1.18237763543126, 
		0.474470174292615, -0.0841976626296649, 0.162519137330374, 0.408575102314361, 
		0.380389262578336, -1.0899680730642, 0.761723898018308, 0.583999819058532,
		0.837167589916069, -0.256811273536703, -0.109667250684895, 1.1563934433257, 
		-1.49844971781231, 0.389009475870518, -1.31176237214435, 0.14291289865847, -0.299799939617929
	};

	private static double savedBias2 = -3.20560986098054;

	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		leftCast =       new Vector2 (-1, 0);
		frontLeftCast =  new Vector2 (-1, 1).normalized;
		frontCast =      new Vector2 (0, 1);
		frontRightCast = new Vector2 (1, 1).normalized;
		rightCast =      new Vector2 (1, 0);
		lastLocation =   gameObject.transform;
		steerNetwork =   new NeuralNetwork (5, 1, 4, 1);
		steerNetwork.putWeights (new List<double> (savedWeights2));
		steerNetwork.hiddenLayers [0].neurons [4].output = savedBias2;

		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Cars"), LayerMask.NameToLayer ("Cars"));
	}

	void OnTriggerEnter2D(Collider2D other){
		if (int.Parse (other.gameObject.name.Substring (11)) > lastCheckpoint) {
			lastCheckpoint = int.Parse (other.gameObject.name.Substring (11));
			network.fitness += 10;
			lastLocation = other.gameObject.transform;
		} else if (int.Parse (other.gameObject.name.Substring (11)) == 0 && lastCheckpoint == 13) {
			lastCheckpoint = 0;
			network.fitness += 10;
			lastLocation = other.gameObject.transform;
		}
	}

	void OnCollisionStay2D(Collision2D other){
		network.fitness *= 0.999;
		//gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		//network.fitness += (gameObject.transform.position - lastLocation.position).magnitude;
	}
		

	// Update is called once per frame
	void FixedUpdate () {
		if (gameObject.GetComponent<Rigidbody2D> ().bodyType == RigidbodyType2D.Dynamic) {
			if (Input.GetKey ("w")) {
				//rigidBody.velocity += rigidBody.GetRelativeVector(new Vector2 (0.0f, 0.3f));
			}

			if (Input.GetKey ("s")) {
				//rigidBody.velocity -= rigidBody.GetRelativeVector(new Vector2 (0.0f, 0.3f));
			}

			if (Input.GetKey ("d")) {
				//rigidBody.angularVelocity -= 30;
			}

			if (Input.GetKey ("a")) {
				//rigidBody.angularVelocity += 30;
			}

			RaycastHit2D cast = Physics2D.Raycast (rigidBody.position, rigidBody.GetRelativeVector (frontCast), 100, 1 << LayerMask.NameToLayer ("Course")); 
			RaycastHit2D cast1 = Physics2D.Raycast (rigidBody.position, rigidBody.GetRelativeVector (leftCast), 100, 1 << LayerMask.NameToLayer ("Course")); 
			RaycastHit2D cast2 = Physics2D.Raycast (rigidBody.position, rigidBody.GetRelativeVector (rightCast), 100, 1 << LayerMask.NameToLayer ("Course")); 
			RaycastHit2D cast3 = Physics2D.Raycast (rigidBody.position, rigidBody.GetRelativeVector (frontLeftCast), 100, 1 << LayerMask.NameToLayer ("Course")); 
			RaycastHit2D cast4 = Physics2D.Raycast (rigidBody.position, rigidBody.GetRelativeVector (frontRightCast), 100, 1 << LayerMask.NameToLayer ("Course")); 

			if (Selection.Contains (gameObject)) {
				Debug.DrawRay (rigidBody.position, cast.point - rigidBody.position, Color.red, 0.0f, false);
				Debug.DrawRay (rigidBody.position, cast1.point - rigidBody.position, Color.blue, 0.0f, false);
				Debug.DrawRay (rigidBody.position, cast2.point - rigidBody.position, Color.green, 0.0f, false);	
				Debug.DrawRay (rigidBody.position, cast3.point - rigidBody.position, Color.magenta, 0.0f, false);
				Debug.DrawRay (rigidBody.position, cast4.point - rigidBody.position, Color.yellow, 0.0f, false);
			}

			network.inputLayer.neurons [0].output = cast.distance;
			network.inputLayer.neurons [1].output = cast1.distance;
			network.inputLayer.neurons [2].output = cast2.distance;
			network.inputLayer.neurons [3].output = cast3.distance;
			network.inputLayer.neurons [4].output = cast4.distance;

			steerNetwork.inputLayer.neurons [0].output = cast.distance;
			steerNetwork.inputLayer.neurons [1].output = cast1.distance;
			steerNetwork.inputLayer.neurons [2].output = cast2.distance;
			steerNetwork.inputLayer.neurons [3].output = cast3.distance;
			steerNetwork.inputLayer.neurons [4].output = cast4.distance;

			//rigidBody.velocity += rigidBody.GetRelativeVector (new Vector2 (0.0f, (float) network.outputLayer.neurons[0].CalculateOutput()));
			rigidBody.velocity += rigidBody.GetRelativeVector (new Vector2 (0.0f, 0.3f));
			rigidBody.angularVelocity += ((float)steerNetwork.outputLayer.neurons [0].CalculateOutput () * 180) - 90;
			//rigidBody.angularVelocity += ((float)network.outputLayer.neurons [0].CalculateOutput () * 180) - 90;
		}
	}

	void OnGUI(){
		if (Selection.Contains (gameObject)) {
			GUI.skin = null;


			GUI.Box (new Rect (10, 0, 200, 20), "Fitness: " + network.fitness);

			for (int i = 0; i < network.inputLayer.neurons.Length; i++) {
				GUI.Box (new Rect (10, 40 * i + 20, 200, 20), "Input: " + network.inputLayer.neurons [i].output);
			}

			for (int layer = 0; layer < network.hiddenLayers.Length; layer++) {
				for (int i = 0; i < network.hiddenLayers [layer].neurons.Length; i++) {
					if (i != network.hiddenLayers [layer].neurons.Length - 1) {
						GUI.Box (new Rect (250 + 250 * layer, 40 * i + 20, 200, 20), "Hidden: " + network.hiddenLayers [layer].neurons [i].output);
					} else {
						GUI.Box (new Rect (250 + 250 * layer, 40 * i + 20, 200, 20), "Bias: " + network.hiddenLayers [layer].neurons [i].output);
					}
				}
			}

			for (int i = 0; i < network.outputLayer.neurons.Length; i++) {
				GUI.Box (new Rect (250 + 250 * network.hiddenLayers.Length, 40 * i + 20, 200, 20), "Ouput: " + network.outputLayer.neurons [i].output);
			}
		}
	}
}
