using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateContainers
{
    public class TemplateContainer<T> where T : BaseTemplate
    {
        public static IEnumerable<T> Values => datas.Values;

        private static Dictionary<int, T> datas = new Dictionary<int, T>();

        private static Dictionary<string, T> nameKeyDatas = new Dictionary<string, T>();
        public static T Find(int id)
        {
            if (datas.ContainsKey(id) == false)
            {
                return default;
            }

            return datas[id];
        }
        public static void Load(string path, string fileName)
        {
            string fullPath = Path.Combine(path, fileName);

            using (var stream = new StreamReader(File.OpenRead(fullPath)))
            {
                var content = stream.ReadToEnd();

                var templateDatas = JsonConvert.DeserializeObject<List<T>>(content);

                foreach (var template in templateDatas)
                {
                    datas.Add(template.Id, template);

                    nameKeyDatas.Add(template.Name, template);
                }
            }
        }
    }
}
