using DatosLoteria.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DatosLoteria.Static
{
    static public class StaticLoteria
    {
        //public static LoteriaContext Contexto { get; } = new LoteriaContext();

        /*
        public static string CadenaConexion()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddUserSecrets("55c958c9-1eb6-418b-ba28-ddd6fce8204e")
            .Build();

            return config["Cadena"];
        }
        */
        public static string CadenaConexion()
        {
            ResourceManager recursos = new ResourceManager(typeof(Datos));
            string salida = recursos.GetString("CadenaConexion") ?? "Error.";

            return salida;
        }
    }
}
