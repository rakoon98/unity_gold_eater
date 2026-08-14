using GoldEater;
using UnityEngine;

public class HudPanelUI : BasePanel
{

    [SerializeField] GameObject hudPanel;
    [SerializeField] GoldCountUI goldCountUI;
    [SerializeField] HealthCountUI healthCountUI;

    public void Reset()
    {
        healthCountUI.Refresh();
        goldCountUI.Refresh();
    }

    public void Bind(PlayerController controller)
    {
        GoldInventory goldInventory = controller.GetComponent<GoldInventory>(); 
        PlayerHealth playerHealth = controller.GetComponent<PlayerHealth>(); 

        goldCountUI.Bind(goldInventory);
        healthCountUI.Bind(playerHealth);
    }
}
