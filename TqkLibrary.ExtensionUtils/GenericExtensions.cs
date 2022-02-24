using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.ExtensionUtils
{
    /// <summary>
    /// 
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action.Invoke(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> func)
        {
            foreach (var item in items)
            {
                await func.Invoke(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static async Task<T> FirstAsync<T>(this Task<IEnumerable<T>> tasks) => (await tasks).First();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static async Task<T> FirstOrDefaultAsync<T>(this Task<IEnumerable<T>> tasks) => (await tasks).FirstOrDefault();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static async Task<T> LastAsync<T>(this Task<IEnumerable<T>> tasks) => (await tasks).Last();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static async Task<T> LastOrDefaultAsync<T>(this Task<IEnumerable<T>> tasks) => (await tasks).LastOrDefault();
    }
}
