using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]
public class BalanceController
{

    private TMP_Text _scoreText;
    private string _currenciName = "score";
    private float _countMoneyForOneTile;
    public BalanceController(TMP_Text scoreTex, string currenciName)
    {
        _scoreText = scoreTex;
        _currenciName = currenciName;
    }
    public void IncreaseBalance(float countMoneyForOneTile)
    {
        _countMoneyForOneTile += countMoneyForOneTile;
        _scoreText.text = _currenciName + ": "+ _countMoneyForOneTile;
    }
}
