using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

public partial class Sevenfold : Node
{

	public const int NODE_AMT = 7;
	[Export] SNode[] nodes;
	Dictionary<SNode, float> nodes_multipliers = new Dictionary<SNode, float>();
	float node_evenness;

	public override void _Ready()
	{

		node_evenness = Evenness(GetNodeLengths(nodes));
		GD.Print($"{node_evenness}");

		UpdateMultipliers(node_evenness, 1.0f);
	}

	public int[] GetNodeLengths(SNode[] nodes)
	{	

		int[] node_lengths = new int[NODE_AMT];

		for (int x = 0; x < NODE_AMT; x++)
		{
			node_lengths[x] = nodes[x].elements.Length;
		}

		return node_lengths;
	}

public static float Evenness(int[] numbers)
{

    int totalCount = numbers.Length;
    if (totalCount <= 1) return 1f;

    var occurrences = new Dictionary<int, int>();
    foreach (var number in numbers)
        occurrences[number] = occurrences.GetValueOrDefault(number) + 1;

    float entropy = 0f;
    foreach (var count in occurrences.Values)
    {
        float probability = count / (float)totalCount;
        entropy -= probability * Mathf.Log(probability);
    }

    float maxEntropy = Mathf.Log(totalCount);
    return Math.Max(0f, 1f - (entropy / maxEntropy));
}

	public void UpdateMultipliers(float evenness, float multiplier_strength)
	{

		float[] multipliers = new float[NODE_AMT];

		for (int n = 0; n < multipliers.Length; n++)
		{

			float mult = multipliers[n];
			multipliers[n] = 1f + evenness * multiplier_strength;
			nodes_multipliers[nodes[n]] = mult;
			//GD.Print($"{nodes_multipliers[nodes[n]]}");
		}		
	}
}
