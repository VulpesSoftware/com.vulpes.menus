﻿using System;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public enum MenuModalResult : int
    {
        /// <summary>The result when the 'Confirm' button is pressed.</summary>
        Confirm,
        /// <summary>The result when the 'Cancel' button are pressed.</summary>
        Cancel,
        /// <summary>The result when the 'Alternate' button is pressed.</summary>
        Alternate,
    }

    public interface IMenuModal
    {
        IPromise<MenuModalResult> Show(in string title, in string body, in string confirm, Action onConfirm = null);

        IPromise<MenuModalResult> Show(in string title, in string body, in string confirm, in string cancel, Action onConfirm = null, Action onCancel = null);

        IPromise<MenuModalResult> Show(in string title, in string body, in string confirm, in string cancel, in string alternate, Action onConfirm = null, Action onCancel = null, Action onAlternate = null);
    }
}