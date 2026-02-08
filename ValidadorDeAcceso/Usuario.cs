using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidadorDeAcceso
{
    internal class Usuario
    {
        private string nombre="";  
        private string contraseña="";

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }   
        public string Contraseña
        {
            get { return contraseña; }
            set { contraseña = value; }
        }   


        // Constructor
        public Usuario(string nombre, string contraseña)
        {
            this.nombre = nombre;
            this.contraseña = contraseña;
        }
    }
}
