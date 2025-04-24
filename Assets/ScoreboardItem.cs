using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreBoardItem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text winsText;

    public void Initialize(PlayerInfo player)
    {
        nameText.text = player.GetName();
    }
}
