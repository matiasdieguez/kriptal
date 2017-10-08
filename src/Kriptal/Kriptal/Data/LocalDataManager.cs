using System;
using System.Collections.Generic;
using System.Linq;

using Realms;

using Kriptal.Models;

namespace Kriptal.Data
{
    /// <summary>
    /// LocalDataManager wrapper for Realm local database
    /// </summary>
    public class LocalDataManager
    {
        /// <summary>
        /// Config
        /// </summary>
        private static readonly RealmConfiguration Config = new RealmConfiguration { ShouldDeleteIfMigrationNeeded = false };

        /// <summary>
        /// DbInstance
        /// </summary>
        public static Realm DbInstance => Realm.GetInstance(Config);

        /// <summary>
        /// ExistsDb
        /// </summary>
        /// <returns>bool</returns>
        public static bool ExistsDb()
        {
            var dbId = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.DatabaseId.ToString());
            return dbId != null;
        }

        /// <summary>
        /// GetUID
        /// </summary>
        /// <returns></returns>
        public static string GetUID()
        {
            var uId = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.DatabaseId.ToString()).Value;
            return uId;
        }

        /// <summary>
        /// CreateDb
        /// </summary>
        public static void CreateDb()
        {
            var db = Realm.GetInstance(Config);

            using (var transaction = db.BeginWrite())
            {
                var entry = new DbControlModel
                {
                    Key = DbControlKeys.DatabaseId.ToString(),
                    Value = Guid.NewGuid().ToString()
                };
                db.Add(entry);
                transaction.Commit();
            }
        }

        /// <summary>
        /// DeleteDb
        /// </summary>
        public static void DeleteDb()
        {
            Realm.DeleteRealm(Config);
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static void Save<T>(T data) where T : RealmObject
        {
            var db = Realm.GetInstance(Config);

            using (var transaction = db.BeginWrite())
            {
                db.Add(data, true);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static void Delete<T>(T data) where T : RealmObject
        {
            var db = Realm.GetInstance(Config);

            using (var transaction = db.BeginWrite())
            {
                db.Remove(data);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> Query<T>() where T : RealmObject
        {
            return DbInstance.All<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> List<T>() where T : RealmObject
        {
            return DbInstance.All<T>().ToList();
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T Get<T>(Func<T, bool> predicate) where T : RealmObject
        {
            return DbInstance.All<T>().SingleOrDefault(predicate);
        }

        /// <summary>
        /// FirstOrDefault
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FirstOrDefault<T>() where T : RealmObject
        {
            return DbInstance.All<T>().FirstOrDefault();
        }

        /// <summary>
        /// Where
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<T> Where<T>(Func<T, bool> predicate) where T : RealmObject
        {
            return DbInstance.All<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// SaveMyPublicKey
        /// </summary>
        /// <param name="publicKey">publicKey</param>
        public static void SaveMyPublicKey(string publicKey)
        {
            Save(new DbControlModel { Key = DbControlKeys.MyPublicKey.ToString(), Value = publicKey });
        }

        static DateTime ConvertFromDateTimeOffset(DateTimeOffset dateTime)
        {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            else if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
            else
                return dateTime.DateTime;
        }
    }
}
