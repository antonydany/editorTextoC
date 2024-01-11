using System;
using System.IO;
using System.Windows.Forms;




namespace editorTexto
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog;
        string fileUbication = "";
        string fileNombre = "Documento de Texto";
        private bool changesNotSaves = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            changesNotSaves = true;
        }

        private void RichTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            int posicion = richTextBox1.SelectionStart;
            int linea = richTextBox1.GetLineFromCharIndex(posicion);
            int columna = posicion - richTextBox1.GetFirstCharIndexFromLine(linea);
            label1.Text = "Ln: " + (linea + 1) + ", Col: " + (columna + 1);
            label1.Invalidate();
        }
        private void RichTextBox1_SelectedCount(object sender, EventArgs e)
        {
            int countCharacters = richTextBox1.SelectionLength;
            this.label2.Text = "Caracteres: " + countCharacters;
            label2.Invalidate();
        }




        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void Label1_Click(object sender, EventArgs e)
        {

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
                    this.Text = "EditordeTexto v0.0.1 - " + this.fileNombre;
                    changesNotSaves = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void GuardarToolStripMenuItem_Save(object sender, EventArgs e)
        {
            if (fileUbication != "")
            {
                File.WriteAllText(fileUbication, richTextBox1.Text);
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    RestoreDirectory = true,
                    Filter = "Archivo de Texto (*.txt)| *.txt|Todos los Archivos | *.*",
                    FilterIndex = 1,
                    FileName = "Documento de Texto"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, richTextBox1.Text);
                    fileUbication = saveFileDialog.FileName;
                    fileNombre = Path.GetFileName(saveFileDialog.FileName);
                    // Actualiza el titulo
                    this.Text = "EditordeTexto v0.0.1 - " + this.fileNombre;
                    changesNotSaves = false;
                }
                Console.WriteLine("Se guardó: " + fileNombre);
            }
        }

        private void GuardarComoToolStripMenuItem_Save(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                Filter = "Archivo de Texto (*.txt)| *.txt|Todos los Archivos | *.*",
                FilterIndex = 1,
                FileName = "Documento de Texto",
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                saveFileDialog.InitialDirectory = fileUbication;
                saveFileDialog.FileName = fileUbication;
                File.WriteAllText(fileUbication, richTextBox1.Text);
                fileNombre = Path.GetFileName(saveFileDialog.FileName);
                Console.WriteLine("Se guardó como: " + fileNombre);
                // Actualiza el titulo
                this.Text = "EditordeTexto v0.0.1 - " + this.fileNombre;
                changesNotSaves = false;
            }
        }

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (changesNotSaves)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    RestoreDirectory = true,
                    Filter = "Archivo de Texto (*.txt)| *.txt|Todos los Archivos | *.*",
                    FilterIndex = 1,
                    FileName = "Documento de Texto",
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    saveFileDialog.InitialDirectory = fileUbication;
                    saveFileDialog.FileName = fileUbication;
                    File.WriteAllText(fileUbication, richTextBox1.Text);
                    fileNombre = Path.GetFileName(saveFileDialog.FileName);
                    Console.WriteLine("Se guardó como: " + fileNombre);
                    // Actualiza el titulo
                    this.Text = "EditordeTexto v0.0.1 - " + this.fileNombre;
                    changesNotSaves = false;
                }
            }
            else
            {
                this.Close();

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changesNotSaves)
            {
                DialogResult result = MessageBox.Show("¿Quieres guardar los cambios antes de salir?", "Guardar cambios", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        RestoreDirectory = true,
                        Filter = "Archivo de Texto (*.txt)| *.txt|Todos los Archivos | *.*",
                        FilterIndex = 1,
                        FileName = "Documento de Texto",
                    };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        saveFileDialog.InitialDirectory = fileUbication;
                        saveFileDialog.FileName = fileUbication;
                        File.WriteAllText(fileUbication, richTextBox1.Text);
                        fileNombre = Path.GetFileName(saveFileDialog.FileName);
                        Console.WriteLine("Se guardó como: " + fileNombre);
                        // Actualiza el titulo
                        this.Text = "EditordeTexto v0.0.1 - " + this.fileNombre;
                        changesNotSaves = false;
                    }
                    else
                    {
                        // Cancelar el cierre si el usuario elige cancelar el guardado
                        e.Cancel = true;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    // Cancelar el cierre si el usuario elige cancelar el guardado
                    e.Cancel = true;
                }
            }
        }


    }
}
