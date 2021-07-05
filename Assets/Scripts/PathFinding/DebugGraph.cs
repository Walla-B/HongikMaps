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

                if (Vector3.Distance(curnode.Coordinate,targetNode.Coordinate) > 10f)
                {
                    Debug.DrawLine(curnode.Coordinate,targetNode.Coordinate,Color.red,10000f);
                }
                else
                    Debug.DrawLine(curnode.Coordinate,targetNode.Coordinate,Color.blue,10000f);

            }

        }
    }

}
