using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace editorTexto
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog;
        private string fileUbication = "";

        public Form1()
        {
            InitializeComponent();
        }


        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            int posicion = richTextBox1.SelectionStart;
            int linea = richTextBox1.GetLineFromCharIndex(posicion);
            int columna = posicion - richTextBox1.GetFirstCharIndexFromLine(linea);
            label1.Text = "Ln: " + (linea + 1) + ", Col: " + (columna + 1);
            label1.Invalidate();
        }




        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            // Configurar el OpenFileDialog
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt |Archivos Json (*.json)|*.json";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            // Mostrar el cuadro de diálogo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtener la ruta del archivo seleccionado
                string filePath = openFileDialog.FileName;
                fileUbication = filePath;
                //Trasladando texto a richtextbox
                try
                {
                    // Lee el contenido del archivo y lo carga en el RichTextBox
                    string contenido = File.ReadAllText(filePath);
                    richTextBox1.Text = contenido;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void guardarToolStripMenuItem_Save(object sender, EventArgs e)
        {
            File.WriteAllText(fileUbication, richTextBox1.Text);
        }

        private void guardarComoToolStripMenuItem_Save(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.RestoreDirectory = false;
            saveFileDialog.Filter = "Todos los Archivos |*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = "Nuevo";
            if (fileUbication != "")
            {
                saveFileDialog.InitialDirectory = fileUbication;
                saveFileDialog.FileName = fileUbication;
            }
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(fileUbication, richTextBox1.Text);
            }
        }
    }
}
