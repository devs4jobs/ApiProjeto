using Model;
using Newtonsoft.Json;
using System;
using System.IO;
namespace Core.util
{
    //essa classe file é estatica pelo fato de eu não querer instaciar ela nunca e manipular os Metodos dela da onde eu quiser.
    public static class file
    {
        //aqui eu declaro que meu arquivo Json será salvo na base do meu Programa.
        public static string arquivoDb = AppDomain.CurrentDomain.BaseDirectory + "db.json";

        //esse tupple serve pra fazeer a gravação ou a leitura do arquivo Json e eu Consigo retornar o sistema caso tenha no Arquivo Json.
        public static (bool gravacao, Sistema sistema) ManipulacaoDeArquivos(bool read, Sistema _sistema)
        {
            try
            {
                if (!File.Exists(arquivoDb)) { File.Create(arquivoDb).Close();}

                if (read)
                    return (false, JsonConvert.DeserializeObject<Sistema>(File.ReadAllText(arquivoDb)));

                File.WriteAllText(arquivoDb, JsonConvert.SerializeObject(_sistema));

                return (true, null);

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
