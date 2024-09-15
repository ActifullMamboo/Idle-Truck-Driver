using _Game.Scripts.Ui.Base;

namespace _GAME.Scripts.UI.MoveTargets
{
    public enum UITargetType
    {
        none,
        Money,
        Stars
    }
    public class UIMoveTarget : BaseUIView
    {
        public UITargetType targetType;
    }
}
