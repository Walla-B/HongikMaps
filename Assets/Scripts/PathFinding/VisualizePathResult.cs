using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VisualizePathResult
{
    //using linerenderer component to draw path is easy, but looks bad.
    //calculating each vertex at certain nodes and making a mesh to render
    //a component might be hard, but i will look good with a Unlit/Color shader.
    public static void VisulaizePath (List<Node> node , GameObject nodeobject) {
        //drawing nodes 
        for (int i = 0 ; i < node.Count; i++ ) {
            GameObject instantobj = GameObject.Instantiate(nodeobject,node[i].Coordinate,Quaternion.identity);
            if (i < node.Count - 1) {
                //draw line between node[i].Coordinate, node[i+1].Coordinate
            }
            else if (i == node.Count -1) {
                break;
            }
            //Instantiate(nodeobject,node[i].Coordinate,Quaternion.identity);
        }    
        //drawing edges

    }

}
