using UnityEngine;
using UnityEngine.UI;

namespace GoldEater
{
    public class DashUI : MonoBehaviour
    {
        private PlayerController player;

        [SerializeField] private Image overlay;

        private float cooldown;
        private float remain;
        private bool isCooldown;


        public void SetController(PlayerController controller)
        {
            // 기존 Player 이벤트 해제
            if (player != null)
            {
                player.OnDashCooldownStarted -= StartCooldown;
                player.OnDashReady -= DashReady;
            }

            // 새 Player 연결
            player = controller;

            // 새 Player 이벤트 구독
            if (player != null)
            {
                player.OnDashCooldownStarted += StartCooldown;
                player.OnDashReady += DashReady;
            }

            ResetUI();
        }


        private void Update()
        {
            if (!isCooldown)
                return;

            remain -= Time.deltaTime;

            overlay.fillAmount = remain / cooldown;

            if (remain <= 0f)
            {
                remain = 0f;
                overlay.fillAmount = 0f;
                isCooldown = false;
            }
        }


        private void StartCooldown(float cd)
        {
            cooldown = cd;
            remain = cd;
            isCooldown = true;

            overlay.fillAmount = 1f;
        }


        private void DashReady()
        {
            // 띵!
            // 반짝!
        }


        private void ResetUI()
        {
            cooldown = 0f;
            remain = 0f;
            isCooldown = false;

            if (overlay != null)
                overlay.fillAmount = 0f;
        }


        private void OnDestroy()
        {
            if (player != null)
            {
                player.OnDashCooldownStarted -= StartCooldown;
                player.OnDashReady -= DashReady;
            }
        }
    }
}