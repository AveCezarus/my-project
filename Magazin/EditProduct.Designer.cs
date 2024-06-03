namespace Magazin
{
    partial class EditProduct
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            textBox1 = new TextBox();
            comboBox1 = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            textBox2 = new TextBox();
            label4 = new Label();
            textBox3 = new TextBox();
            dateTimePicker1 = new DateTimePicker();
            label5 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(75, 63);
            label1.Name = "label1";
            label1.Size = new Size(118, 20);
            label1.TabIndex = 0;
            label1.Text = "Назва продукта";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(75, 86);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(220, 27);
            textBox1.TabIndex = 1;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Килограмм", "Штука", "Литр", "Упаковка" });
            comboBox1.Location = new Point(75, 181);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(220, 28);
            comboBox1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(75, 158);
            label2.Name = "label2";
            label2.Size = new Size(120, 20);
            label2.TabIndex = 3;
            label2.Text = "Одиниці виміру";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(75, 255);
            label3.Name = "label3";
            label3.Size = new Size(92, 20);
            label3.TabIndex = 4;
            label3.Text = "Ціна товару";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(75, 278);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(220, 27);
            textBox2.TabIndex = 5;
            textBox2.KeyPress += textBox2_KeyPress;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(389, 63);
            label4.Name = "label4";
            label4.Size = new Size(58, 20);
            label4.TabIndex = 6;
            label4.Text = "Кол-во";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(389, 86);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(219, 27);
            textBox3.TabIndex = 7;
            textBox3.KeyPress += textBox3_KeyPress;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(389, 182);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(219, 27);
            dateTimePicker1.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(389, 159);
            label5.Name = "label5";
            label5.Size = new Size(146, 20);
            label5.TabIndex = 9;
            label5.Text = "Термін придатності";
            // 
            // button1
            // 
            button1.Location = new Point(89, 373);
            button1.Name = "button1";
            button1.Size = new Size(195, 33);
            button1.TabIndex = 10;
            button1.Text = "Зберегти";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // EditProduct
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(669, 524);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(dateTimePicker1);
            Controls.Add(textBox3);
            Controls.Add(label4);
            Controls.Add(textBox2);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(comboBox1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Name = "EditProduct";
            Text = "EditProduct";
            FormClosed += EditProduct_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private Label label3;
        private TextBox textBox2;
        private Label label4;
        private TextBox textBox3;
        private DateTimePicker dateTimePicker1;
        private Label label5;
        private Button button1;
        internal ComboBox comboBox1;
    }
}