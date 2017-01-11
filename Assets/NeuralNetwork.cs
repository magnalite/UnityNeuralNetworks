using UnityEngine;
using System.Collections.Generic;

public class NeuralNetwork
{
	public NeuronLayer inputLayer;
	public NeuronLayer outputLayer;
	public NeuronLayer[] hiddenLayers;
	public double fitness;

	public NeuralNetwork (int inputs, int outputs, int nodesInHiddenLayer, int numHiddenLayers)
	{
		inputLayer = new NeuronLayer(inputs, null, false);
		hiddenLayers = new NeuronLayer[numHiddenLayers];

		NeuronLayer lastLayer = inputLayer;

		for (int i = 0; i < numHiddenLayers; i++){
			hiddenLayers[i] = new NeuronLayer(nodesInHiddenLayer, lastLayer.neurons, true);
			lastLayer = hiddenLayers[i];
		}

		outputLayer = new NeuronLayer (outputs, lastLayer.neurons, false);
	}

	public List<double> getWeights()
	{
		List<double> weights = new List<double>();

		foreach (NeuronLayer layer in hiddenLayers){
			foreach (Neuron neuron in layer.neurons) {
				if (neuron.inputWeights != null) {
					weights.AddRange (neuron.inputWeights);
				}
			}
		}

		foreach (Neuron neuron in outputLayer.neurons) {
			weights.AddRange (neuron.inputWeights);
		}

		return weights;
	}

	public void putWeights(List<double> weights)
	{
		int index = 0;

		foreach (NeuronLayer layer in hiddenLayers){
			foreach (Neuron neuron in layer.neurons) {
				if (neuron.inputWeights != null) {
					for (int i = 0; i < neuron.inputWeights.Length; i++) {
						neuron.inputWeights [i] = weights [index++];
					}
				}
			}
		}

		foreach (Neuron neuron in outputLayer.neurons) {
			for (int i = 0; i < neuron.inputWeights.Length; i++) {
				neuron.inputWeights [i] = weights [index++];
			}
		}
	}
}

