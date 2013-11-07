using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using PanoramaApp1.Utilities;

namespace PanoramaApp1.Utilities
{
    public static class IsolatedStorageOps
    {
        public static async Task Append(this BMIHistoryModel obj, string file)
        {
            var list = await Load<List<BMIHistoryModel>>(file);

            if (list.Count > 19)
            {
                BMIHistoryModel oldest = list.First();

                foreach (var item in list)
                {
                    if (item.Date <= oldest.Date)
                        oldest = item;
                }
                list.Remove(oldest);
            }
            list.Add(obj);

            await list.Save(file);
        }

        public static async Task Save<T>(this T obj, string file)
        {
            await Task.Run(() =>
            {
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream stream = null;

                try
                {
                    stream = storage.CreateFile(file);
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(stream, obj);
                }
                catch (Exception) { }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            });
        }

        public static async Task<T> Load<T>(string file)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            T obj = Activator.CreateInstance<T>();

            if (storage.FileExists(file))
            {
                IsolatedStorageFileStream stream = null;
                try
                {
                    stream = storage.OpenFile(file, FileMode.Open);
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    obj = (T)serializer.Deserialize(stream);
                }
                catch (Exception) { }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
                return obj;
            }
            await obj.Save(file);
            return obj;
        }
    }
}
