using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizePathResult
{
    //using linerenderer component to draw path is easy, but looks bad.
    //calculating each vertex at certain nodes and making a mesh to render
    //a component might be hard, but i will look good with a Unlit/Color shader.

    public static void VisulaizePath (List<Node> node , GameObject nodeobject, GameObject edgeobject) {

        //Destroys previously added path components
        OncallDestroyPrevious();

        Vector3 anglebetweennode = new Vector3();
        Vector3 lengthtoscale = new Vector3();
        float lengthbetweennode = new float();
        //drawing nodes 
        for (int i = 0 ; i < node.Count; i++ ) {
            //Draw Nodes
            GameObject nodecompobject = GameObject.Instantiate(nodeobject,node[i].Coordinate,Quaternion.identity);
            nodecompobject.tag = "Instantiatepath";

            //Draw Edges
            if (i < node.Count - 1) {
                anglebetweennode = node[i].Coordinate - node[i+1].Coordinate;
                lengthbetweennode = Vector3.Distance(node[i].Coordinate,node[i+1].Coordinate);
                lengthtoscale = new Vector3(1f,0.2f,lengthbetweennode);
                
                GameObject edgecompobject = GameObject.Instantiate(edgeobject,(node[i].Coordinate + node[i+1].Coordinate)/2,Quaternion.LookRotation(anglebetweennode,Vector3.up)); //.transform.localScale = lengthtoscale;
                edgecompobject.transform.localScale = lengthtoscale;
                edgecompobject.tag = "Instantiatepath";
            }
            else if (i == node.Count -1) {
                break;
            }
        }    

    }

    public static void OncallDestroyPrevious(){
        var previousObjects = GameObject.FindGameObjectsWithTag("Instantiatepath");
            for (int i = 0; i < previousObjects.Length ; i++) {
                GameObject.Destroy(previousObjects[i]);
            }
    }

}
