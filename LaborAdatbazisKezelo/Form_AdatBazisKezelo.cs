using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaborAdatbaziKezelo
{
    class Form_AdatbázisKezelő: Form
    {
        public static string server = ".\\SQLEXPRESS";
        public static bool integrated_security = false;
        public static string labor_database = "Labor";
        public static string sql_username = "labor";
        public static string sql_password = "labor";

        private SqlConnection laborconnection;

        TextBox[] rekordok;
        TextBox[] típusok;
        TextBox box_tábla;

        public Form_AdatbázisKezelő()
        {
            InitializeForm();
            Configurate();
            string LaborConnectionString = @"Data Source=" + server + ";Initial Catalog=" +
                labor_database + ";User ID=" + sql_username + ";Password=" + sql_password + ";" + (integrated_security ? "Integrated Security=true;" : "");
            laborconnection = new SqlConnection(LaborConnectionString);
            laborconnection.Open();
            InitilizeContent();
            laborconnection.Close();
        }

        public void InitializeForm()
        {
            Text = "Adatbázis Kezelő";
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            ClientSize = new Size(312, 256);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = ClientSize;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        }

        public void InitilizeContent()
        {
            Label label_tábla = new Label();
            label_tábla.Text = "Módosítandó tábla: ";
            label_tábla.Location = new Point(10, 20);
            label_tábla.Width = 100;

            Label label_név = new Label();
            label_név.Text = "Rekord neve";
            label_név.Location = new Point(10, 50);
            label_név.Width = 70;

            Label label_típus = new Label();
            label_típus.Text = "Új típus";
            label_típus.Location = new Point(150, 50);

            box_tábla = new TextBox();
            box_tábla.Width = 100;
            box_tábla.Location = new Point(label_tábla.Location.X + label_tábla.Width + 40, label_tábla.Location.Y - 3);

            rekordok = new TextBox[3];
            típusok = new TextBox[3];

            Controls.Add(box_tábla);

            for (int i = 0; i < rekordok.Length; i++)
            {
                rekordok[i] = new TextBox();
                rekordok[i].Width = 100;
                rekordok[i].Location = new Point(label_név.Location.X, label_név.Location.Y + i * 40 + 20);

                típusok[i] = new TextBox();
                típusok[i].Width = 100;
                típusok[i].Location = new Point(label_típus.Location.X, label_név.Location.Y + i * 40 + 20);

                Controls.Add(rekordok[i]);
                Controls.Add(típusok[i]);
            }

            Button rendben = new Button();
            rendben.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            rendben.Text = "Rendben";
            rendben.Size = new System.Drawing.Size(96, 32);
            rendben.Location = new Point(ClientRectangle.Width - rendben.Size.Width - 16, ClientRectangle.Height - rendben.Size.Height - 16);
            rendben.Click += rendben_Click;

            Button csere = new Button();
            csere.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            csere.Text = "tinyint->smallint";
            csere.Size = new System.Drawing.Size(96, 32);
            csere.Location = new Point(ClientRectangle.Width - csere.Size.Width - 92 - rendben.Width, ClientRectangle.Height - csere.Size.Height - 16);
            csere.Click += csere_Click;

            Controls.Add(label_név);
            Controls.Add(label_típus);
            Controls.Add(label_tábla);
            Controls.Add(rendben);
            Controls.Add(csere);
        }

        void csere_Click(object sender, EventArgs e)
        {
            laborconnection.Open();
            SqlCommand command = laborconnection.CreateCommand();

            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIHOKE smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIASAV smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VICIAD smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIMATO smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIFEFE smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIFEBA smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIOCS1 smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIOCS2 smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIELE1 smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIELE2 smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIPEN1 smallint;";
            command.CommandText += "ALTER TABLE L_VIZSLAP ALTER COLUMN VIPEN2 smallint;";

            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FOASAVT smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FOASAVI smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FONETOT smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FONETOI smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FOHOFOT smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FOHOFOI smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FOCIADT smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FOCIADI smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FOFOHO smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FOSZSZ smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FOBOSAI smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN FOBOSAT smallint;";
            command.CommandText += "ALTER TABLE L_FOGLAL ALTER COLUMN SZSZAM smallint;";

            command.CommandText += "ALTER TABLE L_SZLEV ALTER COLUMN FOFOHO smallint;";

            command.CommandText += "ALTER TABLE L_TAPERTEK ALTER COLUMN TAKIO smallint;";
            command.CommandText += "ALTER TABLE L_TAPERTEK ALTER COLUMN TAKCAL smallint;";

            try{ command.ExecuteNonQuery();}
            catch (Exception){ MessageBox.Show("Hiba történt a módosítások végrehajtása közben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);}
            MessageBox.Show("Sikeres módosítás", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            command.Dispose();
            laborconnection.Close();
        }

        void rendben_Click(object sender, EventArgs e)
        {
            laborconnection.Open();
            SqlCommand command = laborconnection.CreateCommand();
            if (box_tábla.Text == "") { return; }
            for (int i = 0; i < rekordok.Length; i++)
            {
                if (rekordok[i].Text != "" && típusok[i].Text != "")
                {
                    command.CommandText += "ALTER TABLE " + box_tábla.Text + "  ALTER COLUMN " + rekordok[i].Text + " " + típusok[i].Text + ";";
                }
            }

            if (command.CommandText != "")
            {
                if (MessageBox.Show("Biztosan végrehajtja a módosításokat?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try{ command.ExecuteNonQuery(); } catch (Exception) { MessageBox.Show("Hiba történt a módosítások végrehajtása közben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                command.Dispose();
            }
            MessageBox.Show("Sikeres módosítás", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            laborconnection.Close();
        }

        public static void Configurate()
        {
            try
            {
                StreamReader config = new StreamReader("config");

                string line = null;
                while ((line = config.ReadLine()) != null)
                {
                    try
                    {
                        if (0 < line.Length)
                            if (line[0] != '#')
                            {
                                string[] arguments = line.Split('=');

                                switch (arguments[0])
                                {
                                    case "server": server = arguments[1]; break;
                                    case "labor_database": labor_database = arguments[1]; break;
                                    case "integrated_security": integrated_security = true; break;
                                    case "sql_username": sql_username = arguments[1]; break;
                                    case "sql_password": sql_password = arguments[1]; break;
                                }
                            }
                    }
                    catch { }
                }
                config.Close();
            }
            catch { }
        }
    }
}
