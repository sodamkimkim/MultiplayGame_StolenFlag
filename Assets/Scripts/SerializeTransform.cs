using ExitGames.Client.Photon;
using UnityEngine;

public class SerializeTransform
{
    // 4 * 3 + 4 * 4 + 4 * 3
    public float[] _position = new float[3];
    public float[] _rotation = new float[4];
    public float[] _scale = new float[3];

    public  SerializeTransform(Transform transform)
    {
        _position[0] = transform.localPosition.x;
        _position[1] = transform.localPosition.y;
        _position[2] = transform.localPosition.z;

        _rotation[0] = transform.localRotation.w;  
        _rotation[1] = transform.localRotation.x;  
        _rotation[2] = transform.localRotation.y;  
        _rotation[3] = transform.localRotation.z;

        _scale[0] = transform.localScale.x;
        _scale[1] = transform.localScale.y;
        _scale[2] = transform.localScale.z;
    }
} // end of class
