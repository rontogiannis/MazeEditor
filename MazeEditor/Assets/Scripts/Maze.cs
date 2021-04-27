using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class Maze : MonoBehaviour
{
    public InputField textObject;
    public GameObject cellPrefab;

    public float side = 0.5f;

    public float lowLeftX = 0f;
    public float lowLeftZ = 0f;

    int width, depth;

    List<GameObject> prefabsSpawned;

    void Awake()
    {
        prefabsSpawned = new List<GameObject>();
    }

    void ClearAndDestroy()
    {
        foreach (GameObject g in prefabsSpawned)
        {
            Destroy(g);
        }

        prefabsSpawned.Clear();
    }

    public void Encode()
    {
        string result = "";

        result += width + " " + depth + "\n";

        for(int x=0; x<width; ++x)
        {
            string line = "";

            for(int z=0; z<depth; ++z)
            {
                int bigIndex = x * depth + z;
                line += prefabsSpawned[bigIndex].GetComponent<CellEncoder>().GetValue();
            }

            result += line + "\n";
        }

        textObject.text = result;

    }

    public void Decode()
    {
        // Parsing

        string[] elementsRaw = textObject.text.Split(null);

        List<string> elements = new List<string>();


        for(int i=0; i<elementsRaw.Length; ++i)
        {
            if (elementsRaw[i].Length > 0)
                elements.Add(elementsRaw[i]);
        }

        if (elements.Count < 3)
            return;

        bool okWidth = int.TryParse(elements[0], out width);
        bool okDepth = int.TryParse(elements[1], out depth);

        if (!okWidth || !okDepth)
            return;

        string big = "";

        for(int i=2; i<elements.Count; ++i)
        {
            big += elements[i];
        }

        if (big.Length != width * depth)
            return;


        // Actually Generating

        ClearAndDestroy();

        for(int x=0; x<width; ++x)
        {
            for(int z=0; z<depth; ++z)
            {
                int bigIndex = x * depth + z;
                int mazeValue;

                if (!int.TryParse(big[bigIndex].ToString(), out mazeValue))
                {
                    ClearAndDestroy();
                    return;
                }
                    

                if (mazeValue < 0 || mazeValue > 3)
                {
                    ClearAndDestroy();
                    return;
                }


                Vector2 spawnPos = new Vector2((x - lowLeftX) * side, (z - lowLeftZ) * side);

                GameObject instance = Instantiate(cellPrefab, spawnPos, Quaternion.identity, transform);

                if ((mazeValue & 1) == 0)
                {
                    instance.transform.Find("Right").GetComponent<SpriteRenderer>().enabled = false;
                }

                if ((mazeValue & 2) == 0)
                {
                    instance.transform.Find("Up").GetComponent<SpriteRenderer>().enabled = false;
                }

                if (x > 0 || z > 0)
                {
                    instance.transform.Find("Pole4").gameObject.SetActive(false);
                }

                if (x > 0)
                {
                    instance.transform.Find("Left").gameObject.SetActive(false);
                    instance.transform.Find("Pole1").gameObject.SetActive(false);
                }

                if (z > 0)
                {
                    instance.transform.Find("Down").gameObject.SetActive(false);
                    instance.transform.Find("Pole3").gameObject.SetActive(false);
                }

                prefabsSpawned.Add(instance);
            }
        }
    }

}
