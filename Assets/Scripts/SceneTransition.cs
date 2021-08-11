using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace MiniPuzzles.Firefighter
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private Image _fadeImage = null;

        public void LoadScene(int index)
        {
            StartCoroutine(FadeToScene(SceneManager.GetSceneByBuildIndex(index).name));
        }

        public void LoadScene(SceneReference target)
        {
            StartCoroutine(FadeToScene(target));
        }

        private IEnumerator FadeToScene(string sceneName)
        {
            yield return _fadeImage.DOFade(1, 0.5f).SetEase(Ease.OutQuad).WaitForCompletion();

            Camera.main.gameObject.SetActive(false);

            var currentScene = SceneManager.GetActiveScene();
            foreach (var root in currentScene.GetRootGameObjects().Where(x => x.transform != transform.root))
            {
                root.SetActive(false);
            }

            var scene = SceneManager.LoadScene(sceneName, new LoadSceneParameters(LoadSceneMode.Additive));

            while (!scene.isLoaded)
            {
                yield return null;
            }

            yield return _fadeImage.DOFade(0, 0.5f).SetEase(Ease.InQuad).WaitForCompletion();

            SceneManager.SetActiveScene(scene);
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}
