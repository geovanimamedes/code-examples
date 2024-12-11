using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DapperExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        public class Template
        {
            public Campo[] DadosEntrada { get; set; }
            public Campo[] Parametros { get; set; }
            public Campo[] DadosSaida { get; set; }
        }

        public class Campo
        {
            public string Nome { get; set; }
            public TipoCampo Tipo { get; set; }
            public TipoCampo SubTipo { get; set; } // Utilizado somente quando o tipo for array, para identificar o tipo do array
            public string Obrigatorio { get; set; }
            public Campo[] Campos { get; set; } // Utilizado somente quando o tipo object, para definir os campos do object
        }

        public enum TipoCampo
        {
            Number,
            Datetime,
            Boolean,
            String,
            Array,
            Object
        }

        public static string ValidarTemplate(string idTemplate, string jsonDadosEntrada, string jsonParametros, string jsonDadosSaida)
        {
            // Buscar template no banco pelo idTemplate
            var Template = new Template
            {
                DadosEntrada = new Campo[1],
                Parametros = new Campo[1],
                DadosSaida = new Campo[1]
            };

            var camposComErro = new List<string>();

            if (!string.IsNullOrEmpty(jsonDadosEntrada))
                camposComErro.AddRange(ValidarCampos(Template.DadosEntrada, jsonDadosEntrada, "DadosEntrada"));

            if (!string.IsNullOrEmpty(jsonParametros))
                camposComErro.AddRange(ValidarCampos(Template.Parametros, jsonParametros, "Parametros"));

            if (!string.IsNullOrEmpty(jsonDadosSaida))
                camposComErro.AddRange(ValidarCampos(Template.DadosSaida, jsonDadosSaida, null));

            return camposComErro.Any() ? FormatarMensagemErro(camposComErro) : null;
        }

        public static string FormatarMensagemErro(List<string> camposInvalidos)
        {
            var limitador = camposInvalidos.Count > 30 ? ", entre outros" : string.Empty;

            return $"Requisição inválida. Os seguintes campos estão inválidos em relação ao template do cálculo: " +
                $"{string.Join(", ", camposInvalidos.Take(30))}{limitador}.";
        }

        public static List<string> ValidarCampos(Campo[] camposTemplate, string json, string campoPai)
        {
            var camposInvalidos = new List<string>();

            if (!camposTemplate.Any())
                return camposInvalidos;

            if (string.IsNullOrEmpty(json))
            {
                camposInvalidos.Add(campoPai);
                return camposInvalidos;
            }

            var camposJson = (JArray)JObject.Parse(json)[campoPai];

            foreach (var campo in camposTemplate)
            {
                var campoJson = camposJson[campo.Nome];
                camposInvalidos.AddRange(ValidarCampo(campo, campoJson, campoPai));
            }

            return camposInvalidos;
        }

        public static List<string> ValidarCampo(Campo campoTemplate, JToken campoJson, string campoPai)
        {
            switch (campoTemplate.Tipo)
            {
                case TipoCampo.Boolean: return ValidarCampoSimples(campoTemplate, campoJson, campoPai);
                case TipoCampo.String: return ValidarCampoSimples(campoTemplate, campoJson, campoPai);
                case TipoCampo.Number: return ValidarCampoSimples(campoTemplate, campoJson, campoPai);
                case TipoCampo.Datetime: return ValidarCampoSimples(campoTemplate, campoJson, campoPai);
                case TipoCampo.Array: return ValidarCampoArray(campoTemplate, campoJson, campoPai);
                case TipoCampo.Object: return ValidarCampoObjeto(campoTemplate, campoJson, campoPai);
                default: throw new Exception($"O tipo de campo '{campoTemplate.Tipo}' no campo '{campoTemplate.Nome}' é inválido no template do cálculo.");
            }
        }

        public static List<string> ValidarCampoSimples(Campo campoTemplate, JToken campoJson, string campoPai)
        {
            return null;
        }

        public static List<string> ValidarCampoArray(Campo campoTemplate, JToken campoJson, string campoPai)
        {
            return null;
        }

        public static List<string> ValidarCampoObjeto(Campo campoTemplate, JToken campoJson, string campoPai)
        {
            return null;
        }
    }
}
