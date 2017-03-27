using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private Sprite damageSprite;
    [SerializeField]
    private int hitPoints = 4;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    internal void DamageWall(int loss)
    {
        spriteRenderer.sprite = damageSprite;
        hitPoints -= loss;
        if(hitPoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
