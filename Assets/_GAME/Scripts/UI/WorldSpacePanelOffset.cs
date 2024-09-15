using System;
using UnityEngine;

public class WorldSpacePanelOffset : MonoBehaviour
{
    [SerializeField] private Anchor _anchor;
    public Anchor Anchor => _anchor;
}
[Serializable]
public class Anchor
{
    public Vector3 Scale = Vector3.one;
    public Vector3 EulerAngles = Vector3.zero;
    public Vector3 Position = Vector3.one;
}
