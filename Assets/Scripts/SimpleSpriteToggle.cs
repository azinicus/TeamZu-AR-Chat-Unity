using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleSpriteToggle : MonoBehaviour
{
    [SerializeField]
    Image[] targetGraphics;
    [SerializeField]
    Sprite[] offSprites;
    [SerializeField]
    Sprite[] onSprites;

    //private variables
    bool itsOn = false;

    public void SpriteToggle()
    {
        if (itsOn)
        {
            if(targetGraphics[0] != null)
                targetGraphics[0].sprite = offSprites[0];
            if (targetGraphics[1] != null)
                targetGraphics[1].sprite = offSprites[1];
            itsOn = false;
        }
        else
        {
            if (targetGraphics[0] != null)
                targetGraphics[0].sprite = onSprites[0];
            if (targetGraphics[1] != null)
                targetGraphics[1].sprite = onSprites[1];
            itsOn = true;
        }

    }
}
