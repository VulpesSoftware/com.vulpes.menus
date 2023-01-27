using Vulpes.Promises;

namespace Vulpes.Menus
{
    public enum MenuModalResult : int
    {
        Confirm,
        Cancel,
        Alternate,
    }

    public interface IMenuModal
    {
        IPromise<MenuModalResult> Show(in string title, in string body, in string confirm);

        IPromise<MenuModalResult> Show(in string title, in string body, in string confirm, in string cancel);

        IPromise<MenuModalResult> Show(in string title, in string body, in string confirm, in string cancel, in string alternate);
    }
}