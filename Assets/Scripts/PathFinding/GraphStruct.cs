using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {
    
    //Node as key is bad implementation
    
    //public Dictionary<Node, List<Node>> TotalGraph {get; private set;}
    public List<Node> TotalGraph {get; private set;}

    public Graph() {
        TotalGraph = new List<Node>();
    }
    public void AddNodeToGraph(Node node) {
        this.TotalGraph.Add(node);
    }
    public Node GetNodeInGraph(int index) {
        return TotalGraph[index];
    }

    //////
    //      |Node|   ->     |Node|   ->    |Node|   ->    |Node| 
    //        |               |              |              |
    // index [0]             [1]            [2]            [3]
    //
    //////
}

public class Node {
    public int NodeID {get; private set;}
    public Vector3 Coordinate {get; private set;}
    public List<Node> AdjNodes {get; private set;}

    /*
    public float Xcoord {get; private set;}
    public float Ycoord {get; private set;}
    public float Zcoord {get; private set;}
    */
    public Node(){
    }
    public Node(int id, Vector3 position /* + Additional information */) {
        AdjNodes = new List<Node>();
        this.NodeID = id; 
        this.Coordinate = position;
    }
    //method to add edges
    public void AddAdjNode(Node node) {
        AdjNodes.Add(node);
    }
}

