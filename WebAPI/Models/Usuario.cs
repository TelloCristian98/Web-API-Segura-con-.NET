﻿namespace WebAPI.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string usuario { get; set;}
        public string password { get; set; }
        public string rol { get; set; }

        public static List<Usuario> DB()
        {
            var list = new List<Usuario>()
            {
                new Usuario()
                {
                    idUsuario = 1,
                    usuario = "Enrique",
                    password = "123..",
                    rol = "empleado"
                },
                new Usuario()
                {
                    idUsuario = 2,
                    usuario = "Jaime",
                    password = "123..",
                    rol = "empleado"
                },
                new Usuario()
                {
                    idUsuario = 3,
                    usuario = "Juan",
                    password = "123..",
                    rol = "asesor"
                },
                new Usuario()
                {
                    idUsuario = 4,
                    usuario = "Pedro",
                    password = "123..",
                    rol = "administrador"
                }
            };
            return list;
        }
    }
}
