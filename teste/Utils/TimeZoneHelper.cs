using System;

namespace teste.Utils
{
    /// <summary>
    /// Helper para trabalhar com fuso horário de Brasília (America/Sao_Paulo - UTC-3)
    /// </summary>
    public static class TimeZoneHelper
    {
        private static readonly TimeZoneInfo BrasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

        /// <summary>
        /// Obtém a data/hora atual no fuso horário de Brasília
        /// </summary>
        public static DateTime GetBrasiliaNow()
        {
            var utcNow = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, BrasiliaTimeZone);
        }

        /// <summary>
        /// Converte uma data/hora para UTC considerando que está no fuso de Brasília
        /// </summary>
        public static DateTime ConvertToUtc(DateTime brasiliaTime)
        {
            if (brasiliaTime.Kind == DateTimeKind.Utc)
                return brasiliaTime;

            // Se já está em UTC, retorna como está
            if (brasiliaTime.Kind == DateTimeKind.Unspecified)
            {
                // Assume que está no fuso de Brasília e converte para UTC
                return TimeZoneInfo.ConvertTimeToUtc(brasiliaTime, BrasiliaTimeZone);
            }

            // Se já está em Local, converte para UTC primeiro
            var utc = brasiliaTime.ToUniversalTime();
            return utc;
        }

        /// <summary>
        /// Converte uma data/hora UTC para o fuso horário de Brasília
        /// </summary>
        public static DateTime ConvertFromUtc(DateTime utcTime)
        {
            if (utcTime.Kind != DateTimeKind.Utc)
            {
                utcTime = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
            }
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, BrasiliaTimeZone);
        }
    }
}
