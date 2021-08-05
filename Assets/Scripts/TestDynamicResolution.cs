using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    public class TestDynamicResolution : MonoBehaviour
	{
		public Camera camera;

        void Update()
		{
			float designWidth = 1080f;//Wide resolution during development
			float designHeight = 2246f;//High resolution during development
			float designFOV = 60f;//Orthogonal camera Size during development
			float designScale = designWidth / designHeight;
			float scaleRate = (float)Screen.width / Screen.height;

			//The current resolution is greater than the development resolution, it will be automatically scaled, if it is less than the resolution, you need to manually process
			if (scaleRate < designScale)
			{
				float scale = scaleRate / designScale;
				camera.fieldOfView = designFOV / scale;
			}
			else
			{
				camera.fieldOfView = designFOV;
			}
		}
	}
}
