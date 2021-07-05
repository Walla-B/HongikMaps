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
    //
    // index [0]             [1]            [2]            [3]
    //        |               |              |              |
    //      |Node|   ->     |Node|   ->    |Node|   ->    |Node| 
    //
    //////

    //////
    //      |Node|
    //        |
    //    List<Node>
    //        +
    //       data
    //////
}

public class Node {
    public int NodeID {get; private set;}
    public Vector3 Coordinate {get; private set;}
    public List<Node> AdjNodes {get; private set;}
    
    //pathfinding components
    public float Weight {get; private set;}

    public Node ParentNode {get; private set;}

    public bool IsWalkAble {get; private set;}

    /*
    public float Xcoord {get; private set;}
    public float Ycoord {get; private set;}
    public float Zcoord {get; private set;}
    */
    public Node() {     }
    public Node(int id, Vector3 position /* + Additional information */) {
        AdjNodes = new List<Node>();
        this.NodeID = id; 
        this.Coordinate = position;
    }
    //method to add edges
    public void AddAdjNode(Node node) {
        AdjNodes.Add(node);
    }
    public void SetWeight(float weight) {
        this.Weight = weight;
    }
    public void SetParenNode(Node node) {
        this.ParentNode = node;
    }
    public void SetIsWalkable(bool walkable) {
        this.IsWalkAble = walkable;
    }
}


    /*    
    public class Node : MonoBehaviour
    {
        [SerializeField]
        private ushort Node_ID;
        [SerializeField]
        //private float X_Coord, Y_Coord, Z_Coord;
        private float weight;
        [SerializeField]
        private string Node_Name;
        [SerializeField]
        private byte Node_Attribute, Node_Building;
        [SerializeField]
        private sbyte Node_floor;
        private List<Node> neighbourNode;

    */