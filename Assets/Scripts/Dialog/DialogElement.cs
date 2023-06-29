using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class DialogElement
{
    [TextArea(3,10)]
    public string sentence;
    public Color sentenceColor;
    public AudioClip sentenceAudio;
    public Sprite characterImage;
    public bool placeImageToTheRight = false;
    public UnityEvent afterDialogAction = new UnityEvent();
}
