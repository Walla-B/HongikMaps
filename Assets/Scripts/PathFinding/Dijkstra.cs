using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra 
{   
    public static List<Node> Dijkstrasolve(Graph graph, int startnodeindex, int targetnodeindex) {

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

    //FIXME:
    //it works fine, but time complexity of this algorithm is so slow.
    //Current time complexity is O(n^2)
    //using priority queue(Fibonacci heap), it can be reduced by O(n + elogn)
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
        
        /*
        BASE WEIGHT CALCULATION UNIT IS DISTANCE (meters).
        this is to reduce calculations. so all factors must be converted into distance unit.

        for example, "waiting for elevator" time costs  approx. 120 seconds,
        weight = [Distance factor] + [120 seconds * walking speed(m/s)]
        */

        float weight = 0f;
        // distance to weight
        float distance = Vector3.Distance(from.Coordinate,to.Coordinate);
        weight += distance;

        // additonal factors

        //psudocode
        /*
        if (both Node's attributes are "Stair") {

            if (Check if it's z coord is significantly different) {

                (Note that real stairs are not this steep.)

                Z           S2          S3
                            x-----------x
                A          /|           |\
                x         / |           | \
                i        /  |           |  \
                s       /   |           |   \
                       /    |           |    \
                + - - x- - -x - - - - - x- - -x - - - - -XY Plane
                     S1     S1'         S3'   S4

                On ascend S1 -> S2:
                    weight *= movement factor Ka (Ka > 1.0)
                On descend S3 -> S4:
                    weight *= movement factor kd (Kd > 1.0?)
                
                S2 -> S3 are both "Stair" nodes, but ignored by if statement above

            }
            else if (both Node's attributes are "Slope") {
                Same as calculating "Stair", just with different factor
            }
            else if ("to" Node's attribute is "Elevator") {
                4F  ■
                    |
                3F  + -- >> out
                    |
                2F  +
                    |
                1F  □ 
                    |
                B1F + -- << in
                    |
                B2F ■

                Elevators are bit different. Elevator nodes are positioned along same Z axis.

                weight *= movement factor Ke (Ke < 1.0)
                 
                After calcuation, constant Cw has to be added for "waiting time" when entering elevator
                if (from's attribute is "Non-Elevator") {
                    weight += Cw (waiting time converted into dist)
                }

                
            }

        }
        */
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
