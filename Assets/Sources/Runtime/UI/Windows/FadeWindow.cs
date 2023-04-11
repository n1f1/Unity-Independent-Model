using System.Collections;
using UnityEngine;

namespace UI.Windows
{
    public class FadeWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _timeToFade = 1;

        public void Open() =>
            StartCoroutine(Fade(true));

        public void Close() =>
            StartCoroutine(Fade(false));

        public void SetClosed(bool closed)
        {
            _canvasGroup.alpha = closed ? 0 : 1;
            _canvasGroup.interactable = !closed;
            _canvasGroup.blocksRaycasts = !closed;
        }

        private IEnumerator Fade(bool active)
        {
            _canvasGroup.interactable = active;
            _canvasGroup.blocksRaycasts = active;

            float time = 0;

            do
            {
                yield return null;
                time += Time.deltaTime;
                float fadeProgress = time / _timeToFade;
                _canvasGroup.alpha = Mathf.Lerp(0, 1, active ? fadeProgress : 1 - fadeProgress);
            } while (time < _timeToFade);
        }
    }
}