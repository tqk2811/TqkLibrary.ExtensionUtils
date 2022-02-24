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
    public static class ParallelExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="itemPerThread"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task ParallelWorkAsync<T>(this IList<T> list, int itemPerThread, Action<T> action)
        {
            if (list == null || list.Count == 0) return;
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (itemPerThread <= 0) throw new InvalidOperationException("itemPerThread must be >0");

            int pageCount = (int)Math.Ceiling((double)list.Count / itemPerThread);
            if (pageCount == 1)
            {
                foreach (var item in list) action.Invoke(item);
            }
            else
            {
                Task[] tasks = new Task[pageCount];
                for (int i = 0; i < pageCount; i++)
                {
                    var page = list.Skip(itemPerThread * i).Take(itemPerThread);
                    tasks[i] = Task.Factory.StartNew(() => { foreach (var item in page) action.Invoke(item); }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
                }
                await Task.WhenAll(tasks);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="itemPerThread"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task ParallelWorkAsync<T>(this IList<T> list, int itemPerThread, Func<T, Task> func)
        {
            if (list == null || list.Count == 0) return;
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (itemPerThread <= 0) throw new InvalidOperationException("itemPerThread must be >0");

            int pageCount = (int)Math.Ceiling((double)list.Count / itemPerThread);
            if (pageCount == 1)
            {
                foreach (var item in list) await func.Invoke(item);
            }
            else
            {
                Task[] tasks = new Task[pageCount];
                for (int i = 0; i < pageCount; i++)
                {
                    var page = list.Skip(itemPerThread * i).Take(itemPerThread);
                    tasks[i] = Task.Factory.StartNew(() => { foreach (var item in page) func.Invoke(item).Wait(); }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
                }
                await Task.WhenAll(tasks);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="list"></param>
        /// <param name="itemPerThread"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task<IEnumerable<TResult>> ParallelWorkAsync<T, TResult>(this IList<T> list, int itemPerThread, Func<IEnumerable<T>, TResult> func)
        {
            if (list == null || list.Count == 0) return new List<TResult>();
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (itemPerThread <= 0) throw new InvalidOperationException("itemPerThread must be >0");

            List<TResult> results = new List<TResult>();

            int pageCount = (int)Math.Ceiling((double)list.Count / itemPerThread);
            if (pageCount == 1)
            {
                results.Add(func.Invoke(list));
            }
            else
            {
                Task<TResult>[] tasks = new Task<TResult>[pageCount];
                for (int i = 0; i < pageCount; i++)
                {
                    var page = list.Skip(itemPerThread * i).Take(itemPerThread);
                    tasks[i] = Task.Factory.StartNew(() => func.Invoke(page), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
                }
                await Task.WhenAll(tasks);
                results.AddRange(tasks.Select(x => x.Result));
            }
            return results;
        }
    }
}
