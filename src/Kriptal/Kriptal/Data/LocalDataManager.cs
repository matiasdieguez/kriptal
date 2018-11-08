using System;
using System.Collections.Generic;
using System.Linq;
using Realms;
using Xamarin.Forms;
using Kriptal.Models;

namespace Kriptal.Data
{
    public class LocalDataManager
    {
        public byte[] Key { get; set; }

        private readonly RealmConfiguration Config;

        public LocalDataManager(byte[] key)
        {
            Key = key;
            if (Device.RuntimePlatform == Device.Windows || Device.RuntimePlatform == Device.WinPhone)
                Config = new RealmConfiguration { ShouldDeleteIfMigrationNeeded = true };
            else
                Config = new RealmConfiguration { ShouldDeleteIfMigrationNeeded = true, EncryptionKey = key };
        }

        public Realm DbInstance => Realm.GetInstance(Config);

        public bool ExistsDb()
        {
            var dbId = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.DatabaseId.ToString());
            return dbId != null;
        }

        public static byte[] GetSaltBytes()
        {
            var db = Realm.GetInstance("kriptal1");
            var salt = db.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.SaltBytes.ToString());
            return Convert.FromBase64String(salt.Value);
        }

        public static bool ExistsPassword()
        {
            var db = Realm.GetInstance("kriptal1");
            var salt = db.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.SaltBytes.ToString());
            return salt != null;
        }

        public string GetUID()
        {
            var uId = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.DatabaseId.ToString()).Value;
            return uId;
        }

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

        public void DeleteDb()
        {
            Realm.DeleteRealm(Config);
        }

        public void Save<T>(T data) where T : RealmObject
        {
            var db = Realm.GetInstance(Config);

            using (var transaction = db.BeginWrite())
            {
                db.Add(data, true);
                transaction.Commit();
            }
        }

        public void Delete<T>(T data) where T : RealmObject
        {
            var db = Realm.GetInstance(Config);

            using (var transaction = db.BeginWrite())
            {
                db.Remove(data);
                transaction.Commit();
            }
        }

        public IQueryable<T> Query<T>() where T : RealmObject
        {
            return DbInstance.All<T>();
        }

        public List<T> List<T>() where T : RealmObject
        {
            return DbInstance.All<T>().ToList();
        }

        public T Get<T>(Func<T, bool> predicate) where T : RealmObject
        {
            return DbInstance.All<T>().SingleOrDefault(predicate);
        }

        public T FirstOrDefault<T>() where T : RealmObject
        {
            return DbInstance.All<T>().FirstOrDefault();
        }

        public List<T> Where<T>(Func<T, bool> predicate) where T : RealmObject
        {
            return DbInstance.All<T>().Where(predicate).ToList();
        }

        public void SavePublicKey(string publicKey)
        {
            Save(new DbControlModel { Key = DbControlKeys.MyPublicKey.ToString(), Value = publicKey });
        }

        public void SaveMyId(string id)
        {
            Save(new DbControlModel { Key = DbControlKeys.MyId.ToString(), Value = id });
        }

        public string GetName()
        {
            var key = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.MyName.ToString());
            return key.Value;
        }

        public string GetMyId()
        {
            var key = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.MyId.ToString());
            return key.Value;
        }

        public string GetPublicKey()
        {
            var key = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.MyPublicKey.ToString());
            return key.Value;
        }

        public void SavePrivateKey(string key)
        {
            Save(new DbControlModel { Key = DbControlKeys.MyPrivateKey.ToString(), Value = key });
        }

        public void SaveName(string name)
        {
            Save(new DbControlModel { Key = DbControlKeys.MyName.ToString(), Value = name });
        }

        public void SaveEmail(string email)
        {
            Save(new DbControlModel { Key = DbControlKeys.Email.ToString(), Value = email });
        }

        public string GetEmail()
        {
            var key = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.Email.ToString());
            return key.Value;
        }

        public string GetPrivateKey()
        {
            var key = DbInstance.All<DbControlModel>().SingleOrDefault(x => x.Key == DbControlKeys.MyPrivateKey.ToString());
            return key.Value;
        }

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