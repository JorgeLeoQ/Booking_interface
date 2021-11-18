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
    public partial class Conferma : Form
    {
        Pagina_Iniziale_Docente PID;
        RegistrazioneEsame RE;
        Esame result;

        public Conferma(Esame x, RegistrazioneEsame RE)
        {
            result = x;
            this.RE = RE;
            InitializeComponent();
            label1.Text = x.ToString();
        }
        /*
        private void button2_Click(object sender, EventArgs e)
        {
            RE.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PID = RE.PID;
            RE.Close();
            PID.lista_esami.Add(result);
            PID.Show();
            this.Close();
        }*/
    }
}
