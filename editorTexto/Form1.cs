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
        string fileUbication = "";
        string fileNombre = "Documento de Texto";

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
            openFileDialog.Filter = "Todos los Archivos |*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            // Mostrar el cuadro de diálogo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtener la ruta del archivo seleccionado
                string filePath = openFileDialog.FileName;
                fileNombre = openFileDialog.SafeFileName;
                fileUbication = filePath;
                //Trasladando texto a richtextbox
                try
                {
                    // Lee el contenido del archivo y lo carga en el RichTextBox
                    string contenido = File.ReadAllText(filePath);
                    richTextBox1.Text = contenido;
                    // Actualiza el titulo
                    this.Text = "El Danytor - " + this.fileNombre;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void guardarToolStripMenuItem_Save(object sender, EventArgs e)
        {
            if (fileUbication != "")
            {
                File.WriteAllText(fileUbication, richTextBox1.Text);
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Filter = "Archivo de Texto (*.txt)|*.txt |Todos los Archivos |*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.FileName = "Documento de Texto";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, richTextBox1.Text);
                    fileUbication = saveFileDialog.FileName;
                    fileNombre = Path.GetFileName(saveFileDialog.FileName);
                    // Actualiza el titulo
                    this.Text = "El Danytor - " + this.fileNombre;
                }
                Console.WriteLine("Se guardó: " + fileNombre);
            }
        }

        private void guardarComoToolStripMenuItem_Save(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "Archivo de Texto (*.txt)|*.txt| Todos los Archivos |*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = "Documento de Texto";
 
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                saveFileDialog.InitialDirectory = fileUbication;
                saveFileDialog.FileName = fileUbication;
                File.WriteAllText(fileUbication, richTextBox1.Text);
                fileNombre = Path.GetFileName(saveFileDialog.FileName);
                Console.WriteLine("Se guardó como: " + fileNombre);
                // Actualiza el titulo
                this.Text = "El Danytor - " + this.fileNombre;
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
