using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Match3Setting match3Setting;

    [SerializeField] private GridGenerator gridGenerator;

    [SerializeField] private PlayerSetting playerSetting;

    [SerializeField] private TMP_Text scoreText;
    private void Start()
    {
        BalanceController balanceController = new BalanceController(scoreText, playerSetting.name);
        gridGenerator.Init(match3Setting.size, match3Setting.size, match3Setting.animationSpeed, () =>
           {
               balanceController.IncreaseBalance(playerSetting.countMoneyForOneTile);
           });
    }
}
