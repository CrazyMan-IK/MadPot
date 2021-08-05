using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    [Serializable]
    public class LevelProduct
    {
        [field: SerializeField] public Product Product { get; set; } = null;
        [field: SerializeField] public Vector2 Position { get; set; } = Vector2.zero;

        public Vector3 GetTargetPosition(Camera camera)
        {
            var mainCamera = Camera.main.transform;
            var center = mainCamera.position + mainCamera.forward / 2;

            var result = Position - (Position / 2);

            return center + camera.transform.rotation * result;
        }
    }
}
