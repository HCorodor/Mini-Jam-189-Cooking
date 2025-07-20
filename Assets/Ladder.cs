using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Collider2D topTrigger;
    public Collider2D bottomTrigger;

    public bool IsAtTop(Vector2 playerPosition)
    {
        return topTrigger.OverlapPoint(playerPosition);
    }

    public bool IsAtBottom(Vector2 playerPosition)
    {
        return bottomTrigger.OverlapPoint(playerPosition);
    }
}