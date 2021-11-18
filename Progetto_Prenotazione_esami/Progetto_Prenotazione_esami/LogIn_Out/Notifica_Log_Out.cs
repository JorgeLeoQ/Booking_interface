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
    public partial class Notifica_Log_Out : Form
    {
        private Pagina_Insegnante closeWindow;
        private Interfaccia_Studente closeWindow2;
        private Pagina_Iniziale_Docente pagina_Iniziale_Docente;

        public Notifica_Log_Out(Pagina_Insegnante x)
        {
            InitializeComponent();
            closeWindow = x;
        }

        public Notifica_Log_Out(Interfaccia_Studente x)
        {
            InitializeComponent();
            closeWindow2 = x;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (closeWindow != null)
            {
                closeWindow.Show();
            }
            else
            {
                closeWindow2.Show();
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (closeWindow != null)
            {
                closeWindow.log.Show();
                closeWindow.Close();
            }
            else
            {
                closeWindow2.Close();
                closeWindow2.log.Show();
            }
            this.Close();
        }
    }
}
