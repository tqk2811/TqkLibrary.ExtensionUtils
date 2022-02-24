using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqkLibrary.ExtensionUtils
{
    /// <summary>
    /// 
    /// </summary>
    public static class AsynchronousExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<TResult> UnWrap<T,TResult>(this Task<T> task,Func<T,TResult> func)
        {
            T t = await task;
            return func(t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task UnWrap<T>(this Task<T> task, Action<T> action)
        {
            T t = await task;
            action(t);
        }
    }
}
