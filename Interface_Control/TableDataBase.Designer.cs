namespace Interface_Control
{
    partial class TableDataBase
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
            this.dataGridViewTableDB = new System.Windows.Forms.DataGridView();
            this.treeView1 = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTableDB)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewTableDB
            // 
            this.dataGridViewTableDB.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewTableDB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTableDB.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataGridViewTableDB.Location = new System.Drawing.Point(220, 0);
            this.dataGridViewTableDB.Name = "dataGridViewTableDB";
            this.dataGridViewTableDB.RowHeadersVisible = false;
            this.dataGridViewTableDB.Size = new System.Drawing.Size(539, 399);
            this.dataGridViewTableDB.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(223, 399);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect_1);
            // 
            // TableDataBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 399);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.dataGridViewTableDB);
            this.Name = "TableDataBase";
            this.Text = "Таблиця бази даних";
            this.Load += new System.EventHandler(this.TableDataBase_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTableDB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewTableDB;
        private System.Windows.Forms.TreeView treeView1;
    }
}