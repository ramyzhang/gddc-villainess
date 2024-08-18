using System;
using UnityEngine;

public class AnimatorReskinner : MonoBehaviour
{
    public Character character;
    private Sprite[] npcSprites;
    private Sprite[] constantSprites;
    // Start is called before the first frame update
    void Start()
    {
        npcSprites = Resources.LoadAll<Sprite>($"Sprites/Characters/{character}");
        constantSprites = Resources.LoadAll<Sprite>("Sprites/Characters/FemaleConstants");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            var newSprite = Array.Find(npcSprites, sprite => sprite.name == renderer.sprite.name);

            // If this is not not in the npc sprite folder, check the body constants
            if (newSprite == null)
            {
                newSprite = Array.Find(constantSprites, sprite => sprite.name == renderer.sprite.name);
            }

            if (newSprite != null)
            {
                renderer.sprite = newSprite;
            }
        }
    }
}
