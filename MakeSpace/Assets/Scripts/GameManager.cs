using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PriorityQueue<Action> actionQueue = new PriorityQueue<Action>();

    // Start is called before the first frame update
    void Awake()
    {
        if (instance && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResolveActions()
    {

    }
}
