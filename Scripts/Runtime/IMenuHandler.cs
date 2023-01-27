namespace Vulpes.Menus
{
    public interface IMenuHandler : IMenuStack
    {
        bool Visible { get; set; }

        bool CursorLocked { get; set; }

        IMenuAlert Alert { get; }

        IMenuModal Modal { get; }

        IMenuLoading Loading { get; }

        T GetScreen<T>() where T : IMenuScreen;
    }
}