using System;
using System.Collections.Generic;
using System.Drawing; // Necesario para cambiar colores (Color.Red, etc.)
using System.Threading.Tasks; // Necesario para Task.Delay
using System.Windows.Forms;

namespace ValidadorDeAcceso
{
    public partial class Form1 : Form
    {
        private GestorAcceso gestor;
        // Referencia para el botón "Mostrar usuarios" que creaste dinámicamente o en designer
        private Button btnMostrarUsuarios;

        // Lista local para la UI
        private readonly List<string> listaNombres = new List<string>();

        public Form1()
        {
            InitializeComponent();

            // Configuración inicial de tus controles
            // Nota: Si 'button1' ya existe en el Designer, esta línea lo asigna.
            this.btnMostrarUsuarios = this.button1;

            gestor = new GestorAcceso();

            // Estado inicial de la UI
            lstUsuarios.Visible = false;
            if (btnMostrarUsuarios != null) btnMostrarUsuarios.Text = "Mostrar usuarios";

            ActualizarIntentos();
            lblEstado.Text = "Listo";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        // --- BOTÓN AGREGAR ---
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var nombre = txtNombre.Text.Trim();
            var contraseña = txtContraseña.Text;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contraseña))
            {
                lblEstado.Text = "Nombre y contraseña obligatorios.";
                lblEstado.ForeColor = Color.Red;
                return;
            }

            gestor.AgregarUsuario(new Usuario(nombre, contraseña));
            listaNombres.Add(nombre);

            lblEstado.Text = $"Usuario \"{nombre}\" agregado.";
            lblEstado.ForeColor = Color.Green;

            txtNombre.Clear();
            txtContraseña.Clear();
            txtNombre.Focus();

            ActualizarListaUsuarios();
        }

        // --- BOTÓN VALIDAR (Aquí está la lógica del bloqueo) ---
        private async void btnValidar_Click(object sender, EventArgs e)
        {
            var nombre = txtNombre.Text.Trim();
            var contraseña = txtContraseña.Text;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contraseña))
            {
                lblEstado.Text = "Ingrese sus datos.";
                return;
            }

            // 1. Validar credenciales
            var valido = gestor.ValidarAcceso(nombre, contraseña);

            if (valido)
            {
                lblEstado.Text = $"¡Bienvenido, {nombre}!";
                lblEstado.ForeColor = Color.Green;
                // Aquí podrías abrir otra ventana o realizar acciones de éxito
            }
            else
            {
                lblEstado.Text = "Credenciales incorrectas.";
                lblEstado.ForeColor = Color.Red;
            }

            ActualizarIntentos();

            // 2. Comprobar si debemos bloquear
            if (gestor.EstaBloqueado())
            {
                // INICIO DEL BLOQUEO
                lblEstado.Text = "BLOQUEADO: Espere 5 segundos...";
                lblEstado.ForeColor = Color.DarkRed;

                DeshabilitarAcciones(); // Congelar botones

                // Esperar 5 segundos SIN congelar la ventana}
                await Task.Delay(5000);

                // FIN DEL BLOQUEO
                gestor.ReiniciarIntentos(); // Resetear contador interno
                HabilitarAcciones();       // Reactivar botones

                ActualizarIntentos();
                lblEstado.Text = "Sistema desbloqueado. Intente de nuevo.";
                lblEstado.ForeColor = Color.Black;

                txtContraseña.Clear();
                txtContraseña.Focus();
            }
        }

        // Métodos auxiliares para la interfaz
        private void DeshabilitarAcciones()
        {
            btnValidar.Enabled = false;
            btnAgregar.Enabled = false;
            txtNombre.Enabled = false;
            txtContraseña.Enabled = false;
            if (button1 != null) button1.Enabled = false;
            lstUsuarios.Enabled = false;
        }

        private void HabilitarAcciones()
        {
            btnValidar.Enabled = true;
            btnAgregar.Enabled = true;
            txtNombre.Enabled = true;
            txtContraseña.Enabled = true;
            if (button1 != null) button1.Enabled = true;
            lstUsuarios.Enabled = true;
        }

        private void ActualizarIntentos()
        {
            lblIntentos.Text = $"Intentos fallidos: {gestor.IntentosFallidos}/{gestor.MaxIntentos}";


        }

        // --- Resto de funciones de la UI ---

        private void button1_Click(object sender, EventArgs e)
        {
            lstUsuarios.Visible = !lstUsuarios.Visible;
            button1.Text = lstUsuarios.Visible ? "Ocultar usuarios" : "Mostrar usuarios";
        }

        private void ActualizarListaUsuarios()
        {
            lstUsuarios.BeginUpdate();
            lstUsuarios.Items.Clear();
            foreach (var nombre in listaNombres)
            {
                lstUsuarios.Items.Add(nombre);
            }
            lstUsuarios.EndUpdate();

            if (lstUsuarios.Items.Count == 0)
            {
                lstUsuarios.Visible = false;
                if (btnMostrarUsuarios != null) btnMostrarUsuarios.Text = "Mostrar usuarios";
            }
        }

        private void lstUsuarios_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lstUsuarios.SelectedItem != null)
            {
                txtNombre.Text = lstUsuarios.SelectedItem.ToString();
                txtContraseña.Clear();
                txtContraseña.Focus();
            }
        }

        // Eventos vacíos que tenías generados (puedes dejarlos o borrarlos)
        private void lblIntentos_Click(object sender, EventArgs e) { }
    }
}