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
    public partial class RegistrazioneEsame : Form
    {
        private Esame result;
        public List<string> lista_materie;
        List<DateTime> EsamiGiàPrenotati = new List<DateTime>();
        public Pagina_Iniziale_Docente PID;
        private DateTime selected;

        public RegistrazioneEsame(List<string> list, Pagina_Iniziale_Docente PID)
        {
            this.PID = PID;
            lista_materie = list;
            InitializeComponent();
            monthCalendar1.BackColor = System.Drawing.Color.White;
            monthCalendar1.TrailingForeColor = System.Drawing.Color.Red;
            monthCalendar1.TitleForeColor = System.Drawing.Color.Yellow;
            this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            monthCalendar1.TitleBackColor = System.Drawing.Color.Blue;
            monthCalendar1.TrailingForeColor = System.Drawing.Color.Red;
            monthCalendar1.TitleForeColor = System.Drawing.Color.Yellow;

            AddNewExam();
            foreach (string x in lista_materie)
                comboBox5.Items.Add(x);
            comboBox6.Items.Add("Scritto");
            comboBox6.Items.Add("Orale");
            comboBox6.Items.Add("Scritto-Orale");
            comboBox6.Items.Add("Compitino");

            for (int i = 1; i < 13; i++)
            {
                comboBox2.Items.Add(i.ToString());
            }
            comboBox2.Text = "1";

            for (int i = 8; i < 14; i++) {
                if(i<10)
                    comboBox4.Items.Add("0"+i.ToString()+"\n");
                else
                    comboBox4.Items.Add(i.ToString() + "\n");
            }
            comboBox3.Items.Add("00");
            comboBox3.Items.Add("30");
            foreach(DateTime x in EsamiGiàPrenotati)
            {

            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void monthCalendar1_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            selected = new DateTime(e.Start.Year, e.Start.Month, e.Start.Day);
            if (EsamiGiàPrenotati.IndexOf(selected) == -1)
            {
                EsamiGiàPrenotati.Add(selected);
                textBox1.Text = "Hai prenotato: " + selected.ToShortDateString();
                AddNewExam();
            }
            else
                textBox1.Text = "Non puoi prenotare: " + selected.ToShortDateString();
        }

        private void monthCalendar2_DateSelected_1(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            selected = new DateTime(e.Start.Year, e.Start.Month, e.Start.Day);
            if (EsamiGiàPrenotati.IndexOf(selected) == -1)
            {
                EsamiGiàPrenotati.Add(selected);
                textBox1.Text = "Hai prenotato: " + selected.ToShortDateString();
                AddNewExam();
            }
            else
                textBox1.Text = "Non puoi prenotare: " + selected.ToShortDateString();
        }


        public void AddNewExam()
        {
            DateTime[] bolding = new DateTime[EsamiGiàPrenotati.Count + 1];
            int i = 0;
            foreach (DateTime d in EsamiGiàPrenotati)
            {
                bolding[i] = d;
                i++;
            }
            monthCalendar1.BoldedDates = bolding;
            monthCalendar2.BoldedDates = bolding;
        }

        private void monthCalendar2_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void comboBox2_TextUpdate(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox1.Items.Clear();
            if (string.Compare(comboBox2.Text, "1") == 0 || string.Compare(comboBox2.Text, "3") == 0 || string.Compare(comboBox2.Text, "5") == 0 || string.Compare(comboBox2.Text, "7") == 0 || string.Compare(comboBox2.Text, "8") == 0 || string.Compare(comboBox2.Text, "10") == 0 || string.Compare(comboBox2.Text, "12") == 0)
            {
                for (int i = 1; i < 32; i++)
                {
                    comboBox1.Items.Add(i.ToString());
                }
            }
            else if (string.Compare(comboBox2.Text, "4") == 0 || string.Compare(comboBox2.Text, "6") == 0 || string.Compare(comboBox2.Text, "9") == 0 || string.Compare(comboBox2.Text, "11") == 0)
            {
                for (int i = 1; i < 31; i++)
                {
                    comboBox1.Items.Add(i.ToString());
                }
            }
            else
            {
                for (int i = 1; i < 29; i++)
                {
                    comboBox1.Items.Add(i.ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            result = new Esame(
                comboBox5.Text,
                123456789,
                selected,
                new DateTime(selected.Year, selected.Month, selected.Day),
                comboBox6.Text,
                textBox3.Text,
                comboBox4.Text + ":" + comboBox3.Text,
                PID.id_utente,
                textBox2.Text);
            Conferma x = new Conferma(result, this);
            x.Show();
        }
    }
}
