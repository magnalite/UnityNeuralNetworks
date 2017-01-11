using UnityEngine;
//using UnityEditor;
using System.Collections;

public class DriverScript : MonoBehaviour {

	public NeuralNetwork network;

	private Rigidbody2D rigidBody;
	private Vector2 leftCast;
	private Vector2 frontLeftCast;
	private Vector2 frontCast;
	private Vector2 frontRightCast;
	private Vector2	rightCast;

	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		leftCast =       new Vector2 (-1, 0);
		frontLeftCast =  new Vector2 (-1, 1).normalized;
		frontCast =      new Vector2 (0, 1);
		frontRightCast = new Vector2 (1, 1).normalized;
		rightCast =      new Vector2 (1, 0);
		//network = new NeuralNetwork (5, 1, 2, 2);

		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Cars"), LayerMask.NameToLayer ("Cars"));
	}

	void OnTriggerEnter2D(Collider2D other){
		network.fitness += 10;
	}

	void OnCollisionStay2D(Collision2D other){
		network.fitness *= 0.9;
	}
		

	// Update is called once per frame
	void FixedUpdate () {
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

		RaycastHit2D cast = Physics2D.Raycast(rigidBody.position, rigidBody.GetRelativeVector(frontCast), 100, 1 << LayerMask.NameToLayer("Course")); 
		RaycastHit2D cast1 = Physics2D.Raycast(rigidBody.position, rigidBody.GetRelativeVector(leftCast), 100, 1 << LayerMask.NameToLayer("Course")); 
		RaycastHit2D cast2 = Physics2D.Raycast(rigidBody.position, rigidBody.GetRelativeVector(rightCast), 100, 1 << LayerMask.NameToLayer("Course")); 
		RaycastHit2D cast3 = Physics2D.Raycast(rigidBody.position, rigidBody.GetRelativeVector(frontLeftCast), 100, 1 << LayerMask.NameToLayer("Course")); 
		RaycastHit2D cast4 = Physics2D.Raycast(rigidBody.position, rigidBody.GetRelativeVector(frontRightCast), 100, 1 << LayerMask.NameToLayer("Course")); 

		/*if (Selection.Contains (gameObject)) {
			Debug.DrawRay (rigidBody.position, cast.point - rigidBody.position, Color.red, 0.0f, false);
			Debug.DrawRay (rigidBody.position, cast1.point - rigidBody.position, Color.blue, 0.0f, false);
			Debug.DrawRay (rigidBody.position, cast2.point - rigidBody.position, Color.green, 0.0f, false);	
			Debug.DrawRay (rigidBody.position, cast3.point - rigidBody.position, Color.magenta, 0.0f, false);
			Debug.DrawRay (rigidBody.position, cast4.point - rigidBody.position, Color.yellow, 0.0f, false);
		}*/

		network.inputLayer.neurons[0].output = cast.distance;
		network.inputLayer.neurons[1].output = cast1.distance;
		network.inputLayer.neurons[2].output = cast2.distance;
		network.inputLayer.neurons[3].output = cast3.distance;
		network.inputLayer.neurons[4].output = cast4.distance;

		rigidBody.velocity += rigidBody.GetRelativeVector(new Vector2 (0.0f, 0.1f));
		rigidBody.angularVelocity += ((float) network.outputLayer.neurons [0].CalculateOutput () * 60) - 30;
	}

	void OnGUI(){
		/*if (Selection.Contains (gameObject)) {
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
		}*/
	}
}
