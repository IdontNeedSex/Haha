using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndcardScript : MonoBehaviour
{
    [SerializeField] private EndcardData _endcardData;

    [SerializeField] private TMP_Text _isWonTxt;
    [SerializeField] private string _wonText = "YOU WIN!";
    [SerializeField] private string _looseText = "YOU LOST!";
    
    void Start()
    {
        _isWonTxt.text = _endcardData.isWon ? _wonText : _looseText;
    }
}
