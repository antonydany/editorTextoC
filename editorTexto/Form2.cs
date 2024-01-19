using System;
using System.Linq;
using System.Windows.Forms;

namespace editorTexto
{
    public partial class Busqueda : Form
    {
        private readonly RichTextBox richTextBox1;
        public Busqueda(RichTextBox richTextBoxForm1)
        {
            InitializeComponent();
            this.richTextBox1 = richTextBoxForm1;
        }

        private void BusquedaArriba_Click(object sender, EventArgs e)
        {
            string textoBusqueda = this.textoBusqueda.Text;

            // Obtener la posición actual del cursor en el RichTextBox
            int currentPosition = richTextBox1.SelectionStart;

            // Invertir el texto hasta la posición actual para simular la búsqueda hacia arriba
            string textoInvertido = new string(richTextBox1.Text.Reverse().ToArray());
            string textoInvertidoBusqueda = new string(textoBusqueda.Reverse().ToArray());

            // Buscar y seleccionar el texto invertido en el RichTextBox a partir de la posición actual hacia arriba
            int indexInvertido = textoInvertido.IndexOf(textoInvertidoBusqueda, richTextBox1.Text.Length - currentPosition);
            if (indexInvertido != -1)
            {
                // Calcular la posición original en el texto no invertido
                int index = richTextBox1.Text.Length - indexInvertido - textoBusqueda.Length;

                richTextBox1.Select(index, textoBusqueda.Length);
                richTextBox1.ScrollToCaret();  // Opcional: Desplaza automáticamente hasta el texto seleccionado

            }

        }


        private void BusquedaAbajo_Click(object sender, EventArgs e)
        {
            string textoBusqueda = this.textoBusqueda.Text;

            // Obtener la posición actual del cursor en el RichTextBox
            int currentPosition = richTextBox1.SelectionStart + richTextBox1.SelectionLength;

            // Buscar y seleccionar el texto en el RichTextBox a partir de la posición actual hacia abajo
            int index = richTextBox1.Text.IndexOf(textoBusqueda, currentPosition);
            if (index != -1)
            {
                richTextBox1.Select(index, textoBusqueda.Length);
                richTextBox1.ScrollToCaret();  // Opcional: Desplaza automáticamente hasta el texto seleccionado
            }

        }

    }
}
