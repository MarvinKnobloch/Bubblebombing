using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite left;
    public Sprite up;
    public Sprite right;
    public Sprite down;

    public void Face(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                SetActiveSprite(left);
                break;
            case Direction.Up:
                SetActiveSprite(up);
                break;
            case Direction.Right:
                SetActiveSprite(right);
                break;
            case Direction.Down:
                SetActiveSprite(down);
                break;
        }
    }

    private void SetActiveSprite(Sprite activeSprite)
    {
        spriteRenderer.sprite = activeSprite;
    }
}
