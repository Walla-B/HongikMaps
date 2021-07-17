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
        OnCallDestroyPrevious_PathObj();

        Vector3 anglebetweennode = new Vector3();
        Vector3 lengthtoscale = new Vector3();
        float lengthbetweennode = new float();
        
        //Initiated Objects belongs to parent "PathObjects"
        GameObject parent = GameObject.Find("PathObjects");
        //drawing nodes 
        for (int i = 0 ; i < node.Count; i++ ) {
            //Draw Nodes
            GameObject nodecompobject = GameObject.Instantiate(nodeobject,node[i].Coordinate,Quaternion.identity,parent.transform);
            
            nodecompobject.tag = "Instantiatepath";

            //Draw Edges

            //If Node.type == elevator && Node[i+1].type == elevator
            //Instantiate nodeobj in middle of two points, set vertical localscale to match the size,
            //so that elevator path appears as cylinder shape.
            
            //can be optimized by seperating case:  node[i].type == elevator && node[i+1].type != elevator, do not initiate nodes between start and end.
            if (i < node.Count - 1) {
                anglebetweennode = node[i].Coordinate - node[i+1].Coordinate;
                lengthbetweennode = Vector3.Distance(node[i].Coordinate,node[i+1].Coordinate);
                lengthtoscale = new Vector3(1f,0.2f,lengthbetweennode);
                
                GameObject edgecompobject = GameObject.Instantiate(edgeobject,(node[i].Coordinate + node[i+1].Coordinate)/2,Quaternion.LookRotation(anglebetweennode,Vector3.up),parent.transform); //.transform.localScale = lengthtoscale;
                edgecompobject.transform.localScale = lengthtoscale;
                edgecompobject.tag = "Instantiatepath";
            }
            else if (i == node.Count -1) {
                break;
            }
        }    

    }

    public static void OnCallDestroyPrevious_PathObj(){
        var previousObjects = GameObject.FindGameObjectsWithTag("Instantiatepath");
            for (int i = 0; i < previousObjects.Length ; i++) {
                GameObject.Destroy(previousObjects[i]);
            }
    }

}
