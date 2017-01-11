using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticsHandler
{
	private static float BREEDING_PERCENTAGE = 0.2f;
	private static float MUTATION_RATE = 0.3f;
	private static float MAX_MUTEX = 0.1f;
	private static float CROSSOVER_RATE = 0.5f;
	private static float LIVING_PERCENTAGE = 0.1f;

	public GeneticsHandler ()
	{
		
	}

	public static NeuralNetwork[] breedNetworks(NeuralNetwork[] networks, int numberChildren)
	{
		int NumParents           = (int)(networks.Length * BREEDING_PERCENTAGE + 1);
		int NumLiving            = (int)(networks.Length * LIVING_PERCENTAGE + 1);
		NeuralNetwork[] children = new NeuralNetwork[numberChildren];
		NeuralNetwork[] parents  = new NeuralNetwork[NumParents];

		networks = (NeuralNetwork[]) networks.OrderBy (net => net.fitness).ToArray();

		for (int i = 0; i < NumParents; i++) {
			parents [i] = networks [networks.Length - 1 - i];
		}

		int childrenMade = NumLiving;

		for (int i = 0; i < NumLiving; i++) {
			children [i] = networks [networks.Length - 1 - i];
		}

		while (childrenMade < numberChildren){
			children [childrenMade++] = crossNetworks (
				parents[UnityEngine.Random.Range(0, NumParents)], 
				parents[UnityEngine.Random.Range(0, NumParents)]);
		}

		return children;
	}

	public static NeuralNetwork crossNetworks(NeuralNetwork network1, NeuralNetwork network2)
	{
		double[] weights1 = network1.getWeights ().ToArray ();
		double[] weights2 = network2.getWeights ().ToArray ();
		double[] childWeights = new Double[weights1.Length];

		NeuralNetwork network = new NeuralNetwork (5, 1, 4, 1);

		bool WeightsSwitch = false;
		for (int i = 0; i < weights1.Length; i++){
			
			childWeights [i] = WeightsSwitch ? weights1 [i] : weights2 [i];

			if (UnityEngine.Random.value < MUTATION_RATE) {
				childWeights[i] += (double) ((UnityEngine.Random.value * 2) - 1) * MAX_MUTEX;
			}
		}
			
		//bias neurons

		for (int i = 0; i < network.hiddenLayers.Length; i++){
			int biasLocation = network.hiddenLayers[i].neurons.Length - 1;

			WeightsSwitch = UnityEngine.Random.value < CROSSOVER_RATE ? !WeightsSwitch : WeightsSwitch;

			network.hiddenLayers [i].neurons [biasLocation] = WeightsSwitch ? 
				network1.hiddenLayers [i].neurons [biasLocation] : 
				network2.hiddenLayers [i].neurons [biasLocation];

			if (UnityEngine.Random.value < MUTATION_RATE) {
				network.hiddenLayers [i].neurons [biasLocation].output += (double) ((UnityEngine.Random.value * 2) - 1) * MAX_MUTEX;
			}
		}
			
		network.putWeights (new List<double>(childWeights));

		return network;
	}

	public static float getMaxFitness(NeuralNetwork[] networks){
		float fitness = -9999999;

		foreach (NeuralNetwork net in networks) {
			if (net.fitness > fitness) {
				fitness = (float) net.fitness;
			}
		}

		return fitness;
	}

	public static float getAverageFitness(NeuralNetwork[] networks){
		float fitness = 0;

		foreach (NeuralNetwork net in networks) {
			fitness += (float) net.fitness;
		}

		return fitness / networks.Length;
	}
}

