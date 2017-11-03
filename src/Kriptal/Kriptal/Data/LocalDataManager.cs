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
        /// Key
        /// </summary>
        public byte[] Key { get; set; }

        /// <summary>
        /// Config
        /// </summary>
        private readonly RealmConfiguration Config;

        public LocalDataManager(byte[] key)
        {
            Key = key;
            Config = new RealmConfiguration { ShouldDeleteIfMigrationNeeded = true, EncryptionKey = key };
        }

        /// <summary>
        /// DbInstance
        /// </summary>
        public Realm DbInstance => Realm.GetInstance(Config);

        /// <summary>
        /// ExistsDb
        /// </summary>
        /// <returns>bool</returns>
        public bool ExistsDb()
        {
            var dbId = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.DatabaseId.ToString());
            return dbId != null;
        }

        /// <summary>
        /// ExistsDb
        /// </summary>
        /// <returns>bool</returns>
        public static byte[] GetSaltBytes()
        {
            var db = Realm.GetInstance("kriptal1");
            var salt = db.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.SaltBytes.ToString());
            return Convert.FromBase64String(salt.Value);
        }

        /// <summary>
        /// ExistsPassword
        /// </summary>
        /// <returns>bool</returns>
        public static bool ExistsPassword()
        {
            var db = Realm.GetInstance("kriptal1");
            var salt = db.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.SaltBytes.ToString());
            return salt != null;
        }

        /// <summary>
        /// GetUID
        /// </summary>
        /// <returns></returns>
        public string GetUID()
        {
            var uId = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.DatabaseId.ToString()).Value;
            return uId;
        }

        /// <summary>
        /// CreateDb
        /// </summary>
        public void CreateDb()
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
        public void DeleteDb()
        {
            Realm.DeleteRealm(Config);
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void Save<T>(T data) where T : RealmObject
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
        public void Delete<T>(T data) where T : RealmObject
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
        public IQueryable<T> Query<T>() where T : RealmObject
        {
            return DbInstance.All<T>();
        }

        /// <summary>
        /// List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> List<T>() where T : RealmObject
        {
            return DbInstance.All<T>().ToList();
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Get<T>(Func<T, bool> predicate) where T : RealmObject
        {
            return DbInstance.All<T>().SingleOrDefault(predicate);
        }

        /// <summary>
        /// FirstOrDefault
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FirstOrDefault<T>() where T : RealmObject
        {
            return DbInstance.All<T>().FirstOrDefault();
        }

        /// <summary>
        /// Where
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<T> Where<T>(Func<T, bool> predicate) where T : RealmObject
        {
            return DbInstance.All<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// SaveMyPublicKey
        /// </summary>
        /// <param name="publicKey">publicKey</param>
        public void SavePublicKey(string publicKey)
        {
            Save(new DbControlModel { Key = DbControlKeys.MyPublicKey.ToString(), Value = publicKey });
        }

        /// <summary>
        /// GetName
        /// </summary>
        public string GetName()
        {
            var key = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.MyName.ToString());
            return key.Value;
        }


        /// <summary>
        /// GetMyPublicKey
        /// </summary>
        public string GetPublicKey()
        {
            var key = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.MyPublicKey.ToString());
            return key.Value;
        }

        /// <summary>
        /// SaveMyPrivateKey
        /// </summary>
        /// <param name="key">key</param>
        public void SavePrivateKey(string key)
        {
            Save(new DbControlModel { Key = DbControlKeys.MyPrivateKey.ToString(), Value = key });
        }

        /// <summary>
        /// SaveName
        /// </summary>
        /// <param name="key">key</param>
        public void SaveName(string name)
        {
            Save(new DbControlModel { Key = DbControlKeys.MyName.ToString(), Value = name });
        }

        /// <summary>
        /// GetPrivateKey
        /// </summary>
        public string GetPrivateKey()
        {
            var key = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.MyPrivateKey.ToString());
            return key.Value;
        }

        /// <summary>
        /// SaveSaltBytes
        /// </summary>
        /// <param name="salt">salt</param>
        public static void SaveSaltBytes(byte[] salt)
        {
            var db = Realm.GetInstance("kriptal1");

            using (var transaction = db.BeginWrite())
            {
                db.Add(new DbControlModel
                {
                    Key = DbControlKeys.SaltBytes.ToString(),
                    Value = Convert.ToBase64String(salt)
                }, true);
                transaction.Commit();
            }
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
