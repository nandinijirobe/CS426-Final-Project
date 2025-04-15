using TMPro;
using UnityEngine;

public class GameGeneralManager : MonoBehaviour
{
    int numAuditions;
    public TMP_Text auditionCount;

    public void updateAuditionCount()
    {
        if (numAuditions < 5) {
            numAuditions++;
            auditionCount.text = "Auditions: " + numAuditions + "/5";
        }
    }
}
