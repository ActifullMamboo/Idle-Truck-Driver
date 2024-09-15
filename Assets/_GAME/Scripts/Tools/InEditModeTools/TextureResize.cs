using UnityEngine;
using System.ComponentModel;
using System;
using UnityEditor;

[ExecuteInEditMode]
public class TextureResize : MonoBehaviour
{
    public enum Coords
    {
        XY,
        XZ,
        YZ,
    }
#if UNITY_EDITOR


    public Vector2 ScaleFactor = Vector2.one*5;
    public Coords coords;
    

    void Update()
    {

        if ( Application.isEditor && !Application.isPlaying)
        {
            if (coords ==  Coords.XY)
            {
                GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x / ScaleFactor.x, transform.localScale.y / ScaleFactor.y);

            }
            else if (coords == Coords.XZ)
            {
                GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x / ScaleFactor.x, transform.localScale.z / ScaleFactor.y);

            }
            if (coords == Coords.YZ)
            {
                GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.y / ScaleFactor.x, transform.localScale.z / ScaleFactor.y);

            }
        }

    }
    private void OnValidate()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            if (coords == Coords.XY)
            {
                GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x / ScaleFactor.x, transform.localScale.y / ScaleFactor.y);

            }
            else if (coords == Coords.XZ)
            {
                GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x / ScaleFactor.x, transform.localScale.z / ScaleFactor.y);

            }
            if (coords == Coords.YZ)
            {
                GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.y / ScaleFactor.x, transform.localScale.z / ScaleFactor.y);

            }
        }
    }
#endif

}