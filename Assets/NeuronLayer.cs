using UnityEngine;

public class NeuronLayer
{
	public int layerSize;
	public Neuron[] neurons;


	public NeuronLayer (int size, Neuron[] inputs)
	{
		//Add 1 for bias neuron
		layerSize = size + 1;
		neurons = new Neuron[layerSize];

		for (int i = 0; i < size; i++) {
			if (inputs != null) {
				neurons [i] = new Neuron (inputs);
			} else {
				neurons [i] = new Neuron (null);
			}
		}
		//Bias neuron
		neurons [size] = new Neuron (null);
		neurons [size].output = (Random.value * 2.0) - 1;
	}
}

