using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Tasques.Utils
{
    public class Utils
    {
        public class Part
        {
            public Part(string clau, string valor)
            {
                this.clau = clau;
                this.valor = valor;
            }

            public string clau { get; set; }
            public string valor { get; set; }
        }
        public static bool IsCompleted(string task)
        {
            if (!string.IsNullOrEmpty(task))
            {
                string[] parts = task.Split(',');
                foreach (var part in parts)
                {
                    if (part.Split(':')[1].IndexOf("0") > -1)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            //["Part11:0","Part21:0","Part31:0"]
            
            return true;
        }
        public static Dictionary<int, Part> JsonToDic(string json)
        {
            Dictionary<int, Part> diccionari = new Dictionary<int, Part>();
            int contador = 0;
            foreach (var j in json.Split(','))
            {
                contador++;
                var key = j.Split(':')[0].Replace("\"", "").Replace("[", "");
                var value = j.Split(':')[1].Replace("\"", "").Replace("]", "");
                diccionari.Add(contador, new Part(key, value));
            }
            return diccionari;

        }
        public static string DicToJson(Dictionary<int, Part> dict)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            foreach (var d in dict.Values)
            {

                json.Append(d.clau+":"+d.valor+",");
            }
            var json2 = json.ToString().Remove(json.Length-1);
            json2+="]";
            return json2;

        }
    }
}