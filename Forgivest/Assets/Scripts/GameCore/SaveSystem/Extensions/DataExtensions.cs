using GameCore.SaveSystem.Data.Types;
using UnityEngine;

namespace GameCore.SaveSystem.Extensions
{
    public static class DataExtensions
    {
        public static Vector3Data AsVector3Data(this Vector3 vector3) =>
            new Vector3Data(vector3.x, vector3.y, vector3.z);
        
        public static Vector3 AsUnityVector(this Vector3Data vector3Data) =>
            new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);
        
        public static QuaternionData AsQuaternionData(this Quaternion quaternionData) =>
            new QuaternionData(quaternionData.x, quaternionData.y, quaternionData.z, quaternionData.w);

        public static Quaternion AsUnityQuaternion(this QuaternionData quaternionData) =>
            new Quaternion(quaternionData.X, quaternionData.Y, quaternionData.Z, quaternionData.W);
        
        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);
        
        public static T ToDeserializedObject<T>(this string serializedObject) =>
            JsonUtility.FromJson<T>(serializedObject);
    }
}