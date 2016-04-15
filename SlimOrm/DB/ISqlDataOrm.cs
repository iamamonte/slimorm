using System.Collections.Generic;

namespace SlimOrm.DB
{
    /// <summary>
    /// Lightweight ORM
    /// </summary>
    public interface ISqlDataOrm
    {
        /// <summary>
        /// The factory moderating database connections.
        /// </summary>
        IDbConnectionService DbConnectionService { get; }

        /// <summary>
        /// Updates a  <typeparamref name="T"/> record in a database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        T Update<T>(T obj) where T : DataBase;
        /// <summary>
        /// Creates a generic <typeparamref name="T"/> in the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        T Create<T>(T obj) where T : DataBase;
        /// <summary>
        /// Retrieves a <typeparamref name="T"/> from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Retrive<T>(T obj) where T : DataBase;
        /// <summary>
        /// Removes <typeparamref name="T"/> from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        int Purge<T>(T obj) where T : DataBase;

        /// <summary>
        /// Retrieves generic object from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        List<T> GetWithQuery<T>(string query, object paramObject) where T : class;
    }
}
