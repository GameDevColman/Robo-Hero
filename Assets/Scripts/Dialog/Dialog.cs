using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class Dialog
{
    public DialogElement[] sentences;
    public UnityEvent afterDialogAction = new UnityEvent();
}
