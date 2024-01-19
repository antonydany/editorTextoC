using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;





namespace editorTexto
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog;
        readonly string windowTitle = "EditordeTexto v0.0.2(alpha) - ";

        string fileUbication = "";
        string fileNombre = "Documento de Texto";
        private bool changesNotSaves = false;
        string selectedText = "";
        //zoomFactor
        readonly float zoomFactorMax = 12f;
        readonly float zoomFactorMin = 0.5f;

        public Form1()
        {
            InitializeComponent();
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!changesNotSaves)
            {
                changesNotSaves = true;
            }

        }

        private void RichTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            int posicion = richTextBox1.SelectionStart + richTextBox1.SelectionLength;
            int linea = richTextBox1.GetLineFromCharIndex(posicion);
            int columna = posicion - richTextBox1.GetFirstCharIndexFromLine(linea);
            this.lineColumnLabel.Text = $"Ln: {linea + 1}, Col: {columna + 1}";
            RichTextBox1_SelectedCount(sender, e);
        }

        private void RichTextBox1_SelectedCount(object sender, EventArgs e)
        {
            int countCharacters = richTextBox1.SelectionLength;
            this.CharactersLabel.Text = $"Carácter: {countCharacters}";
            this.CharactersLabel.Enabled = countCharacters != 0;
        }


        private void AbrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog
            {
                // Configurar el OpenFileDialog
                Filter = "Archivo de Texto (*.txt)| *.txt|Todos los Archivos | *.*",
                FilterIndex = 1,
                RestoreDirectory = true,
            };
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
                    this.Text = windowTitle + this.fileNombre;
                    changesNotSaves = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void GuardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarToolStripMenuItem_Save();
        }
        private void GuardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarComoToolStripMenuItem_Save();
        }


        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changesNotSaves)
            {
                DialogResult result = MessageBox.Show("¿Quieres guardar los cambios antes de salir?", "Guardar cambios", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    e.Cancel = GuardarToolStripMenuItem_Save();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }



        private void CortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedText = richTextBox1.SelectedText;
            if (!string.IsNullOrEmpty(selectedText))
            {
                Clipboard.SetText(selectedText);
                this.richTextBox1.SelectedText = "";
            }
        }

        private void CopiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedText = richTextBox1.SelectedText;
            if (!string.IsNullOrEmpty(selectedText))
            {
                Clipboard.SetText(selectedText);
            }

        }

        private void PegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {

                richTextBox1.Paste(DataFormats.GetFormat(DataFormats.Text));
            }
        }

        private void SeleccionarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void BuscarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Busqueda>().Any())
            {
                Console.WriteLine("Ventana abierta");
                return;
            }
            Busqueda busqueda = new Busqueda(richTextBox1);
            busqueda.Show();
        }

        private void RestaurarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.ZoomFactor = 1;
        }

        private void HandleZoom(float factor)
        {
            this.richTextBox1.ZoomFactor = Math.Max(0.5f, Math.Min(zoomFactorMax, this.richTextBox1.ZoomFactor + factor));
            this.alejarToolStripMenuItem.Enabled = (this.richTextBox1.ZoomFactor > zoomFactorMin);
            this.acercarToolStripMenuItem.Enabled = (this.richTextBox1.ZoomFactor < zoomFactorMax);
        }

        private void AcercarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleZoom(0.1f);
        }

        private void AlejarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleZoom(-0.1f);
        }

        private void RichTextBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                float factor = e.Delta > 0 ? 0.1f : -0.1f;
                HandleZoom(factor);
            }
        }


        //funciones booleanas

        private bool SaveFile(string filePath)
        {
            try
            {
                File.WriteAllText(filePath, richTextBox1.Text);
                fileUbication = filePath;
                fileNombre = Path.GetFileName(filePath);
                this.Text = windowTitle + fileNombre;
                changesNotSaves = false;
                return false;
            }
            catch
            {
                return true;
            }


        }

        private bool GuardarToolStripMenuItem_Save()
        {
            if (string.IsNullOrEmpty(fileUbication))
            {
                return GuardarComoToolStripMenuItem_Save();
            }
            else
            {
                return SaveFile(fileUbication);
            }

        }

        private bool GuardarComoToolStripMenuItem_Save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                Filter = "Archivo de Texto (*.txt)| *.txt|Todos los Archivos | *.*",
                FilterIndex = 1,
                FileName = fileNombre,
            };
            DialogResult dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                Console.WriteLine("Se guardó como: " + fileNombre);
                return SaveFile(saveFileDialog.FileName);
            }
            else if (dialogResult == DialogResult.No)
            {
                Console.WriteLine("Se presiono NO");
                return false;
            }
            else
            {
                Console.WriteLine("Se presiono otra cosa");
                return true;
            }

        }


    }
}
