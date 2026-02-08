using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValidadorDeAcceso
{
    public partial class Form1 : Form
    {
        private GestorAcceso gestor;
        private Button btnMostrarUsuarios;

        public Form1()
        {
            InitializeComponent();
            gestor = new GestorAcceso();
            // Usuario de ejemplo
            gestor.AgregarUsuario(new Usuario("admin", "1234"));
            //ActualizarListaUsuarios();
            ActualizarIntentos();
            lblEstado.Text = "Listo";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var nombre = txtNombre.Text.Trim();
            var contraseña = txtContraseña.Text;
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contraseña))
            {
                lblEstado.Text = "Nombre y contraseña obligatorios para agregar.";
                return;
            }

            gestor.AgregarUsuario(new Usuario(nombre, contraseña));
            lblEstado.Text = $"Usuario \"{nombre}\" agregado.";
            txtNombre.Clear();
            txtContraseña.Clear();
            //ActualizarListaUsuarios();
        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            var nombre = txtNombre.Text.Trim();
            var contraseña = txtContraseña.Text;
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contraseña))
            {
                lblEstado.Text = "Ingrese nombre y contraseña para validar.";
                return;
            }
            if (gestor.EstaBloqueado())
            {
                lblEstado.Text = "Acceso bloqueado por demasiados intentos.";
                DeshabilitarAcciones();
                return;
            }

            var valido = gestor.ValidarAcceso(nombre, contraseña);
            if (valido)
            {
                lblEstado.Text = $"Acceso concedido. Bienvenido, {nombre}!";
            }
            else
            {
                lblEstado.Text = $"Acceso denegado. Intento #{gestor.IntentosFallidos}";
            }

            if (gestor.EstaBloqueado())
            {
                lblEstado.Text = "Acceso bloqueado por demasiados intentos.";
                DeshabilitarAcciones();
            }

            ActualizarIntentos();
        }

        
        

        private void lstUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Al seleccionar un usuario, rellenar el campo nombre y situar foco en contraseña
            if (lstUsuarios.SelectedItem == null)
            {
                return;
            }

            txtNombre.Text = lstUsuarios.SelectedItem.ToString();
            txtContraseña.Clear();
            txtContraseña.Focus();
            lblEstado.Text = $"Usuario \"{txtNombre.Text}\" seleccionado. Introduce la contraseña para validar.";
        }

        private void lstUsuarios_DoubleClick(object sender, EventArgs e)
        {
            // Doble clic: intenta validar usando el usuario seleccionado y la contraseña actual
            if (lstUsuarios.SelectedItem == null)
            {
                return;
            }

            txtNombre.Text = lstUsuarios.SelectedItem.ToString();
            btnValidar_Click(this, EventArgs.Empty);
        }

        private void DeshabilitarAcciones()
        {
            btnValidar.Enabled = false;
            btnAgregar.Enabled = false;
            txtNombre.Enabled = false;
            txtContraseña.Enabled = false;
            btnMostrarUsuarios.Enabled = false;
            lstUsuarios.Enabled = false;
        }

        private void ActualizarIntentos()
        {
           lblIntentos.Text = $"Intentos fallidos: {gestor.IntentosFallidos} {gestor.MaxIntentos}";
        }

        private void lblIntentos_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
       
           // Alterna la visibilidad del listado de usuarios
            lstUsuarios.Visible = !lstUsuarios.Visible;
           btnMostrarUsuarios.Text = lstUsuarios.Visible ? "Ocultar usuarios" : "Mostrar usuarios";

            if (lstUsuarios.Visible)
            {
                lstUsuarios.Focus();
            }
        }

         
    }
}

