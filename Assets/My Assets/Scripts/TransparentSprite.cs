using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentSprite : MonoBehaviour
{
    public SpriteRenderer transparentSpriteRenderer;
    public float transparencyLevel;

    private void Start()
    {
        transparentSpriteRenderer.color = new Color(1f, 1f, 1f, transparencyLevel);
    }
}
