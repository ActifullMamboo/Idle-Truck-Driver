using _Game.Scripts.View;

namespace _GAME.Scripts.UI.CarPick
{
    public enum PanelType
    {
        Stats,
        Level
    }
    public class BasePanel : BaseView
    {
        public PanelType panelType;
        public virtual void SetupPanel(params object[] list)
        {
            
        }
    }
}
