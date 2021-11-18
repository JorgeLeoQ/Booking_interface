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
    public partial class Interfaccia_Studente : Form
    {
        private List<string> id_P = new List<string>();
        private List<List<string>> lista_esami_P = new List<List<string>>();
        public WARNING.Warning w = new WARNING.Warning();
        public int index = -1;
        private string id;
        public Login log;
        private DateTime[] bolding = new DateTime[180];

        public DataBase db = new DataBase();

        public Interfaccia_Studente(Login log, string id)
        {
            this.id = id;
            this.log = log;
            InitializeComponent();
            Prenota.Hide();
            db.ReturnMaterie(id, 1);
            gestisciCalendario();
            w.Hide();
            gestisciLista();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            int count = -1;
            index = -1;
            Info_esame.Items.Clear();
            string id_esame_selezionato = "-1";

            DateTime selected = new DateTime(e.Start.Year, e.Start.Month, e.Start.Day);
            string data = selected.Day.ToString() + "/" + selected.Month.ToString() + "/" + selected.Year.ToString();
            if (bolding.ToList<DateTime>().IndexOf(selected) != -1) //ho selezionato un esame
            {
                foreach (List<string> x in db.lista_esami_totali)
                {
                    count++;
                    if (x[4].Equals(data) && db.ReturnMaterie(id, 1).IndexOf(x[x.Count - 1]) != -1)
                    {
                        index = count;
                        //Esame che ci interessa
                        for (int i = 0; i < x.Count - 1; i++)
                        {
                            Info_esame.Items.Add(x[i]);
                            textBox2.Text = x[1] + " " + x[5];
                        }

                    }
                }
            }
            if (Info_esame.Items.Count != 0)
            {
                bool control = false;
                if (db.idEsamiPrenotati(id) != null)
                {
                    foreach (string x in db.idEsamiPrenotati(id))
                    {
                        if (db.lista_esami_totali[index][0].Equals(x))
                        {
                            control = true;
                        }
                    }
                }
                if (control)
                {
                    Prenota.Text = "disdici";
                }
                else
                    Prenota.Text = "Prenota";
                Prenota.Show();
            }
        }

        private void Prenota_Click(object sender, EventArgs e)
        {
            if (Prenota.Text.Equals("Prenota"))
            {
                db.registrazione_Esame(id, db.lista_esami_totali[index][0]);
                w.text("Esame prenotato\ncon successo");
                w.Show();
                Prenota.Hide();
                listBox1.Items.Clear();
                gestisciLista();
            }
            else
            {
                if (index != -1)
                {
                    db.disdetta_esame(id, db.lista_esami_totali[index][0]);
                    w.text("Esame disdetto con successo");
                    w.Show();
                    Prenota.Hide();
                    listBox1.Items.Clear();
                    gestisciLista();
                }
                else
                {
                    index++;
                    for (int j = 0; j < db.lista_esami_totali.Count; j++)
                    {
                        index = 0;
                        for (int i = 0; i < db.idEsamiPrenotati(id).Count; i++)
                        {
                            if (!db.lista_esami_totali[0].Equals(db.idEsamiPrenotati(id)[i]))
                                index++;
                            else
                            {
                                i = db.idEsamiPrenotati(id).Count;
                                j = db.lista_esami_totali.Count;
                            }
                        }
                    }

                    db.disdetta_esame(id, db.lista_esami_totali[index][0]);
                    w.text("Esame disdetto con successo");
                    Info_esame.Items.Clear();
                    w.Show();
                    Prenota.Hide();
                    listBox1.Items.Clear();
                    gestisciLista();
                }
            }
        }


        private void gestisciCalendario()
        {
            int count;
            int j = 0;
            foreach (List<String> x in db.lista_esami_totali)
            {
                for (int i = 0; i < db.ReturnMaterie(id, 1).Count; i++)
                {

                    if (db.ReturnMaterie(id, 1)[i].Equals(x[x.Count - 1]))
                    {
                        int d = 0;
                        int m = 0;
                        int y = 0;
                        string res = "";
                        count = 0;

                        res = "";
                        foreach (char l in x[4].ToCharArray())
                        {
                            if ('/' != l)
                            {
                                res = res + l;
                            }
                            if ('/' == l && count == 1)
                            {
                                Int32.TryParse(res, out m);
                                res = "";
                                count++;
                            }

                            if ('/' == l && count == 0)
                            {
                                Int32.TryParse(res, out d);
                                res = "";
                                count++;
                            }
                        }
                        Int32.TryParse(res, out y);
                        if (DateTime.Now.Year >= y)
                            if (DateTime.Now.Month == m && DateTime.Now.Day < d)
                                bolding[j] = new DateTime(y, m, d);
                            else if (DateTime.Now.Month < m)
                                bolding[j] = new DateTime(y, m, d);

                        j++;
                    }
                }
            }
            monthCalendar1.BoldedDates = bolding;
        }

        private void gestisciLista()
        {
            listBox1.Items.Clear();
            List<string> ret = new List<string>();
            lista_esami_P = new List<List<string>>();
            foreach (string x in db.idEsamiPrenotati(id))
            {
                foreach (List<string> y in db.lista_esami_totali)
                {
                    if (x.Equals(y[0]))
                    {
                        lista_esami_P.Add(y);
                    }
                }
            }
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            for (int i = 0; i < lista_esami_P.Count; i++)
            {
                listBox1.Items.Add(lista_esami_P[i][1]);
                listBox1.Items.Add("");
                listBox2.Items.Add(lista_esami_P[i][5]);
                listBox2.Items.Add("");
                listBox3.Items.Add(lista_esami_P[i][4]);
                listBox3.Items.Add("");
            }
            Controllo_esami_passati();

        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            listBox3.SelectedIndex = listBox1.SelectedIndex;
            listBox2.SelectedIndex = listBox1.SelectedIndex;
            int count = -1;
            if (listBox1.SelectedIndex != -1)
            {
                if (!listBox1.Items[listBox1.SelectedIndex].Equals("") && listBox1.Items.Count > 1)
                {
                    foreach (List<string> x in lista_esami_P)
                    {
                        if (x[1].Equals(listBox1.Items[listBox1.SelectedIndex]) && x[4].Equals(listBox3.Items[listBox1.SelectedIndex]))
                        {

                            Info_esame.Items.Clear();
                            label8.Hide();
                            for (int i = 0; i < x.Count; i++)
                            {
                                switch (i)
                                {
                                    case 2:
                                        break;
                                    case 3:
                                        Info_esame.Items.Add(x[i + 1]);
                                        break;
                                    case 4:
                                        Info_esame.Items.Add(x[i - 1]);
                                        break;
                                    case 9:
                                        label8.Show();
                                        Info_esame.Items.Add(x[i]);
                                        break;
                                    default:
                                        Info_esame.Items.Add(x[i]);
                                        break;
                                }

                            }
                        }
                    }
                }
                if (!listBox1.Items[listBox1.SelectedIndex].Equals(""))
                {
                    Prenota.Text = "Disdici";
                    Prenota.Show();
                }
                if (listBox1.Items[listBox1.SelectedIndex].Equals(""))
                {
                    Prenota.Hide();
                    Info_esame.Items.Clear();
                }
            }
            
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox2.SelectedIndex;
            listBox3.SelectedIndex = listBox2.SelectedIndex;
            int count = -1;
            if (listBox1.SelectedIndex != -1)
            {
                if (!listBox1.Items[listBox1.SelectedIndex].Equals("") && listBox1.Items.Count > 1)
                {
                    foreach (List<string> x in lista_esami_P)
                    {
                        if (x[1].Equals(listBox1.Items[listBox1.SelectedIndex]) && x[4].Equals(listBox3.Items[listBox1.SelectedIndex]))
                        {

                            Info_esame.Items.Clear();
                            label8.Hide();
                            for (int i = 0; i < x.Count; i++)
                            {
                                switch (i)
                                {
                                    case 2:
                                        break;
                                    case 3:
                                        Info_esame.Items.Add(x[i + 1]);
                                        break;
                                    case 4:
                                        Info_esame.Items.Add(x[i - 1]);
                                        break;
                                    case 9:
                                        label8.Show();
                                        Info_esame.Items.Add(x[i]);
                                        break;
                                    default:
                                        Info_esame.Items.Add(x[i]);
                                        break;
                                }

                            }
                        }
                    }
                }
                if (!listBox1.Items[listBox1.SelectedIndex].Equals(""))
                {
                    Prenota.Text = "Disdici";
                    Prenota.Show();
                }
                if (listBox1.Items[listBox1.SelectedIndex].Equals(""))
                {
                    Prenota.Hide();
                    Info_esame.Items.Clear();
                }
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox3.SelectedIndex;
            listBox2.SelectedIndex = listBox3.SelectedIndex;
            int count = -1;
            if (listBox1.SelectedIndex != -1)
            {
                if (!listBox1.Items[listBox1.SelectedIndex].Equals("") && listBox1.Items.Count > 1)
                {
                    foreach (List<string> x in lista_esami_P)
                    {
                        if (x[1].Equals(listBox1.Items[listBox1.SelectedIndex]) && x[4].Equals(listBox3.Items[listBox1.SelectedIndex]))
                        {

                            Info_esame.Items.Clear();
                            label8.Hide();
                            for (int i = 0; i < x.Count; i++)
                            {
                                switch (i)
                                {
                                    case 2:
                                        break;
                                    case 3:
                                        Info_esame.Items.Add(x[i + 1]);
                                        break;
                                    case 4:
                                        Info_esame.Items.Add(x[i - 1]);
                                        break;
                                    case 9:
                                        label8.Show();
                                        Info_esame.Items.Add(x[i]);
                                        break;
                                    default:
                                        Info_esame.Items.Add(x[i]);
                                        break;
                                }

                            }
                        }
                    }
                }
                if (!listBox1.Items[listBox1.SelectedIndex].Equals(""))
                {
                    Prenota.Text = "Disdici";
                    Prenota.Show();
                }
                if (listBox1.Items[listBox1.SelectedIndex].Equals(""))
                {
                    Prenota.Hide();
                    Info_esame.Items.Clear();
                }
            }
        }

        private void Controllo_esami_passati()
        {
            List<int> indexFilter = new List<int>();
            int count;
            int d = 0;
            int m = 0;
            int y = 0;
            string res = "";
            for (int i = 0; i < listBox3.Items.Count; i = i + 2)
            {
                d = 0;
                m = 0;
                y = 0;
                res = "";


                count = 0;

                res = "";
                foreach (char l in listBox3.Items[i].ToString().ToCharArray())
                {
                    if ('/' != l)
                    {
                        res = res + l;
                    }
                    if ('/' == l && count == 1)
                    {
                        Int32.TryParse(res, out m);
                        res = "";
                        count++;
                    }

                    if ('/' == l && count == 0)
                    {
                        Int32.TryParse(res, out d);
                        res = "";
                        count++;
                    }
                }
                Int32.TryParse(res, out y);

                if(DateTime.Now.Year < y)
                    indexFilter.Add(i);

                if (DateTime.Now.Year >= y)
                    if (!(DateTime.Now.Month == m && DateTime.Now.Day < d))
                        indexFilter.Add(i);

                    else if (DateTime.Now.Month > m)
                        indexFilter.Add(i);
            }

            for(int i = indexFilter.Count-1; i>-1; i--)
            {
                listBox1.Items.RemoveAt(i+1);
                listBox1.Items.RemoveAt(i);
                listBox2.Items.RemoveAt(i + 1);
                listBox2.Items.RemoveAt(i);
                listBox3.Items.RemoveAt(i + 1);
                listBox3.Items.RemoveAt(i);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Notifica_Log_Out next_window = new Notifica_Log_Out(this);
            (next_window).Show();
        }
    }

}