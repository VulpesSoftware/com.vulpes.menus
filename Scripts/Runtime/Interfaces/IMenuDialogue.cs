using System;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public enum MenuDialogueResult
    {
        /// <summary>The result when the 'Confirm' button is pressed.</summary>
        Confirm = 0,
        /// <summary>The result when the 'Cancel' button are pressed.</summary>
        Cancel = 1,
        /// <summary>The result when the 'Alternate' button is pressed.</summary>
        Alternate = 2,
    }

    public interface IMenuDialogue
    {
        IPromise<MenuDialogueResult> Show(string asTitleText, string asBodyText, string asConfirmText, Action akOnConfirm = null);

        IPromise<MenuDialogueResult> Show(string asTitleText, string asBodyText, string asConfirmText, string asCancelText, Action akOnConfirm = null, Action akOnCancel = null);

        IPromise<MenuDialogueResult> Show(string asTitleText, string asBodyText, string asConfirmText, string asCancelText, string asAlternateText, Action akOnConfirm = null, Action akOnCancel = null, Action akOnAlternate = null);
    }
}
