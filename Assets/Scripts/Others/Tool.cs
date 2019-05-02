using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Others
{
    public class Tool
    {

        private static GameObject camera;
        private static Vector3 cameraScale = Vector3.zero;

        public static bool IsOutOfCameraX(float positionX, float offset = 0)
        {
            bool ret = true;
            float deltaX = positionX - Camera.main.transform.position.x;
            float limitDistanceX = GetCameraScale().x * .5f + offset;
            ret = deltaX * deltaX > limitDistanceX * limitDistanceX;
            return ret;
        }

        public static bool IsOutOfCameraY(float positionY, float offset = 0)
        {
            bool ret = true;
            float deltaY = positionY - Camera.main.transform.position.y;
            float limitDistanceY = GetCameraScale().y * .5f + offset;
            ret = deltaY * deltaY > limitDistanceY * limitDistanceY;
            return ret;
        }

        public static Vector3 GetCameraScale()
        {
            if (cameraScale != Vector3.zero)
            {
                return cameraScale;
            }
            float width = 0;
            float height = 0;
            height = Camera.main.orthographicSize * 2;
            // 摄像机纵横比例
            float aspectRatio = Camera.main.aspect;
            width = height * aspectRatio;
            Vector3 ret = Vector3.right * width + Vector3.up * height;
            return ret;
        }

        public static GameObject GetCamera()
        {
            if (camera == null)
            {
                camera = Camera.main.gameObject;
            }
            return camera;
        }

        public static Vector3 GetSpriteSizeInScene(SpriteRenderer sr)
        {
            return GetSpriteSizeInScene(sr.sprite);
        }

        public static Vector2 GetSpriteSizeInScene(Sprite sprite)
        {
            float spriteHeight = sprite.rect.height;
            float spriteWidth = sprite.rect.width;
            float pixelsPerUnit = sprite.pixelsPerUnit;

            Vector2 ret = Vector2.up * spriteHeight / pixelsPerUnit + Vector2.right * spriteWidth / pixelsPerUnit;
            return ret;
        }

        public static T[] Prepend<T>(T item, T[] sourceArray, bool increaseLength)
        {
            T[] retArray = new T[1] { item };

            if (sourceArray != null && sourceArray.Length > 0)
            {
                int retArrayLength = increaseLength ? sourceArray.Length + 1 : sourceArray.Length;
                retArray = new T[retArrayLength];
                retArray[0] = item;
                for (int i = 1; i < retArray.Length; i++)
                {
                    retArray[i] = sourceArray[i - 1];
                }
            }
            return retArray;
        }

        public static T[] Append<T>(T item, T[] sourceArray, bool increaseLength)
        {
            T[] retArray = new T[1] { item };

            if (sourceArray != null && sourceArray.Length > 0)
            {
                int retArrayLength = increaseLength ? sourceArray.Length + 1 : sourceArray.Length;
                if (increaseLength)
                {
                    retArray = new T[sourceArray.Length + 1];
                    for (int i = 0; i < sourceArray.Length; i++)
                    {
                        retArray[i] = sourceArray[i];
                    }
                }
                else
                {
                    retArray = new T[sourceArray.Length];
                    for (int i = 1; i < sourceArray.Length; i++)
                    {
                        retArray[i - 1] = sourceArray[i];
                    }
                }
                retArray[retArray.Length - 1] = item;
            }

            return retArray;
        }

        public static T[] Append<T>(T[] newArray, T[] sourceArray)
        {
            T[] retArray = new T[newArray.Length + sourceArray.Length];
            for(int i = 0; i < sourceArray.Length; i++)
            {
                retArray[i] = sourceArray[i];
            }

            for(int i = 0; i < newArray.Length; i++)
            {
                retArray[i + sourceArray.Length] = newArray[i];
            }

            return retArray;
        }
    }
}
