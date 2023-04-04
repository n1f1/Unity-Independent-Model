using System.Collections;
using UnityEngine;

namespace UI
{
    public class FadeWindow : MonoBehaviour, IWindow
    {
        private const int TimeToFade = 1;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void Open() => 
            StartCoroutine(Fade(true));

        public void Close() => 
            StartCoroutine(Fade(false));

        private IEnumerator Fade(bool active)
        {
            float time = 0;

            while (time < TimeToFade)
            {
                yield return null;
                time += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(0, 1, active ? time : 1 / time);
            }
            
            gameObject.SetActive(active);
        }
    }
}