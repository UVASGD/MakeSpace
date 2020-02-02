using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


        /*
        public static T Choose<T>(Dictionary<T, float> probs)
        {

            Dictionary<int, T> 
            float total = 0;

            foreach (float elem in probs.Keys)
            {
                total += elem;
            }

            float randomPoint = Random.value * total;

            for (int i = 0; i < probs.Keys.Count; i++)
            {
                if (randomPoint < probs[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= probs[i];
                }
            }
            return probs.Length - 1;
        }
        */
    }

public static class DirectionTransform
{
    public static Dictionary<Direction, Vector2Int> dir_to_trans = new Dictionary<Direction, Vector2Int>()
    {
        { Direction.North, Vector2Int.up },
        { Direction.East, Vector2Int.right },
        { Direction.South, Vector2Int.down },
        { Direction.West, Vector2Int.left },
        { Direction.Center, Vector2Int.zero },
    };
    public static Dictionary<Vector2Int, Direction> trans_to_dir = new Dictionary<Vector2Int, Direction>()
    {
        { Vector2Int.up, Direction.North },
        { Vector2Int.right, Direction.East },
        { Vector2Int.down, Direction.South },
        { Vector2Int.left, Direction.West },
        { Vector2Int.zero, Direction.Center },
    };

    public static Direction Reverse(Direction dir)
    {
        return trans_to_dir[-dir_to_trans[dir]];
    }

    public static Dictionary<Direction, Orientation> dir_to_orient = new Dictionary<Direction, Orientation>()
    {
        {Direction.North, Orientation.Horizontal },
        {Direction.South, Orientation.Horizontal },
        {Direction.East, Orientation.Vertical },
        {Direction.West, Orientation.Vertical }
    };
}

public static class TransformDeepChildExtension
{
    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }
}