using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VisualizePathResult
{
    //using linerenderer component to draw path is easy, but looks bad.
    //calculating each vertex at certain nodes and making a mesh to render
    //a component might be hard, but i will look good with a Unlit/Color shader.
    public static void VisulaizePath (List<Node> node , GameObject nodeobject, GameObject edgeobject) {
        //Quaternion quaternion = new Quaternion();
        Vector3 anglebetweennode = new Vector3();
        Vector3 lengthtoscale = new Vector3();
        float lengthbetweennode = new float();
        //drawing nodes 
        for (int i = 0 ; i < node.Count; i++ ) {
            GameObject.Instantiate(nodeobject,node[i].Coordinate,Quaternion.identity);
            
            if (i < node.Count - 1) {
                //draw line between node[i].Coordinate, node[i+1].Coordinate

                //quaternion = Quaternion.identity;
                anglebetweennode = node[i].Coordinate - node[i+1].Coordinate;
                lengthbetweennode = Vector3.Distance(node[i].Coordinate,node[i+1].Coordinate);
                lengthtoscale = new Vector3(1f,0.1f,lengthbetweennode);
                //quaternion.eulerAngles = new Vector3(anglebetweennode.x,0f,anglebetweennode.z);
                //quaternion.eulerAngles = Vector3.up
                
                
                GameObject thisobject = GameObject.Instantiate(edgeobject,(node[i].Coordinate + node[i+1].Coordinate)/2,Quaternion.LookRotation(anglebetweennode,Vector3.up)); //.transform.localScale = lengthtoscale;
                thisobject.transform.localScale = lengthtoscale;
            }
            else if (i == node.Count -1) {
                break;
            }
            //Instantiate(nodeobject,node[i].Coordinate,Quaternion.identity);
        }    
        //drawing edges

    }

}
