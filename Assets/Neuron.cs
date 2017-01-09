using UnityEngine;
using System.Collections;

public class Neuron
{
	public double output;

	private Neuron[] inputNodes;
	private double[] inputWeights;

	public Neuron (Neuron[] inputs)
	{
		if (inputs != null) {
			inputNodes = inputs;
			inputWeights = new double[inputs.Length];

			for (int i = 0; i < inputs.Length; i++) {
				inputWeights [i] = (Random.value * 2.0) - 1.0;
			}
		}
	}

	public double CalculateOutput()
	{
		if (inputNodes != null) {
			output = 0.0;
			for (int i = 0; i < inputNodes.Length; i++) {
				output += inputNodes [i].CalculateOutput () * inputWeights [i];
			}

			output = sigmoid ((float)output);
		}

		return output;
	}

	public static double sigmoid(float val)
	{
		return 1.0 / (1.0 + Mathf.Exp(-val));
	}
}

