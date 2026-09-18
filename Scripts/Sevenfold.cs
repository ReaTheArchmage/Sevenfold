using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

	// THE SEVENFOLD:
// The sevenfold is a synergy/build machine. The sevenfold is consisted of seven nodes,
// or SNode resources. Each SNode contains an array of elements that represent the game
// items equipped to that node. Each node has a multiplicative bonus on each element in 
// it, these values scale in different ways depending on the arbitrary distribution of 
// elements in nodes the player decides to equip:

// The sevenfold rewards more the player that equips items evenly, giving them a good 
// linear scaling through the node tree, while troubling the player that puts all in one 
// node (albeit, vertical builds are still possible, with more investment). Each node will 
// probably have its own attribute, like damage, presentation, speed, affinity... 


	// TODO:
// The second Sevenfold's mechanic is the spinning wheel. Simply, the player can offset the 
// order of multiplicative bounses between nodes, allowing for a deeper build customization.

public partial class Sevenfold : Node
{

	public const int NODE_AMT = 7;
	[Export] SNode[] nodes;
	Dictionary<SNode, float> nodes_multipliers = new Dictionary<SNode, float>();
	float node_evenness;

	public override void _Ready()
	{

		UpdateMultipliers(1.0f);
	}

	// Is called every time node elements change, to calculate evenness first, then multipliers
	// accordingly. node to multiplier dictionary is constructed here. 
	public void UpdateMultipliers(float multiplier_strength)
	{
		node_evenness = Evenness(GetNodeLengths(nodes));

		float[] multipliers = new float[NODE_AMT];

		for (int n = 0; n < multipliers.Length; n++)
		{

			multipliers[n] = 1f + node_evenness * multiplier_strength;
			float mult = multipliers[n];
			nodes_multipliers[nodes[n]] = mult;
			//GD.Print($"{nodes_multipliers[nodes[n]]}");

			multiplier_strength *= 1f + (0.2f * node_evenness);
		}		
	}

	// Calculate the evenness distribution between node lengths. The more similar the lengths
	// are between them, the highest the evenness that will return, the opposite happens otherwise
	// when lengths all tend to a unique value, then evenness will return numbers closer to 0.
	public static float Evenness(int[] numbers)
	{

	    int totalCount = numbers.Length;
	    var occurrences = new Dictionary<int, int>();
	    float entropy = 0f;
	    float maxEntropy = Mathf.Log(totalCount);
	
		if (totalCount <= 1) return 1f;

	    foreach (var number in numbers)
			occurrences[number] = occurrences.GetValueOrDefault(number) + 1;

	    foreach (var count in occurrences.Values)
	    {

	        float probability = count / (float)totalCount;
	        entropy -= probability * Mathf.Log(probability);
	    }

	    return Math.Max(0f, 1f - (entropy / maxEntropy));
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
}
