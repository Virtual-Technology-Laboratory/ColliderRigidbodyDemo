using UnityEngine;
using System.Collections;

public class MakeIons : MonoBehaviour {

    public int xSize = 4;
    public int ySize = 4;
    public int zSize = 4;

    public float xSpacing = 1.1f;
    public float ySpacing = 1.1f;
    public float zSpacing = 1.1f;

    public GameObject IonPrefab;
    void Start () 
    {
        Vector3 centerPos = transform.position;
        Vector3 gridSize = new Vector3( xSize*xSpacing, ySize*ySpacing, zSize*zSpacing );
        Vector3 origin = centerPos - (gridSize / 2);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    GameObject g = Instantiate(IonPrefab);
                    g.transform.position = origin + new Vector3(x * xSpacing, y * ySpacing, z * zSpacing);
                    g.transform.parent = transform;
                }
            }
        }
	}	
}
