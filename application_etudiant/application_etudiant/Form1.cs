using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace application_etudiant

{
    public partial class Form1 : Form

    {
        SqlConnection maconnexion = new SqlConnection(@"Data Source=DESKTOP-MREVLJU\SQLEXPRESS;Initial Catalog=Applications;Integrated Security=True");
        SqlCommand req = new SqlCommand();
        public Form1()
        {
            InitializeComponent();
        }

        void EffacerEcran(Control C)
        {
            foreach (Control item in C.Controls)
            {
                // MessageBox.Show(item.GetType().ToString());
                if (item.GetType() == typeof(TextBox))
                    ((TextBox)item).Clear();
                if (item.GetType() == typeof(DateTimePicker))
                    ((DateTimePicker)item).Value = DateTime.Today;

                if (item.Controls.Count != 0)
                    EffacerEcran(item);
            }

        }
        bool ControlerCodeETd(string xcode)
        {
            SqlCommand cmd = new SqlCommand("select * from AppEtudiant where رقم_مسار = @num", maconnexion);
            cmd.Parameters.AddWithValue("@num", SqlDbType.NVarChar).Value = xcode;
            SqlDataReader DR = cmd.ExecuteReader();
            DR.Read(); //capter une ligne
            bool resultat = DR.HasRows;
            DR.Close();
            return resultat;
        }
        void RemplirDataGrid()
        {
            //Remplissage du datagridview par le contenu de la table DB_Citoyen.Citoyen
            SqlCommand Requete = new SqlCommand("select * from AppEtudiant", maconnexion);
            SqlDataReader SQLDR = Requete.ExecuteReader();
            DataTable DT = new DataTable();
            DT.Load(SQLDR);
            dataGridView1.DataSource = DT;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (maconnexion.State != ConnectionState.Open)
                maconnexion.Open();
            RemplirDataGrid();
            this.dataGridView1.ReadOnly = true;
        }
        //*****************AJOUTER**********************
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                if (ControlerCodeETd(textBox1.Text))
                {
                    MessageBox.Show("Ce Code est déjà utilisé !!");
                }
                else //ControlerCodeETd(int.Parse(textBoxCode.Text)) == False
                {
                    SqlCommand macommande = new SqlCommand();
                    macommande.Connection = maconnexion;
                    macommande.CommandText = "Insert Into AppEtudiant values(@datefrmt,@ecl,@datesrt,@anneescl,@niveau,@datenaiss,@prenom,@nom,@num)";
                    // Cette Requete contient des parametres @paracode,@paranom,@paraprenom,@paraAge
                    // Préparation des parametres
                    macommande.Parameters.AddWithValue("@num", SqlDbType.NVarChar).Value = textBox1.Text;
                    macommande.Parameters.AddWithValue("@nom", SqlDbType.NVarChar).Value = textBox2.Text;
                    macommande.Parameters.AddWithValue("@prenom", SqlDbType.NVarChar).Value = textBox3.Text;
                    macommande.Parameters.AddWithValue("@datenaiss", SqlDbType.NVarChar).Value = textBox4.Text;
                    macommande.Parameters.AddWithValue("@niveau", SqlDbType.NVarChar).Value = textBox5.Text;
                    macommande.Parameters.AddWithValue("@anneescl", SqlDbType.NVarChar).Value = textBox6.Text;
                    macommande.Parameters.AddWithValue("@datesrt", SqlDbType.NVarChar).Value = textBox7.Text;
                    macommande.Parameters.AddWithValue("@ecl", SqlDbType.NVarChar).Value = textBox8.Text;
                    macommande.Parameters.AddWithValue("@datefrmt", SqlDbType.NVarChar).Value = textBox9.Text;
                    // ****** Envoi de la Requete ******
                    int n = macommande.ExecuteNonQuery();
                    if (n == 0)
                        MessageBox.Show("Aucune Insertion !!");
                    else
                    {
                        MessageBox.Show("Insertion bien passée");
                        EffacerEcran(this);
                        RemplirDataGrid();
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir le code de l'Etudiant!!");
            }
        }

        //*****************CHERCHER*********************
        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand Requete = new SqlCommand("select * from AppEtudiant where النسب = @prenom or الإسم=@nom", maconnexion);

            Requete.Parameters.AddWithValue("@prenom", SqlDbType.NVarChar).Value = textBox3.Text;
            Requete.Parameters.AddWithValue("@nom", SqlDbType.NVarChar).Value = textBox2.Text;
            SqlDataReader DR = Requete.ExecuteReader();
            DR.Read(); // Ouverture de l'objet SqlDataReader
            if (DR.HasRows)
            {
                textBox1.Text = DR[8].ToString();
                textBox2.Text = DR[7].ToString();
                textBox3.Text = DR[6].ToString();
                textBox4.Text = DR[5].ToString();
                textBox5.Text = DR[4].ToString();
                textBox6.Text = DR[3].ToString();
                textBox7.Text = DR[2].ToString();
                textBox8.Text = DR[1].ToString();
                textBox9.Text = DR[0].ToString();
            }
            else
            {
                MessageBox.Show("Etudiant n'existe pas !!", "Information", MessageBoxButtons.OK);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox9.Clear();
            }
            DR.Close();
        }

        //*****************MODIFIER*****************
        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand Req = new SqlCommand();
            Req.Connection = maconnexion;
            Req.CommandText = "Update AppEtudiant set تاريخ_اغلاق_المؤسسة=@pdatefrmt,المؤسسة=@pecl,تاريخ_الانقطاع_الدراسي=@pdatesrt,الموسم_الدراسي=@panneescl,المستوى_الدراسي=@pniveau,تاريخ_الازدياد=@pdatenaiss,النسب=@pprenom,الإسم=@pnom where رقم_مسار = @pNum";
            Req.Parameters.AddWithValue("@pNum", SqlDbType.NVarChar).Value = textBox1.Text;
            Req.Parameters.AddWithValue("@pnom", SqlDbType.NVarChar).Value = textBox2.Text;
            Req.Parameters.AddWithValue("@pprenom", SqlDbType.NVarChar).Value = textBox3.Text;
            Req.Parameters.AddWithValue("@pdatenaiss", SqlDbType.NVarChar).Value = textBox4.Text;
            Req.Parameters.AddWithValue("@pniveau", SqlDbType.NVarChar).Value = textBox5.Text;
            Req.Parameters.AddWithValue("@panneescl", SqlDbType.NVarChar).Value = textBox6.Text;
            Req.Parameters.AddWithValue("@pdatesrt", SqlDbType.NVarChar).Value = textBox7.Text;
            Req.Parameters.AddWithValue("@pecl", SqlDbType.NVarChar).Value = textBox8.Text;
            Req.Parameters.AddWithValue("@pdatefrmt", SqlDbType.NVarChar).Value = textBox9.Text;
            int l = Req.ExecuteNonQuery();
            if (l == 0)
            {
                MessageBox.Show("Aucune Mise à Jour !!");
            }
            else
            {
                MessageBox.Show("Mise à Jour bien effectuée");
                RemplirDataGrid();
            }
        }

        //***************SUPPRIMER*************
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Vous dever remplir le numero");
                textBox1.Focus();
            }
            else
            {
                if (MessageBox.Show("Voulez-vous supprimer cet etudiant?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    req.Connection = maconnexion;
                    req.CommandText = "Delete from AppEtudiant where رقم_مسار = @num";
                    req.CommandType = CommandType.Text; // Changer le type de la requete en text (non pas procedure)
                                                        // ******************* Debut Preparation des parametres ************************************
                    req.Parameters.Clear(); // Vider la liste des parametres
                    req.Parameters.Add("@num", SqlDbType.NVarChar).Value = textBox1.Text;
                    int x = req.ExecuteNonQuery();
                    if (x != 0)
                    {
                        MessageBox.Show("Suppression bien faite ", "Information");
                        EffacerEcran(this);
                        RemplirDataGrid();
                    }
                }


            }
        }
        
        //*************VIDER******************
        private void button5_Click(object sender, EventArgs e)
        {
            EffacerEcran(this);
        }

        //*************IMPRESSION****************
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font f1 = new Font("Arial", 30, FontStyle.Bold);

            Font f3 = new Font("Arial", 16, FontStyle.Bold);
            e.Graphics.DrawString("شهادة الإنقطاع عن الدراسة", f1, Brushes.Black, 200, 280);
            e.Graphics.DrawString("________________", f1, Brushes.Black, 200, 290);
            Font f2 = new Font("Arial", 16, FontStyle.Regular);
            e.Graphics.DrawString("يشهد السيد المدير الإقليمي لوزارة التربية الوطنية و التكوين المهني ببني ملال،وبناءعلى ما", f2, Brushes.Black, 50, 400);
            e.Graphics.DrawString("-لديه من مستندات بمصلحة الشؤون التربوية - مكتب التعليم الخصوصي", f2, Brushes.Black, 250, 450);
            e.Graphics.DrawString("  (أن التلميذ(ة", f2, Brushes.Black, 510, 500);
            e.Graphics.DrawString(textBox2.Text + " " + textBox3.Text + " : ", f3, Brushes.Black, 350, 500);
            e.Graphics.DrawString(" المزداد(ة) في", f2, Brushes.Black, 505, 550);
            e.Graphics.DrawString(textBox4.Text + " : ", f3, Brushes.Black, 380, 550);
            e.Graphics.DrawString("كان(ت) يتابع دراسته(ا) بمستوى", f2, Brushes.Black, 370, 600);
            e.Graphics.DrawString(textBox5.Text + " : ", f3, Brushes.Black, 230, 600);



            e.Graphics.DrawString(textBox6.Text + " : ", f3, Brushes.Black, 90, 650);
            e.Graphics.DrawString("خلال الموسم الدراسي ", f2, Brushes.Black, 230, 650);
            e.Graphics.DrawString(textBox1.Text + " : ", f3, Brushes.Black, 410, 650);
            e.Graphics.DrawString("تحت رقم", f2, Brushes.Black, 540, 650);

            e.Graphics.DrawString(textBox7.Text + " : ", f3, Brushes.Black, 250, 700);
            e.Graphics.DrawString("وقد تابع(ت) الدراسة إلى غاية ", f2, Brushes.Black, 395, 700);


            e.Graphics.DrawString("بمؤسسة", f2, Brushes.Black, 555, 750);
            e.Graphics.DrawString(textBox8.Text + " : " + " ", f3, Brushes.Black, 420, 750);
            e.Graphics.DrawString("للتعليم المدرسي الخصوصي ببني ملال", f2, Brushes.Black, 140, 750);


            e.Graphics.DrawString(textBox9.Text + " : ", f3, Brushes.Black, 330, 800);
            e.Graphics.DrawString("والتي أغلقت بتاريخ", f2, Brushes.Black, 470, 800);

            e.Graphics.DrawString(DateTime.Now.ToShortDateString() + " : ", f3, Brushes.Black, 150, 900);
            e.Graphics.DrawString("حرر ببني ملال، بتاريخ", f2, Brushes.Black, 290, 900);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        //*********************CHERCHER A PARTIR DE DATAGRIDCLICK******************
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            string num, nom, prenom, datenaiss, niveau, anneescl, datesrt, ecl, datefrmt;
            num = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            nom = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            prenom = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            datenaiss = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            niveau = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            anneescl = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            datesrt = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            ecl = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            datefrmt = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            textBox1.Text = num;
            textBox2.Text = nom;
            textBox3.Text = prenom;
            textBox4.Text = datenaiss;
            textBox5.Text = niveau;
            textBox6.Text = anneescl;
            textBox7.Text = datesrt;
            textBox8.Text = ecl;
            textBox9.Text = datefrmt;
        }

        //******************QUITTER************************
        private void button7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Voulez-vous quitter?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
