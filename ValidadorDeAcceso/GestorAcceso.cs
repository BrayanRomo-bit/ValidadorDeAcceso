using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidadorDeAcceso
{
    internal class GestorAcceso
    {
        private List<Usuario> usuarios;
        private int intentosFallidos = 0;
        private const int MAX_INTENTOS = 3;

        // Constructor para inicializar la lista de usuarios
        public GestorAcceso()
        {
            usuarios = new List<Usuario>();
            // Agregar algunos usuarios de ejemplo

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
            if (intentosFallidos > MAX_INTENTOS)
            {
                StringBuilder csb = new StringBuilder();
                csb.AppendLine("Demasiados intentos fallidos. Acceso bloqueado.");
                return false;
            }
            foreach (var usuario in usuarios)
            {
                if (usuario.Nombre == nombre && usuario.Contraseña == contraseña)
                {
                    StringBuilder sbb = new StringBuilder();
                    sbb.AppendLine("Acceso concedido. Bienvenido, " + nombre + "!");
                    intentosFallidos = 0; // Reiniciar el contador de intentos fallidos
                    return true;
                }
            }
            intentosFallidos++;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Acceso denegado. Intento fallido #" + intentosFallidos);
            return false;

        }
    }
}
