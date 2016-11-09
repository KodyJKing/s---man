using UnityEngine;
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FFN
{
    int[] shape;
    public NNLayer[] layers;
    public float mutationRadius = 5;
    public float mutationRate = 0.5F;

    public FFN(params int[] shape)
    {
        this.shape = shape;
        layers = new NNLayer[shape.Length - 1];
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new NNLayer(shape[i + 1], shape[i], this);
        }
    }

    public FFN()
    {
    }

    public float[] response(params float[] input)
    {
        for (int i = 0; i < layers.Length - 1; i++)
            input = layers[i].response(input, false);
        return layers[layers.Length - 1].response(input, true); //Final layer should have linear output so that outputs aren't restricted to [0,1].
    }

    public void mutate()
    {
        foreach (NNLayer layer in layers)
            layer.mutate();
    }

    public void crossover(FFN spouse)
    {
        for (int i = 0; i < layers.Length; i++)
            layers[i].crossover(spouse.layers[i]);
    }

    public FFN clone()
    {
        FFN result = new FFN(shape);
        result.setGenome(getGenome());
        return result;
    }

    public List<float> getGenome()
    {
        List<float> result = new List<float>();
        foreach (NNLayer layer in layers)
        {
            foreach (Neuron neuron in layer.neurons)
            {
                result.Add(neuron.bias);
                foreach (float weight in neuron.weights)
                {
                    result.Add(weight);
                }
            }
        }
        return result;
    }

    public void setGenome(List<float> genome)
    {
        int i = 0;
        foreach (NNLayer layer in layers)
        {
            foreach (Neuron neuron in layer.neurons)
            {
                neuron.bias = genome[i++];
                for (int j = 0; j < neuron.weights.Length; j++)
                    neuron.weights[j] = genome[i++];
            }
        }
    }
}

public class NNLayer
{
    FFN network;

    public Neuron[] neurons;

    public NNLayer(int width, int inputWidth, FFN network)
    {
        this.network = network;
        neurons = new Neuron[width];
        for (int i = 0; i < width; i++)
            neurons[i] = new Neuron(inputWidth, network);
    }

    public float[] response(float[] input, bool linear)
    {
        int i = 0;
        float[] responses = new float[neurons.Length];
        foreach (Neuron neuron in neurons)
            responses[i++] = neuron.response(input, linear);
        return responses;
    }

    public void mutate()
    {
        foreach (Neuron neuron in neurons)
            neuron.mutate();
    }

    public void crossover(NNLayer spouse)
    {
        for (int i = 0; i < neurons.Length; i++)
            neurons[i].crossover(spouse.neurons[i]);
    }
}

public class Neuron
{
    FFN network;

    public float[] weights;
    public float bias = 0;

    public Neuron(int inputWidth, FFN network)
    {
        this.network = network;
        weights = new float[inputWidth];
    }

    public static float activation(float x)
    {
        return x < 0 ? 0 : (x < 1 ? x * x * 0.5F : x - 0.5F); 
        //return 1.0F / (1 + Mathf.Exp(-x));
    }

    public float stimulus(float[] input)
    {
        float sum = bias;
        for (int i = 0; i < input.Length; i++)
            sum += weights[i] * input[i];
        return sum;
    }

    public float response(float[] input, bool linear)
    {
        return linear ? stimulus(input) : activation(stimulus(input));
    }

    public void mutate()
    {
        if (Random.value < network.mutationRate)
            bias += 2 * (Random.value - 0.5F) * network.mutationRadius;
        bias *= 0.99F; //Bias decay so the contribution of mutation never becomes small.
        for (int i = 0; i < weights.Length; i++)
        {
            if (Random.value < network.mutationRate)
                weights[i] += 2 * (Random.value - 0.5F) * network.mutationRadius;
            weights[i] *= 0.99F; //Weight decay so the contribution of mutation never becomes small. 
        }
    }

    public void crossover(Neuron spouse)
    {
        if (Random.value < 0.5F)
            bias = spouse.bias;
        for (int i = 0; i < weights.Length; i++)
        {
            if (Random.value < 0.5F)
                weights[i] = spouse.weights[i];
        }
    }
}
