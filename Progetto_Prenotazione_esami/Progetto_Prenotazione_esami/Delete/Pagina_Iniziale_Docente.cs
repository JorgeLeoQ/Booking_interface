using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Progetto_Prenotazione_esami
{
    public partial class Pagina_Iniziale_Docente : Form
    {
        public string id_utente;
        public DataBase db = new DataBase();
        public List<string[]> lista_esami = new List<string[]>();
        public List<string> lista_materie = new List<string>();
        public Login log;

        public Pagina_Iniziale_Docente(Login log)
        {
            this.log = log;
            InitializeComponent();
            //lista_esami.Add(new Esame("Controlli digitali", 123456789, DateTime.Today, DateTime.Today, "scritto", "B0", "9:00", "1", ""));
            lista_esami = db.printExamI(id_utente);
            lista_materie = db.printMaterieI(id_utente);
        }

        private void Pagina_Iniziale_Docente_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {/*
            if (lista_esami.Count != 0)
            {
                foreach (string[] x in lista_esami)
                    //MessageBox.Show(x);
            }
            else
            {
                MessageBox.Show("Non hai ancora inserito alcun esame");
            }*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegistrazioneEsame next_window = new RegistrazioneEsame(lista_materie, this);
            (next_window).Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            //Notifica_Log_Out next_window = new Notifica_Log_Out(this);
            //(next_window).Show();
        }
    }
}
