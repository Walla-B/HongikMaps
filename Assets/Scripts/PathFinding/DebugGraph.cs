using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class DebugGraph
{   
    public static void DrawGraph(Graph graph) {

        for (int i = 0; i < graph.TotalGraph.Count ; i++) {
            Node curnode = graph.TotalGraph[i];
            List<Node> targetnodelist = curnode.AdjNodes;

            for (int j = 0; j < curnode.AdjNodes.Count; j++) {
                Node targetNode = targetnodelist[j];
                Debug.DrawLine(curnode.Coordinate,targetNode.Coordinate,Color.gray,10000f);
            }
            
        }
    }

    public static void DrawPath(List<Node> path) {
        for (int i = 1 ; i < path.Count ; i++ ) {
            // Just draw path wiht green line
            // Debug.DrawLine(path[i].Coordinate, path[i-1].Coordinate, Color.green, 10000f);

            Node curnode = path[i];
            Node beforenode = path[i-1];

            if (Mathf.Abs(curnode.Coordinate.y - beforenode.Coordinate.y) < 1e-3) {
                Debug.DrawLine(curnode.Coordinate, beforenode.Coordinate, Color.yellow, 10000f);
            }
            else {

                float xyplandist = Vector3.Distance(new Vector3(curnode.Coordinate.x, curnode.Coordinate.y, 0f),
                 new Vector3(beforenode.Coordinate.x, beforenode.Coordinate.y, 0f));
                
                float zdist = curnode.Coordinate.y - beforenode.Coordinate.y;

                if (zdist > 0) {
                    Debug.DrawLine(curnode.Coordinate, beforenode.Coordinate, Color.red, 10000f);
                }
                else {
                    Debug.DrawLine(curnode.Coordinate, beforenode.Coordinate, Color.green, 10000f);
                }

            }


        }
    }

}
