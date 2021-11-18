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
    public partial class Login : Form
    {
        bool studente = false;
        bool insegnante = false;
        List<string> nome_utente = new List<string>();
        List<string> numero_matricola = new List<string>();
        List<string> password = new List<string>();
        private bool login;
        public bool Log { get { return login; } set { login = true; } }
        DataBase db = new DataBase();

        public Login()
        {
            login = false;
            InitializeComponent();
            button1.Text = "login";
            label1.Text = "numero di matricola:";
            
            label2.Text = "Password:";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (db.Login_Control(textBox1.Text, textBox2.Text))
            {
                case 1:
                    MessageBox.Show("Studente hai eseguito l'accesso con successo");
                    studente = true;
                    Form1_FormClosed(textBox1.Text);
                    break;
                case 0:
                    MessageBox.Show("Insegnante hai eseguito l'accesso con successo");
                    insegnante = true;
                    Form1_FormClosed(textBox1.Text);
                    break;
                default:
                    MessageBox.Show("Password o nome utente sbagliati");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    break;

            }
        }

        private void Form1_FormClosed(string id)
        {
            this.Hide();
            if (insegnante)
            {
                Pagina_Insegnante next_window = new Pagina_Insegnante(this, id, db.ReturnMaterie(id, 0));
                (next_window).Show();
                textBox1.Text = "";
                textBox2.Text = "";
            }
            if(studente)
            {
                Interfaccia_Studente next_window = new Interfaccia_Studente(this, id);
                (next_window).Show();
            }
            studente = false;
            insegnante = false;
        }
    }
}
