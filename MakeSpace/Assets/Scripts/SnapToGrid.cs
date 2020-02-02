using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{

    public Vector3Int coordinates;
    GridLayout gridLayout;

    // Start is called before the first frame update
    void Start()
    {
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();

        float seed = Random.Range(0, 100);
        print(seed);
        print(Mathf.PerlinNoise(-0.2f, -0.3f));
        seed = Random.Range(0, 10000);
        print(seed);
        print(Mathf.PerlinNoise(-0.2f, -0.3f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = gridLayout.CellToWorld(coordinates);
    }

    void Move()
    {

    }
}
