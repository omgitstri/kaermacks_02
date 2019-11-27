using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AdjacencyGraph : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        GenerateAdjacencyGraph();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateAdjacencyGraph()
    {
        //Generate a adjacency graph of all the childs. Export as a text file.

        GameObject[] cubes = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cubes[i] = transform.GetChild(i).gameObject;
        }

        string content = "" + transform.childCount + "\n";

        for(int i = 0; i < cubes.Length; i++)
        {
            string cubeIndex = i + " " + cubes[i].name + "\n";
            content += cubeIndex;

        }
        for (int i = 0; i < cubes.Length; i++)
        {
            string cubeInfo;
            if(cubes[i].GetComponent<IndividualCube>().frontCube == null)
            {
                cubeInfo = "" + i + " " +
                                -1 + 
                                cubes[i].GetComponent<IndividualCube>().backCube.transform.GetSiblingIndex().ToString() + "\n";
            }else if(cubes[i].GetComponent<IndividualCube>().backCube == null)
            {
                cubeInfo = "" + i + " " +
                                cubes[i].GetComponent<IndividualCube>().frontCube.transform.GetSiblingIndex().ToString() + 
                                -1 + "\n";
            }
            else
            {
                cubeInfo = "" + i + " " +
                                cubes[i].GetComponent<IndividualCube>().frontCube.transform.GetSiblingIndex().ToString() + 
                                cubes[i].GetComponent<IndividualCube>().backCube.transform.GetSiblingIndex().ToString() + "\n";
            }

            content += cubeInfo;
        }


        string path = Application.dataPath + "/AdjacencyGraph.txt";


        if (!File.Exists(path))
        {
            File.WriteAllText(path, content);
        }
    }
}
