using System.Collections;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Utilities;

namespace UI {
    public class MainMenu : MonoBehaviour {
        private const string VOLUME = "Volume";
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Image _fader;

        public void Play() {
            StartCoroutine(PlayFade());
        }

        public void Volume(float value) {
            _mixer.SetFloat(VOLUME, 20 * Mathf.Log10(value));
        }

        public void Quit() {
            StartCoroutine(QuitFade());
        }

        private IEnumerator PlayFade() {
            float timer = 0f;
            while (timer <= 1) {
                timer += Time.fixedDeltaTime;
                _fader.color = Color.Lerp(Color.clear, Color.black, timer);
                yield return Yielders.WaitForFixedUpdate;
            }
            _fader.color = Color.black;
            SceneManager.LoadScene(1);
        }

        private IEnumerator QuitFade() {
            float timer = 0f;
            while (timer <= 1) {
                timer += Time.fixedDeltaTime;
                _fader.color = Color.Lerp(Color.clear, Color.black, timer);
                yield return Yielders.WaitForFixedUpdate;
            }
            _fader.color = Color.black;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}