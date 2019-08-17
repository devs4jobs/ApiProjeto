using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Util
{
    public class Json<T>
    {
        readonly string traj = AppDomain.CurrentDomain.BaseDirectory;
        public void Recuperar(List<T> coisas, string nome)
        {
            string path = $"{traj}{nome}.json";
            using (StreamReader s = File.OpenText(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                 var arq = JsonConvert.DeserializeObject<T>(line);
                    coisas.Add(arq);
                }
            }
        }
        public  void Salvar(List<T> coisas, string nome)
        {
            string path = $"{traj}{nome}.json";
            File.Delete(path);
            using (StreamWriter s = File.AppendText(path))
            {
                foreach (var produto in coisas)
                {
                    var arq = JsonConvert.SerializeObject(produto);
                    s.WriteLine(arq);
                }
            }
        }
    }
}
