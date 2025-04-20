using TMPro;
using UnityEngine;

public class GameGeneralManager : MonoBehaviour
{
    int numAuditions;

    int cash;
    public TMP_Text auditionCount;
    public PlayerMoneyManager moneyManager;

    public void updateAuditionCount()
    {
        if (numAuditions < 5) {
            numAuditions++;
            cash += 100;

            auditionCount.text = "Auditions: " + numAuditions + "/5";
            moneyManager.AddMoney(100);
        }
    }
}
