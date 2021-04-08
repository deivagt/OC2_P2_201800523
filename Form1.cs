using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OC2_P2_201800523.AST;

namespace OC2_P2_201800523
{
    public partial class Form1 : Form
    {
        AnalizadorSintactico n;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(richTextBox1.Text);
            this.consola.Text = "";
            n = new AnalizadorSintactico();
            n.analisis(richTextBox1.Text);
            consola.Text = cosasGlobalesewe.salida;
            
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //manejadorArbol.imprimirTabla();
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrirArchivo = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Abrir archivo pascal",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "pas",
                Filter = "Archivo pas (*.pas)|*.pas",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (abrirArchivo.ShowDialog() == DialogResult.OK)
            {
                string text = System.IO.File.ReadAllText(abrirArchivo.FileName);
                //Program.form.richTextBox1.Text = text;
                //Program.form.richTextBox2.Text = "";
                //Program.form.richTextBox3.Text = "";
                //Program.form.richTextBox4.Text = "";
                //Program.form.richTextBox5.Text = "";
            }
        }
    }
}
