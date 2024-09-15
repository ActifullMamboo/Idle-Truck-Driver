using _GAME.Scripts.Other;
using UnityEngine;
using UnityEngine.UI;

public enum TypeOfButton
{
    Restart,
    Fail,
    Next,
    Start
}
[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    private Button _button;
    
    public TypeOfButton _type;
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => Manage());
    }

    private void Manage()
    {
        switch (_type)
        {
            case TypeOfButton.Restart:
                LevelManager.RestartLevel();
                break;
            case TypeOfButton.Next:
                LevelManager.Win();
                break;
            case TypeOfButton.Fail:
                LevelManager.Reload();
                break;
        }
    }
}