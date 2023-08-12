using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart : MonoBehaviour
{

    [SerializeField] private GameObject BattleCamera, MainCamera;
    public BattleDialogBox battle;
    public PlayerMovement Player;
    public PlayerHealth PlayerHP;
    //public bool defeated = false;
    void Start()
    {
        BattleCamera.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerHP.currentHealth > 0)
        {
            BattleCamera.SetActive(true);
            MainCamera.SetActive(false);
            Player.DisablePlayer(true);
            battle.onStart();
        }
    }

    public void startBattle()
    {
        if(PlayerHP.currentHealth > 0)
        {
            BattleCamera.SetActive(true);
            MainCamera.SetActive(false);
            Player.DisablePlayer(true);
            battle.onStart();
            battle.rematch.SetActive(false);
        }
    }

}
