using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerManager : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Place the sticker at the given location!
    public void stickyStick(Sprite sticker, Vector3 location, string name) {
        gameObject.name = name;
        transform.position = location;
        sr.sprite = sticker;
    }
}
