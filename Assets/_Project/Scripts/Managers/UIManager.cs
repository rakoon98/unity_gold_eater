using GoldEater;
using UnityEngine;

public class UIManager : BaseSingletonManager<UIManager>
{

    [Header("Panels")]
    [SerializeField] private StatusPanelUI statPanel;
    [SerializeField] private GameOverPanelUI gameOverPanel;
    [SerializeField] private HudPanelUI hudPanel;
    [SerializeField] private DashUI dashPanel;
    [SerializeField] private BossHealthUI bossHealthPanel;
    [SerializeField] private StageClearUI stageClearUI;

    public HudPanelUI HUD => hudPanel;
    public StatusPanelUI Status => statPanel;
    public GameOverPanelUI GameOver => gameOverPanel;
    public DashUI Dash => dashPanel;
    public BossHealthUI bossHealthUI => bossHealthPanel;
    public StageClearUI StageClear => stageClearUI;

    public void ToggleStatPanel()
    {
        statPanel.Toggle();
    }

    public void SpawnDamagePopup(Vector3 position, float damage, bool isCritical = false)
    {
        DamagePopupSpawner.Instance.Spawn(position, damage, isCritical);
    }

    public void SetSkillDashConnect(PlayerController controller)
    {
        Dash.SetController(controller);
    }

    public void Reset()
    {
        Status.Reset();
        HUD.Reset();
       
        //// dash √ ±‚»≠?
        //dashHUD.Refresh();
    }

    public void RebindPlayer(PlayerController controller)
    {
        //Status.Bind(controller.GetComponent<StatComponent>());
        //Dash.SetController(controller);
        //HUD.Reset();

        Status.Bind(controller.GetComponent<StatComponent>());
        Dash.SetController(controller);

        HUD.Bind(controller);
    }

    

}
