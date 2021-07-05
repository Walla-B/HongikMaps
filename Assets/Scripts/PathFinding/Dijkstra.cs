using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra 
{   
    public List<Node> Dijkstrasolve(Graph graph, int startnodeindex, int targetnodeindex) {

        double starttime = Time.realtimeSinceStartup;

        InitDistanceToMax(graph);
        InitParentNodeToNULL(graph);
        InitWalkableToTrue(graph);

        
        Node startnode = graph.TotalGraph[startnodeindex];
        Node targetnode = graph.TotalGraph[targetnodeindex];

        List<Node> result = new List<Node>();
        Node resultnode = DijkstraAlgo(graph, startnode, targetnode);

        while (resultnode != null) {
            result.Add(resultnode);
            resultnode = resultnode.ParentNode;
        }

        result.Reverse();


        double endTime = Time.realtimeSinceStartup - starttime;
        Debug.Log("Time elapsed : " + endTime);

        return result;
        //InitDistanceToMax(graph);

        /*

        List<Node> visitednodes = new List<Node>();
        Queue<Node> nodestovisit = new Queue<Node>();

        nodestovisit.Enqueue(targetnode);

        while (nodestovisit.Count > 0) {
            if (startnode == targetnode) {
                startnode.Setdistance(0f);
            }
            List<Node> nextNodes = startnode.AdjNodes;
            var filterdNodes = nextNodes
        }
        */

    }


    private static Node DijkstraAlgo (Graph graph, Node startingNode, Node targetNode) {
        

        List<Node> unexplored = new List<Node>();


        for (int i = 0; i < graph.TotalGraph.Count; i++) {
            if (graph.TotalGraph[i].IsWalkAble == true) {
                unexplored.Add(graph.TotalGraph[i]);
            }
        }

        Node startnode = startingNode;
        startingNode.SetWeight(0f);


        while (unexplored.Count > 0) {
            unexplored.Sort((x1,x2) => x1.Weight.CompareTo(x2.Weight));
            Node currentNode = unexplored[0];
            


            unexplored.Remove(currentNode);

            foreach(Node neighnode in currentNode.AdjNodes) {
                Node node = neighnode;

                if (unexplored.Contains(neighnode) && node.IsWalkAble) {
                    float weight = CalCulateWeight(currentNode,neighnode);
                    weight += currentNode.Weight;

                    if (weight < node.Weight) {
                        node.SetWeight(weight);
                        node.SetParenNode(currentNode);
                    }
                    
                    
                }
            }

        }
        return targetNode;
    }

    private static float CalCulateWeight (Node from, Node to) {
        float distance = Vector3.Distance(from.Coordinate,to.Coordinate);
        
        /*functions to manipulate weight by its factors needed. current is just weight = node*/
        float weight = distance;

        return weight;

    }
    private static void InitDistanceToMax (Graph graph) {
        for (int i = 0 ; i < graph.TotalGraph.Count ; i++) {
            graph.TotalGraph[i].SetWeight(float.MaxValue);
        }
    }
    private static void InitParentNodeToNULL (Graph graph) {
        for (int i = 0 ; i < graph.TotalGraph.Count ; i++) {
            graph.TotalGraph[i].SetParenNode(null);
        }
    }
    private static void InitWalkableToTrue (Graph graph) {
        for (int i = 0; i < graph.TotalGraph.Count ; i++) {
            graph.TotalGraph[i].SetIsWalkable(true);
        }
    }
     
}
