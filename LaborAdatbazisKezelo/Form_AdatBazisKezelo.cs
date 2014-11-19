using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaborAdatbaziKezelo
{
    public struct Hordó
    {
        public string termékkód;
        public string id;
        public int db;


        public Hordó(string _termékkód, string _id, int _db)
        {
            termékkód = _termékkód;
            id = _id;
            db= _db;
        }
    }

    class Form_AdatbázisKezelő: Form
    {
        private SqlConnection laborconnection;
        private SqlConnection marillenconnection;
        public static string server = ".\\SQLEXPRESS";
        public static string labor_database = "Labor";
        public static string marillen_database = "marillen2014";
        public static string sql_username = "labor";
        public static string sql_password = "labor";
        public static bool integrated_security = false;


        TextBox[] rekordok;
        TextBox[] típusok;
        TextBox box_tábla;

        public Form_AdatbázisKezelő()
        {
            InitializeForm();
            Configurate();
            string LaborConnectionString = @"Data Source=" + server + ";Initial Catalog=" +
                           labor_database + ";User ID=" + sql_username + ";Password=" + sql_password + ";Integrated Security=true;";
            string MarillenConnectionString = @"Data Source=" + server + ";Initial Catalog=" + marillen_database + ";" +
                (integrated_security ? "Integrated Security=true;" : "User ID=" + sql_username + ";Password=" + sql_password + ";");

            marillenconnection = new SqlConnection(MarillenConnectionString);
            

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
            ClientSize = new Size(312, 306);
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
            csere.Location = new Point(ClientRectangle.Width - csere.Size.Width - 92 - rendben.Width, ClientRectangle.Height - csere.Size.Height - 64);
            csere.Click += csere_Click;

            Button minbiz = new Button();
            minbiz.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            minbiz.Text = "MinBiz fix szöveg ";
            minbiz.Size = new System.Drawing.Size(96, 32);
            minbiz.Location = new Point(ClientRectangle.Width - minbiz.Size.Width - 92 - rendben.Width, ClientRectangle.Height - minbiz.Size.Height - 16);
            minbiz.Click += minbiz_Click;


            Button hordo = new Button();
            hordo.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            hordo.Text = "hordo";
            hordo.Size = new System.Drawing.Size(96, 32);
            hordo.Location = new Point(ClientRectangle.Width - hordo.Size.Width -16, ClientRectangle.Height - hordo.Size.Height - 64);
            hordo.Click += hordo_Click;
            
            Controls.Add(label_név);
            Controls.Add(label_típus);
            Controls.Add(label_tábla);
            Controls.Add(rendben);
            Controls.Add(csere);
            Controls.Add(minbiz);
            Controls.Add(hordo);
        }

    
        private void hordo_Click(object sender, EventArgs e)
        {
            Form_Hordók hordók = new Form_Hordók();
            hordók.ShowDialog();

            /*
            List<hordo> hordok = new List<hordo>();
            laborconnection.Open();
            SqlCommand command = laborconnection.CreateCommand();
            command.CommandText = "SELECT VITEKO,VISARZ FROM L_VIZSLAP GROUP BY VITEKO, VISARZ ORDER BY VITEKO, VISARZ ";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                hordok.Add(new hordo(reader.GetString(0), reader.GetString(1)));
                //Console.WriteLine(reader.GetString(0) + " " + reader.GetString(1));
            }

            command.Dispose();
            laborconnection.Close();
            reader.Close();
            List<string> asd = new List<string>();
            int c = 0;
            foreach (hordo item in hordok)
            {
                string iPROD_ID = "12" + item.viteko.Substring(0, 2) + "01" + item.viteko[2];
                marillenconnection.Open();
                command = marillenconnection.CreateCommand();
                command.CommandText = "SELECT tetelek.prod_id, tetelek.qty, tetelek.time_ FROM tetelek" +
                                            " INNER JOIN folyoprops ON tetelek.serial_nr=folyoprops.serial_nr" +
                                            " WHERE left(tetelek.prod_id,7) = '" + iPROD_ID + "' AND folyoprops.code= '3' AND folyoprops.propstr = '" + item.visarz + "'  ORDER BY tetelek.prod_id;";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    c++;
                    if (!(asd.Contains(reader.GetString(0).Substring(reader.GetString(0).Length - 4))))
                    {
                        asd.Add(reader.GetString(0).Substring(reader.GetString(0).Length - 4));
                    }
                    else
                    {
                        Console.WriteLine( item.viteko +" " +  item.visarz);// +  reader.GetString(0).Substring(reader.GetString(0).Length - 4));//+ " " + reader.GetDecimal(1) + reader.GetDateTime(2).ToString());
                    }
                }
                Console.WriteLine("c: " + c);

                reader.Close();
                marillenconnection.Close();
            }
            */
        }

        

        void minbiz_Click(object sender, EventArgs e)
        {
            laborconnection.Open();
            SqlCommand command = laborconnection.CreateCommand();
            command.CommandText = "INSERT INTO L_MINBIZ (MISZ1M,MISZ1A,MISZ2M,MISZ2A) VALUES(" +
                "'Alulírott Marillen Kft. kijelenti, hogy a fenti termék mindenben megfelel az érvényes magyar előírásoknak.'," +
                "'Marillen Kft. certifies that the above mentioned product is in accordance with current Hungarian legislation.'," +
                "'Alulírott Marillen Kft. nevében kijelentem, hogy az általunk gyártott aszeptikus velő nem génmanipulált termék. Génmanipulált alap- és segédanyagokat, ill. allergén anyagokat nem tartalmaz.'," +
                "'Aseptic purees produced by Marillen Ltd. are not genetically modified and don’t contain any genetically modified raw materials and additives.')";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                return;
            }
            finally
            {
                MessageBox.Show("Sikeres módosítás", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                command.Dispose();
                laborconnection.Close();
            }
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
                                    case "marillen_database": marillen_database = arguments[1]; break;
                                    case "sql_username": sql_username = arguments[1]; break;
                                    case "sql_password": sql_password = arguments[1]; break;
                                    case "integrated_security": integrated_security = true; break;
                                }
                            }
                    }
                    catch { }
                }
                config.Close();
            }
            catch { }
        }

        public sealed class Form_Hordók : Form
        {
            private SqlConnection laborconnection;

            protected DataGridView table;
            protected DataTable data;
            public Form_Hordók()
            {
                string LaborConnectionString = @"Data Source=" + server + ";Initial Catalog=" +
                           labor_database + ";User ID=" + sql_username + ";Password=" + sql_password + ";Integrated Security=true;";
               
              
                laborconnection = new SqlConnection(LaborConnectionString);
                

                InitializeForm();
                InitializeContent();
                InitializeData();
            }
            private void InitializeForm()
            {
                Text = "Hordók";
                ClientSize = new Size(512, 568);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            public List<Hordó> Hordók()
            {
                List<Hordó> value = new List<Hordó>();

                laborconnection.Open();
                SqlCommand command = laborconnection.CreateCommand();
                command.CommandText = "SELECT HOTEKO, HOSZAM, COUNT(*) AS DB FROM L_HORDO GROUP BY HOTEKO, HOSZAM ORDER BY DB DESC";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    value.Add(new Hordó(reader.GetString(0), reader.GetString(1), reader.GetInt32(2)));
                }

                command.Dispose();
                laborconnection.Close();
                return value;
            }

            private void InitializeContent()
            {
                table = new DataGridView();
                table.Dock = DockStyle.Left;
                table.RowHeadersVisible = false;
                table.AllowUserToResizeRows = false;
                table.AllowUserToResizeColumns = false;
                table.AllowUserToAddRows = false;
                table.Width = 250;
                table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                table.ReadOnly = true;
                table.DataBindingComplete += table_DataBindingComplete;
                table.DataSource = CreateSource();

                Button törlés = new Button();
                törlés.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                törlés.Text = "Törlés";
                törlés.Size = new System.Drawing.Size(96, 32);
                törlés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, ClientRectangle.Height - 32 - 16);
                törlés.Click += törlés_Click;

                Controls.Add(table);
                Controls.Add(törlés);

            }

            public struct Hordó2
            {
                public string termékkód;
                public string sarzs;
                public string id;
                public int? foglalás_száma;
                public string gyártási_év;
                public decimal mennyiség;
                public string time;

                public Hordó2(string _termékkód, string _sarzs, string _id, int? _foglalás_száma, string _gyártási_év, decimal _mennyiség, string _time)
                {
                    termékkód = _termékkód;
                    sarzs = _sarzs;
                    id = _id;
                    foglalás_száma = _foglalás_száma;
                    gyártási_év = _gyártási_év;
                    mennyiség = _mennyiség;
                    time = _time;
                }
            }
            public static T? GetNullable<T>(SqlDataReader _reader, int _column) where T : struct
            {
                if (!_reader.IsDBNull(_column))
                    return _reader.GetFieldValue<T>(_column);
                return null;
            }

            private void törlés_Click(object sender, EventArgs e)
            {

                laborconnection.Open();
                SqlCommand command = laborconnection.CreateCommand();
                command.CommandText = "CREATE TABLE temp(HOTEKO varchar(10), HOSARZ varchar(10), HOSZAM varchar(10), FOSZAM int, VIGYEV varchar(10), HOQTY decimal(14, 2), HOTIME char(30));";
                command.ExecuteNonQuery();
                command.Dispose();

                List<Hordó2> hordók = new List<Hordó2>();
                command = laborconnection.CreateCommand();
                command.CommandText = "SELECT DISTINCT HOTEKO, HOSARZ, HOSZAM, FOSZAM, VIGYEV, HOQTY, HOTIME FROM L_HORDO ORDER BY HOTEKO, HOSARZ, HOSZAM";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    hordók.Add(new Hordó2(reader.GetString(0), reader.GetString(1), reader.GetString(2), GetNullable<int>(reader, 3), reader.GetString(4), reader.GetDecimal(5), reader.GetString(6)));
                }

                command.Dispose();
                reader.Close();

               command = laborconnection.CreateCommand();
                foreach (Hordó2 item in hordók)
                {
                    command.CommandText += "INSERT INTO temp (HOTEKO, HOSARZ, HOSZAM, VIGYEV, HOQTY, HOTIME) VALUES('" + item.termékkód + "','" + item.sarzs + "','" +
                        item.id + "','" +item.gyártási_év + "', " + item.mennyiség.ToString("F2").Replace(',', '.') + ", '" + item.gyártási_év + "');";
                }
                command.ExecuteNonQuery();
                command.Dispose();

                command = laborconnection.CreateCommand();
                command.CommandText = "DROP TABLE L_HORDO; EXEC sp_rename 'temp', 'L_HORDO';";
                 command.ExecuteNonQuery();
                command.Dispose();

                laborconnection.Close();
                Close();
                   //"INSERT INTO temp(HOTEKO,HOSARZ, HOSZAM, FOSZAM , VIGYEV , HOQTY , HOTIME) values ( (SELECT DISTINCT HOTEKO, HOSARZ, HOSZAM, FOSZAM, VIGYEV, HOQTY, HOTIME FROM L_HORDO));";
            }

            private void table_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
            {
                table.DataBindingComplete -= table_DataBindingComplete;
                table.Columns[0].Width = 83;
                table.Columns[1].Width = 83;
                table.Columns[2].Width = 83;
            }
            private void InitializeData()
            {

            }


            private DataTable CreateSource()
            {
                data = new DataTable();

                data.Columns.Add(new DataColumn("Termékkód", System.Type.GetType("System.String")));
                data.Columns.Add(new DataColumn("ID", System.Type.GetType("System.String")));
                data.Columns.Add(new DataColumn("DB", System.Type.GetType("System.Int32")));

                List<Hordó> hordók = Hordók();

                foreach (Hordó item in hordók)
                {
                    if(item.db > 1)
                    {
                        DataRow row = data.NewRow();
                        row[0] = item.termékkód;
                        row[1] = item.id;
                        row[2] = item.db;
                        data.Rows.Add(row);
                    }
                }

                return data;
            }
        }

    }
}
