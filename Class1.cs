using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace Translator
{
    class YandexTranslator
    {
        public string Translate(string s, string lang, string key)
        {
            if (s.Length > 0)
            {
                WebRequest request = WebRequest.Create("https://translate.yandex.net/api/v1.5/tr.json/translate?"
                    + "key=" + key
                    + "&text=" + s
                    + "&lang=" + lang);

                WebResponse response = request.GetResponse();

                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    string line;

                    if ((line = stream.ReadLine()) != null)
                    {
                        Translation translation = JsonConvert.DeserializeObject<Translation>(line);

                        s = "";

                        foreach (string str in translation.text)
                        {
                            s += str;
                        }
                    }
                }

                return s;
            }
            else
                return "";
        }
    }

    class Translation
    {
        public string code { get; set; }
        public string lang { get; set; }
        public string[] text { get; set; }
    }
}