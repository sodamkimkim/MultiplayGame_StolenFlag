//using UnityEngine.UI;

//using System;
//using System.IO;
//using System.Text;
//using UnityEngine;
//using Photon.Pun;
//using ExitGames.Client.Photon;
//using UnityEngine.UIElements;

//[Serializable]
//public class TransformSerialization : MonoBehaviour
//{
//    //// 4 * 3 + 4 * 4 + 4 * 3
//    //public float[] _position = new float[3];
//    //public float[] _rotation = new float[4];
//    //public float[] _scale = new float[3];

//    //public TransformSerialization(Transform transform)
//    //{
//    //    _position[0] = transform.localPosition.x;
//    //    _position[1] = transform.localPosition.y;
//    //    _position[2] = transform.localPosition.z;

//    //    _rotation[0] = transform.localRotation.w;  
//    //    _rotation[1] = transform.localRotation.x;  
//    //    _rotation[2] = transform.localRotation.y;  
//    //    _rotation[3] = transform.localRotation.z;

//    //    _scale[0] = transform.localScale.x;
//    //    _scale[1] = transform.localScale.y;
//    //    _scale[2] = transform.localScale.z;
//    //}
//    public static byte[] transformMemory = new byte[40];
//    public static short SerializeTransform(StreamBuffer outStream, object targetObject)
//    {
//        Transform transform = (Transform)targetObject;
//        lock(transformMemory)
//        {
//            byte[] bytes = transformMemory;
//            int index = 0;
//            Protocol.Serialize(transform.position.x, bytes, ref index);
//            Protocol.Serialize(transform.position.y, bytes, ref index);
//            Protocol.Serialize(transform.position.z, bytes, ref index);
//            Protocol.Serialize(transform.rotation.w, bytes, ref index);
//            Protocol.Serialize(transform.rotation.x, bytes, ref index);
//            Protocol.Serialize(transform.rotation.y, bytes, ref index);
//            Protocol.Serialize(transform.rotation.z, bytes, ref index);
//            Protocol.Serialize(transform.localScale.x, bytes, ref index);
//            Protocol.Serialize(transform.localScale.y, bytes, ref index);
//            Protocol.Serialize(transform.localScale.z, bytes, ref index);
//            outStream.Write(bytes, 0, 40);

//        }
//        return 40;
        
//    }
//    public  object DeserializeTransform(StreamBuffer inStream, short length)
//    {
//        Transform transform = gameObject.GetComponent<Transform>();
//        lock(transformMemory)
//        {
//            inStream.Read(transformMemory, 0, 40);
//            int index = 0;
//            Protocol.Deserialize(out transform.position.x, transformMemory, ref index);
//            Protocol.Deserialize(out transform.position.y, transformMemory, ref index);
//            Protocol.Deserialize(out transform.position.z, transformMemory, ref index);
//            Protocol.Deserialize(out transform.rotation.x, transformMemory, ref index);
//        }
//    }

//} // end of class
