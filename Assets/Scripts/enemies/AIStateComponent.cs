using System;
using UnityEngine;

[Serializable]
public class AIStateComponent : MonoBehaviour
{
    public AIState state;
}

[Serializable]
public enum AIState { Idle, Seek, Flee, Arrive, Pursuit, Evade, Attack }

