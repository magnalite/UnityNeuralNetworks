using UnityEngine;

public class NeuronLayer
{
	public int layerSize;
	public Neuron[] neurons;


	public NeuronLayer (int size, Neuron[] inputs, bool addBias)
	{
		//Add 1 for bias neuron
		layerSize = addBias ? size + 1 : size;

		neurons = new Neuron[layerSize];

		for (int i = 0; i < size; i++) {
			if (inputs != null) {
				neurons [i] = new Neuron (inputs);
			} else {
				neurons [i] = new Neuron (null);
			}
		}
		//Bias neuron
		if (addBias) {
			neurons [size] = new Neuron (null);
			neurons [size].output = (Random.value * 2.0) - 1;
		}
	}
}

