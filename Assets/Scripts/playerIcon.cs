using UnityEngine;

public class playerIcon : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI label;

    public void Set(string scheme)
    {
        switch(scheme)
        {
            case "KB1":
                label.text = "WASD";
                break;
            case "KB2":
                label.text = "Arrow Keys";
                break;
            case "KB3":
                label.text = "IJKL";
                break;
            case "KB4":
                label.text = "Numpad 1235";
                break;
        }
    }
}
