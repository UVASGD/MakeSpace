using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Vector2Int pos;

    [HideInInspector]
    public int oxygen_count;

    [HideInInspector]
    public int jetpack_count;

    [HideInInspector]
    List<Effect> effects = new List<Effect>();

    Action current_action = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInput(CharacterInput characterInput)
    {
        // construct action based on input
    }

    public void ProcessInput()
    {

        // queue up any action from effects
        if (current_action == null)
            return;
        // queue up action
        GameManager.instance.actionQueue.Enqueue(current_action);
    }
}
