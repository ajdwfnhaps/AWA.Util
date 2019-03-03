using System;
using System.Threading.Tasks;

namespace AWA.Util.Cache
{
    /// <summary>
    /// Cache manager interface
    /// </summary>
    public partial interface ICacheManager : IDisposable
    {
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        T Get<T>(string key);



        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool Set<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        void Remove(string key);


        /// <summary>
        /// Clear all cache data
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool KeyExists(string key);

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newKey"></param>
        /// <returns></returns>
        bool KeyRename(string key, string newKey);

        /// <summary>
        /// 设置Key的时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool KeyExpire(string key, TimeSpan expiry);


        /// <summary>
        /// 获取一个key的对象,如果没有即写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="createFn"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        T TryGet<T>(string key, Func<T> createFn = null, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 获取一个key的对象,如果没有即写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="createFn"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<T> TryGetAsync<T>(string key, Func<Task<T>> createFn = null, TimeSpan? expiry = default(TimeSpan?));

    }

}
