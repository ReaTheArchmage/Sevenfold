using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

/*
Problem: Measure distribution volatility across 7 nodes.
Input: Reference to nodes and the count of elements in each node.
Metric (entropy): A value representing how evenly elements are spread.

entropy == 1 = total evenness;
entropy < 1 = less evenly spread;
entropy == 0 = total unevenness;
*/

public partial class EvennessScore : Node {

	public override void _Ready()
	{

		float w = GetEvennessScore
		(
			// random input nodes:
			InputNodes()
		
			// total unevenness:
			/*[[0], [0], [0,0,0], [0,0,0,0], [0,0,0,0,0],
			[0,0,0,0,0,0], [0,0,0,0,0,0,0]]*/
		
			// total evenness:
			//[[0],[0],[0],[0],[0],[0],[0]]
		);
	}

	public float GetEvennessScore(int[][] nodes)
	{	
		// get node elements amount
		int[] node_lengths = new int[NODE_AMT];

		for (int x = 0; x < NODE_AMT; x++)
		{
			node_lengths[x] = nodes[x].Length;
		}

		return Evenness(node_lengths);
	}

public static float Evenness(int[] numbers)
{
    var occurrences = new Dictionary<int, int>();
    foreach (var number in numbers)
        occurrences[number] = occurrences.GetValueOrDefault(number) + 1;

    int distinctCount = occurrences.Count;
    int totalCount = numbers.Length;
    if (totalCount <= 1) return 1f;

    float entropyEvenness = 1f;
    if (distinctCount > 1)
    {
        float entropy = 0f;
        foreach (var count in occurrences.Values)
        {
            float probability = count / (float)totalCount;
            entropy -= probability * Mathf.Log(probability);
        }
        entropyEvenness = entropy / Mathf.Log(distinctCount);
    }

    float repetitionFactor = 1f - (distinctCount - 1) / (float)(totalCount - 1);

    return entropyEvenness * repetitionFactor;
}

	const int NODE_AMT = 7;

	public int[][] InputNodes() {
		
		Random rng = new Random();
		int[][] nodes = new int[NODE_AMT][];

		// create 7 nodes with random length
		for (int node_i = 0; node_i < NODE_AMT; node_i++)
		{
			int node_length = rng.Next(1,4);
			int[] node = new int[node_length];
	
			for (int i = 0; i < node_length; i++) {
			
				int i_value = 1; /* rng.Next(1,4); */
				node[i] = i_value;
			}

			nodes[node_i] = node;
		}

		return nodes;
	}
}
