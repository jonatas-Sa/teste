using System.ComponentModel.DataAnnotations;
using teste.Utils;

namespace teste.Models
{
    /// <summary>
    /// Classe para armazenar coordenadas geográficas
    /// </summary>
    public class Localizacao
    {
        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude deve estar entre -90 e 90")]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude deve estar entre -180 e 180")]
        public double Longitude { get; set; }

        public DateTime DataAtualizacao { get; set; } = TimeZoneHelper.GetBrasiliaNow();

        /// <summary>
        /// Calcula a distância em metros entre duas localizações usando a fórmula de Haversine
        /// </summary>
        public double CalcularDistancia(Localizacao outraLocalizacao)
        {
            const double raioTerrraKm = 6371;
            
            var dLat = GrausParaRadianos(outraLocalizacao.Latitude - Latitude);
            var dLon = GrausParaRadianos(outraLocalizacao.Longitude - Longitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(GrausParaRadianos(Latitude)) * 
                    Math.Cos(GrausParaRadianos(outraLocalizacao.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distanciaKm = raioTerrraKm * c;

            return distanciaKm * 1000; // Retorna em metros
        }

        private double GrausParaRadianos(double graus)
        {
            return graus * Math.PI / 180;
        }
    }
}
