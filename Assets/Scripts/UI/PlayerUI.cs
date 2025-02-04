using UnityEngine;

using TMPro;

using Utilities;
using UnityEngine.UI;
using Entity;
using Entity.Player;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace UI {
    public class PlayerUI : Singleton<PlayerUI> {
        public TMP_Text SizeModifier;
        public TMP_Text SpeedModifier;
        public TMP_Text DamageModifier;
        public TMP_Text AmourModifier;
        public TMP_Text RegenModifier;
        public TMP_Text HealthReadout;
        public TMP_Text RoomReadout;
        public TMP_Text KeyReadout;
        public CanvasGroup RoomUICanvas;
        public Image HeathBar;
        public Image AttackIndicator;
        public CanvasGroup DeathScreen;
        public CanvasGroup WinScreen;
        public Image Fader;
        public GameObject DamageNumberPrefab;
        public Transform WorldCanvas;

        private void Start() {
            Fader = FindObjectOfType<AutoFader>(true).GetComponent<Image>();
            RoomUICanvas.FadeCanvas(0.1f, true, this);
        }

        public void ToggleRoomReadout(bool showReadout) {
            RoomUICanvas.FadeCanvas(0.5f, !showReadout, this);
        }

        public void ShowDeathScreen() {
            DeathScreen.FadeCanvas(0.5f, false, this);
            StartCoroutine(DeathScreenFader());
        }

        private IEnumerator DeathScreenFader() {
            yield return Yielders.WaitForSeconds(1f);
            float timer = 0f;
            while (timer < 1f) {
                timer += Time.fixedDeltaTime;
                yield return Yielders.WaitForFixedUpdate;
                Fader.color = Color.Lerp(Color.clear, Color.black, timer);
            }
            SceneManager.LoadScene(0);
        }

        public void PlayerFinish() {
            WinScreen.FadeCanvas(0.5f, false, this);
        }

        private IEnumerator WinScreenFade() {
            yield return Yielders.WaitForSeconds(1f);
            float timer = 0f;
            while (timer < 1f) {
                timer += Time.fixedDeltaTime;
                yield return Yielders.WaitForFixedUpdate;
                Fader.color = Color.Lerp(Color.clear, Color.black, timer);
            }
            SceneManager.LoadScene(0);
        }
    }
}