using System;
using System.Collections.Generic;
using System.Text;

namespace ValidadorDeAcceso
{
    internal class GestorAcceso
    {
        private List<Usuario> usuarios;
        private int intentosFallidos = 0;

        // Hacemos pública la constante para poder leerla desde fuera si es necesario
        public const int MAX_INTENTOS = 3;

        // Constructor
        public GestorAcceso()
        {
            usuarios = new List<Usuario>();
        }

        public GestorAcceso(List<Usuario> usuarios)
        {
            this.usuarios = usuarios;
        }

        public void AgregarUsuario(Usuario usuario)
        {
            usuarios.Add(usuario);
        }

        public bool ValidarAcceso(string nombre, string contraseña)
        {
            // Si ya está bloqueado lógicamente, no validar (capa de seguridad extra)
            if (intentosFallidos >= MAX_INTENTOS)
            {
                return false;
            }

            foreach (var usuario in usuarios)
            {
                if (usuario.Nombre == nombre && usuario.Contraseña == contraseña)
                {
                    intentosFallidos = 0; // Éxito: Reiniciar contador
                    return true;
                }
            }

            // Fallo: Aumentar contador
            intentosFallidos++;
            return false;
        }

        // Método simple para reiniciar el contador desde el Form
        public void ReiniciarIntentos()
        {
            intentosFallidos = 0;
        }

        // Propiedades para leer el estado
        public int IntentosFallidos
        {
            get { return intentosFallidos; }
        }

        public int MaxIntentos
        {
            get { return MAX_INTENTOS; }
        }

        public bool EstaBloqueado()
        {
            return intentosFallidos >= MAX_INTENTOS;
        }

        
       
                 public void MostrarUsuarios()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Usuarios registrados:");
            foreach (var usuario in usuarios)
            {
                sb.AppendLine("- " + usuario.Nombre);
            }
        }
    }
}