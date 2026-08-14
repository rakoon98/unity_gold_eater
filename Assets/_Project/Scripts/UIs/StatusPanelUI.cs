using GoldEater;
using TMPro;
using UnityEngine;

public class StatusPanelUI : MonoBehaviour
{
    [SerializeField] RectTransform panel;
    [SerializeField] private StatComponent playerStat;

    [Header("스탯 텍스트 연결")]
    [SerializeField] private TextMeshProUGUI goldCount;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [SerializeField] private TextMeshProUGUI jumpSpeedText;
    [SerializeField] private TextMeshProUGUI dashSpeedText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI critChanceText;
    [SerializeField] private TextMeshProUGUI critDamageText;

    bool opened = false;

    public void Toggle()
    {
        opened = !opened;
        panel.anchoredPosition = opened ? new Vector2(20, 0) : new Vector2(-450, 0);

        if (opened)
        {
            if (playerStat == null)
            {
                FindPlayerStat();
                Subscribe(); 
            }

            RefreshAll();
        }
    }

    private void Subscribe()
    {
        if (playerStat != null)
            playerStat.OnStatChanged += HandleStatChanged; 
    }

    private void Unsubscribe()
    {
        if (playerStat != null)
            playerStat.OnStatChanged -= HandleStatChanged;
    }

    private void FindPlayerStat()
    {
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerStat = playerHealth.GetComponent<StatComponent>();
        }
    }


    private void OnEnable()
    {
        Subscribe(); 
    }

    private void OnDisable()
    {
        Unsubscribe();
    }


    private void HandleStatChanged(StatType type, float value)
    {
        RefreshAll();
    }

    private void RefreshAll()
    {
        attackText.text = $"공격력: {playerStat.GetStat(StatType.Attack):F1}";
        moveSpeedText.text = $"이동속도: {playerStat.GetStat(StatType.MoveSpeed):F1}";
        jumpSpeedText.text = $"점프력: {playerStat.GetStat(StatType.JumpSpeed):F1}";
        dashSpeedText.text = $"대시속도: {playerStat.GetStat(StatType.DashSpeed):F1}";
        attackSpeedText.text = $"공격속도: {playerStat.GetStat(StatType.AttackSpeed):F1}";
        critChanceText.text = $"크리티컬 확률: {playerStat.GetStat(StatType.CritChance):F1}%";
        critDamageText.text = $"크리티컬 데미지: {playerStat.GetStat(StatType.CritDamage):F1}배";         
    }

    public void Reset()
    {
        if (playerStat == null)
            return;

        playerStat.ResetModifiers();
        RefreshAll();
    }

    public void Bind(StatComponent stat)
    {
        Unsubscribe();

        playerStat = stat;

        Subscribe();

        RefreshAll();
    }

}