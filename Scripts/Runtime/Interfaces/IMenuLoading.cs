using System;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public interface IMenuLoading
    {
        /// <summary>
        /// Returns a <see cref="IPromise"/> that resolves when the completion predicate evaluates as true.
        /// </summary>
        /// <param name="loadOperation"></param>
        /// <param name="completionPredicate"></param>
        /// <param name="progressPredicate"></param>
        /// <returns></returns>
        IPromise Show(Action loadOperation, Func<bool> completionPredicate, Func<float> progressPredicate);
    }
}