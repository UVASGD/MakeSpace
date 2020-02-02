using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Action : MonoBehaviour, IComparable<Action>
{
    public float priority; // smaller values are higher priority

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CompareTo(Action other)
    {
        if (this.priority < other.priority) return -1;
        else if (this.priority > other.priority) return 1;
        else return 0;
    }

    public virtual bool Resolve()
    {
        return true;
    }
}
