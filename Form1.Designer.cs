﻿
namespace OC2_P2_201800523
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.consola = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.consolaErrores = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.consolaOPT = new System.Windows.Forms.RichTextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.editable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.richTextBox1.Location = new System.Drawing.Point(12, 45);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(824, 275);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "Entrada";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(12, 337);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 32);
            this.label2.TabIndex = 2;
            this.label2.Text = "Salida C3D";
            this.label2.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(311, 335);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 35);
            this.button1.TabIndex = 3;
            this.button1.Text = "Traducir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // consola
            // 
            this.consola.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.consola.Location = new System.Drawing.Point(12, 392);
            this.consola.Name = "consola";
            this.consola.ReadOnly = true;
            this.consola.Size = new System.Drawing.Size(824, 244);
            this.consola.TabIndex = 1;
            this.consola.Text = "";
            this.consola.TextChanged += new System.EventHandler(this.richTextBox2_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(955, 287);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Generar TS";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(861, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 32);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tabla de Simbolos";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(861, 50);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(261, 222);
            this.richTextBox3.TabIndex = 6;
            this.richTextBox3.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(1131, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(225, 32);
            this.label4.TabIndex = 7;
            this.label4.Text = "Salida Graphviz xd";
            // 
            // richTextBox4
            // 
            this.richTextBox4.Location = new System.Drawing.Point(1131, 50);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.ReadOnly = true;
            this.richTextBox4.Size = new System.Drawing.Size(261, 75);
            this.richTextBox4.TabIndex = 8;
            this.richTextBox4.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(861, 313);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 32);
            this.label5.TabIndex = 9;
            this.label5.Text = "Errores";
            // 
            // consolaErrores
            // 
            this.consolaErrores.Location = new System.Drawing.Point(861, 348);
            this.consolaErrores.Name = "consolaErrores";
            this.consolaErrores.ReadOnly = true;
            this.consolaErrores.Size = new System.Drawing.Size(261, 288);
            this.consolaErrores.TabIndex = 10;
            this.consolaErrores.Text = "";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button3.Location = new System.Drawing.Point(717, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 29);
            this.button3.TabIndex = 11;
            this.button3.Text = "Abrir archivo";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button4.Location = new System.Drawing.Point(436, 335);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(98, 35);
            this.button4.TabIndex = 12;
            this.button4.Text = "Optimizar";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(1131, 313);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(264, 32);
            this.label6.TabIndex = 13;
            this.label6.Text = "Reporte Optimización";
            // 
            // consolaOPT
            // 
            this.consolaOPT.Location = new System.Drawing.Point(1131, 348);
            this.consolaOPT.Name = "consolaOPT";
            this.consolaOPT.ReadOnly = true;
            this.consolaOPT.Size = new System.Drawing.Size(261, 288);
            this.consolaOPT.TabIndex = 14;
            this.consolaOPT.Text = "";
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button5.Location = new System.Drawing.Point(738, 335);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(98, 35);
            this.button5.TabIndex = 15;
            this.button5.Text = "Editar ";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // editable
            // 
            this.editable.AutoSize = true;
            this.editable.Location = new System.Drawing.Point(15, 369);
            this.editable.Name = "editable";
            this.editable.Size = new System.Drawing.Size(71, 15);
            this.editable.TabIndex = 16;
            this.editable.Text = "Editable: No";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1420, 648);
            this.Controls.Add(this.editable);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.consolaOPT);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.consolaErrores);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.richTextBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.consola);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.RichTextBox consola;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.RichTextBox richTextBox4;
        public System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.RichTextBox consolaErrores;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.RichTextBox consolaOPT;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label editable;
    }
}

