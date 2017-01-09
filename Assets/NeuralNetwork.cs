
public class NeuralNetwork
{
	public NeuronLayer inputLayer;
	public NeuronLayer outputLayer;
	public NeuronLayer[] hiddenLayers;

	public NeuralNetwork (int inputs, int outputs, int nodesInHiddenLayer, int numHiddenLayers)
	{
		inputLayer = new NeuronLayer(inputs, null);
		hiddenLayers = new NeuronLayer[numHiddenLayers];

		NeuronLayer lastLayer = inputLayer;

		for (int i = 0; i < numHiddenLayers; i++){
			hiddenLayers[i] = new NeuronLayer(nodesInHiddenLayer, lastLayer.neurons);
			lastLayer = hiddenLayers[i];
		}

		outputLayer = new NeuronLayer (outputs, lastLayer.neurons);
	}
}

