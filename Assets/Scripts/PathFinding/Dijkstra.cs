using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra 
{   
    public const float t_elevator_wait = 10f;
    public float Stair_ascend_factor  = 1.3f;
    public float Stair_descend_factor = 1.1f;

    public static List<Node> Dijkstrasolve(Graph graph, int startnodeindex, int targetnodeindex, int pathmodeState) {

        double starttime = Time.realtimeSinceStartup;
        int graphNodeCount = graph.TotalGraph.Count;

        InitWeightToMax(graph, graphNodeCount);
        InitDistanceToZero(graph, graphNodeCount);
        InitParentNodeToNULL(graph, graphNodeCount);
        InitWalkableToTrue(graph, graphNodeCount);

        
        Node startnode = graph.TotalGraph[startnodeindex];
        Node targetnode = graph.TotalGraph[targetnodeindex];

        List<Node> result = new List<Node>();
        Node resultnode = DijkstraAlgo(graph, startnode, targetnode, pathmodeState);

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
    private static Node DijkstraAlgo (Graph graph, Node startingNode, Node targetNode, int pathmodeState) {
        

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
                    float distance, weight;
                    CalCulateWeight(currentNode, neighnode, pathmodeState, out weight,out distance);

                    weight += currentNode.Weight;
                    distance += currentNode.Distance;

                    if (weight < node.Weight) {

                        node.SetDistance(distance);
                        node.SetWeight(weight);
                        node.SetParenNode(currentNode);
                    }
                    
                    
                }
            }

        }
        return targetNode;
    }


    private static void CalCulateWeight (Node from, Node to,int pathmodeState, out float r_weight, out float r_distance) {
        
        /*
        BASE WEIGHT CALCULATION UNIT IS DISTANCE (meters).
        this is to reduce calculations. so all factors must be converted into distance unit.

        for example, "waiting for elevator" time costs  approx. 120 seconds,
        weight = [Distance factor] + [120 seconds * walking speed(m/s)]
        */
        // TODO:
        // Original distance should be preserved, to show the "Actual" distance to user


        // distance to weight
        r_weight = 0f;
        r_distance = Vector3.Distance(from.Coordinate,to.Coordinate);

        r_weight += r_distance;
        
        // this will set the corresponding factors according to modes.
        // if pathmodeState is set to shortest, there will be no more calculations required.
        switch(pathmodeState) {
            case 0: // Recommended
                // no modification required.
                break;
            case 1: // Shortest
                return;
            case 2: // Comfortable
                // Modify Stair_ascend_factor;
                // Modify Stair_desced_factor;
            case 3: // Indoor
                break;
            case 4: // Stairless
                 // Modify Stair_ascend_factor;
                 // Modify Stair_descend_factor;
                 break;
        }

        if (from.NodeAttribute == 1 && to.NodeAttribute == 1) {
            float z_diff = to.Coordinate.z - from.Coordinate.z;
            if (z_diff >= .1f) { // Ascend
                //r_weight *= Stair_ascend_factor;

            }
            else if (z_diff <= .1f) { // Descend
                //r_weight *= Stair_descend_factor;
            }
            else {
                ;
            }
        }

        // if slope
        else if (from.NodeAttribute == (byte)0x0000f && to.NodeAttribute == (byte)0x0000f) { 
            
        }
        


        // additonal factors

        //psudocode
        /*
        if (both Node's attributes are "Stair") {

            if (Check if it's y coord is significantly different) }

                (Note that real stairs are not this steep.)

                Y           S2          S3
                            x-----------x
                A          /|           |\
                x         / |           | \
                i        /  |           |  \
                s       /   |           |   \
                       /    |           |    \
                + - - x- - -x - - - - - x- - -x - - - - -XZ Plane
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
            4F  ???
                |
            3F  + -- >> out
                |
            2F  +
                |
            1F  ??? 
                |
            B1F + -- << in
                |
            B2F ???

            Elevators are bit different. Elevator nodes are positioned along same Z axis.

            weight *= movement factor Ke (Ke < 1.0)
                
            After calcuation, constant Cw has to be added for "waiting time" when entering elevator
            if (from's attribute is "Non-Elevator") {
                weight += Cw (waiting time converted into dist)
            }

                
            }

        }
        */

    }
    private static void InitWeightToMax (Graph graph, int nodecount) {
        for (int i = 0 ; i < nodecount ; i++) {
            graph.TotalGraph[i].SetWeight(float.MaxValue);
        }
    }
    private static void InitDistanceToZero (Graph graph, int nodecount) {
        for (int i = 0 ; i < nodecount ; i++) {
            graph.TotalGraph[i].SetDistance(0f);
        }
    }
    private static void InitParentNodeToNULL (Graph graph, int nodecount) {
        for (int i = 0 ; i < nodecount ; i++) {
            graph.TotalGraph[i].SetParenNode(null);
        }
    }
    private static void InitWalkableToTrue (Graph graph, int nodecount) {
        for (int i = 0; i < nodecount ; i++) {
            graph.TotalGraph[i].SetIsWalkable(true);
        }
    }
     
}
