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
    public partial class Pagina_Insegnante : Form
    {
        private List<string> id_esami_selezionati = new List<string>();

        private DateTime selected;
        public WARNING.Notifica n = new WARNING.Notifica();
        public WARNING.Warning w = new WARNING.Warning();
        public DateTime[] bolding;
        public DateTime[] unbolding;
        public string id_utente;
        public DataBase db = new DataBase();
        public List<string[]> lista_esami = new List<string[]>();
        public List<string> lista_materie = new List<string>();
        public Login log;
        private bool modifica = false;
        private int modifica_Count = -1;

        /// ----- INIZIALIZZAZIONE ----- (aggiornamento calendari)

        public Pagina_Insegnante(Login log, string id, List<String> lista_materie)
        {
            InitializeComponent();
            button4.Hide();

            //monthCalendar1.BackColor = System.Drawing.Color.White;
            //monthCalendar1.TrailingForeColor = System.Drawing.Color.Red;
            //monthCalendar1.TitleForeColor = System.Drawing.Color.Yellow;
            //this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            //monthCalendar1.TitleBackColor = System.Drawing.Color.Blue;
            //monthCalendar1.TrailingForeColor = System.Drawing.Color.Red;
            //monthCalendar1.TitleForeColor = System.Drawing.Color.Yellow;

            label15.Text = "";
            label15.Hide();

            this.log = log;
            id_utente = id;
            this.lista_materie = lista_materie;
            List<string> test = db.printMateries(id_utente);
            foreach (string x in test)
                comboBox5.Items.Add(x);
            //lista_esami.Add(new Esame("Controlli digitali", 123456789, DateTime.Today, DateTime.Today, "scritto", "B0", "9:00", "1", ""));
            UpgradeCalendar();

            comboBox6.Items.Add("Scritto");
            comboBox6.Items.Add("Orale");
            comboBox6.Items.Add("Compitino");
            comboBox6.Items.Add("Scritto/Orale");

            numericUpDown2.Value = DateTime.Now.Month;
            numericUpDown5.Value = DateTime.Now.Month;
        }

        private void Pagina_Iniziale_Docente_Load(object sender, EventArgs e)
        {
        }

        /// ----- BOTTONE DI LOG OUT -----

        private void Log_out_Click(object sender, EventArgs e)
        {
            this.Hide();
            Notifica_Log_Out next_window = new Notifica_Log_Out(this);
            (next_window).Show();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
        }

        /// ----- LIST BOX ELENCO CON TUTTI GLI ESAMI -----

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                listBox4.Items.Clear();
                listBox2.SelectedIndex = listBox1.SelectedIndex;
                Info_Esame.Items.Clear();
                Lista_Studenti_P.Items.Clear();

                object oggetto = listBox1.SelectedItem;

                string x = oggetto.ToString();

                if (x != null)
                {
                    if (!x.Equals(""))
                    {
                        char[] stringa = x.ToCharArray();

                        int count;
                        if ((float)listBox1.SelectedIndex / 2 > 9)
                        {
                            Int32.TryParse(stringa[0].ToString() + stringa[1].ToString(), out count);

                            ControlloStudentiPrenotati(db.printExams(id_utente)[count - 1][0]);

                            foreach (string stampa in lista_esami[count - 1])
                            {

                                Info_Esame.Items.Add(stampa);
                            }
                        }
                        else
                        {
                            Int32.TryParse(stringa[0].ToString(), out count);

                            ControlloStudentiPrenotati(db.printExams(id_utente)[count - 1][0]);

                            foreach (string stampa in lista_esami[count - 1])
                            {

                                Info_Esame.Items.Add(stampa);
                            }
                        }
                    }
                }
            }
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime selected = new DateTime(e.Start.Year, e.Start.Month, e.Start.Day);
            bool control = false;
            foreach (List<string> c in db.lista_esami_totali)
                control = control || c[2].Equals("Creato il:\t" + selected.Day.ToString() + "/" + selected.Month.ToString() + "/" + selected.Year.ToString() + "\n");
            if (unbolding != null)
            {
                if (!control && unbolding.ToList<DateTime>().IndexOf(selected) != -1)
                {
                    numericUpDown1.Value = selected.Day;
                    numericUpDown2.Value = selected.Month;

                }
                else
                {
                    //AGGIUNGI WARNING
                }
            }
        }

        /// ----- INSERIMENTO DATI -----

        private void numericUpDown1_TextUpdate(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_TextUpdate(object sender, EventArgs e)
        {
            numericUpDown1.Value = 1;
            if (numericUpDown2.Value == 1 || numericUpDown2.Value == 3 || numericUpDown2.Value == 5 || numericUpDown2.Value == 7 || numericUpDown2.Value == 8 || numericUpDown2.Value == 10 || numericUpDown2.Value == 12)
            {
                numericUpDown1.Maximum = 31;

            }
            else if (numericUpDown2.Value == 4 || numericUpDown2.Value == 6 || numericUpDown2.Value == 9 || numericUpDown2.Value == 11)
            {
                numericUpDown1.Maximum = 30;
            }
            else
            {
                numericUpDown1.Maximum = 28;
            }
        }

        private void numericUpDown5_TextUpdate(object sender, EventArgs e)
        {
            numericUpDown6.Value = 1;
            if (numericUpDown5.Value == 1 || numericUpDown5.Value == 3 || numericUpDown5.Value == 5 || numericUpDown5.Value == 7 || numericUpDown5.Value == 8 || numericUpDown5.Value == 10 || numericUpDown5.Value == 12)
            {
                numericUpDown6.Maximum = 31;

            }
            else if (numericUpDown5.Value == 4 || numericUpDown5.Value == 6 || numericUpDown5.Value == 9 || numericUpDown5.Value == 11)
            {
                numericUpDown6.Maximum = 30;
            }
            else
            {
                numericUpDown6.Maximum = 28;
            }
        }

        private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
        {
            selected = new DateTime(e.Start.Year, e.Start.Month, e.Start.Day);
            listBox4.Items.Clear();
            List<List<string>> minni = db.printExams(id_utente);
            foreach (List<string> x in db.printExams(id_utente))
            {
                if ((x[4]).Equals(selected.Day.ToString() + "/" + selected.Month.ToString() + "/" + selected.Year.ToString()))
                {
                    listBox4.Items.Add(x[1]);
                    listBox4.Items.Add("");
                    id_esami_selezionati = new List<string>();
                    id_esami_selezionati.Add(x[0]);
                }
            }
        }

        private void listBox4_Click(object sender, EventArgs e)
        {
            if (listBox4.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
                listBox2.SelectedIndex = 0;
                Info_Esame.Items.Clear();
                Lista_Studenti_P.Items.Clear();

                int count = listBox4.SelectedIndex / 2;

                object x = listBox4.SelectedItem;

                if (x != null)
                {
                    if (!x.ToString().Equals(""))
                        Info_Esame.Items.Clear();
                    int i = 0;


                    List<List<string>> pippo = db.InfoExamsI(id_utente);
                    foreach (List<string> C in db.InfoExamsI(id_utente))
                    {
                        string PI = C[1];

                        string carlomagno = selected.Day.ToString() + "/" + selected.Month.ToString() + "/" + selected.Year.ToString();

                        if (x.ToString().Equals(C[1]) && (C[4]).Equals(carlomagno))
                        {
                            ControlloStudentiPrenotati(db.printExams(id_utente)[i][0]);
                            foreach (string stampa in lista_esami[i])
                            {
                                Info_Esame.Items.Add(stampa);
                            }
                        }
                        i++;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox4.Items.Count == 0)
            {
                if (listBox1.Items.Count > 0)
                {
                    if (((getData(listBox2.Items[listBox1.SelectedIndex].ToString())[1] == DateTime.Now.Month && getData(listBox2.Items[listBox1.SelectedIndex].ToString())[0] > DateTime.Now.Day) || (getData(listBox2.Items[listBox1.SelectedIndex].ToString())[1] > DateTime.Now.Month)))
                    {
                        if (!listBox1.Items[listBox1.SelectedIndex].Equals(""))
                        {
                            button4.Show();
                            modifica = true;
                            label15.Text = "Sei in modalità modifica";
                            label15.Show();
                            int i = -1;
                            int count = -1;
                            foreach (string[] x in lista_esami)
                            {
                                i++;
                                if (x[0].Equals(Info_Esame.Items[0]))
                                {
                                    count = i;
                                }
                            }
                            if (count != -1)
                            {
                                comboBox5.Text = lista_esami[count][1].Substring(8);

                                comboBox6.Text = lista_esami[count][5].Substring(11);

                                textBox3.Text = lista_esami[count][6].Substring(7);

                                int[] data = getData(count);
                                int d = data[0];
                                int m = data[1];
                                int y = data[2];

                                numericUpDown2.Value = m;
                                numericUpDown1.Value = d;

                                int h = 0;
                                int min = 0;
                                string res = "";

                                int contatore = 0;
                                res = "";
                                foreach (char l in lista_esami[count][7].ToCharArray())
                                {
                                    if (':' != l)
                                    {
                                        res = res + l;
                                    }
                                    if (':' == l && contatore == 1)
                                    {
                                        Int32.TryParse(res, out h);
                                        res = "";
                                        contatore++;
                                    }

                                    if (':' == l && contatore == 0)
                                    {
                                        Int32.TryParse(res, out min);
                                        res = "";
                                        contatore++;
                                    }
                                }

                                numericUpDown4.Value = h;
                                numericUpDown3.Value = min;

                                data = getData(count);
                                d = data[0];
                                m = data[1];
                                y = data[2];

                                numericUpDown5.Value = m;
                                numericUpDown6.Value = d;

                                if (lista_esami[count].Length == 10)
                                    textBox2.Text = lista_esami[count][8];
                                else
                                    textBox2.Text = "";

                                modifica_Count = count;
                                label14.Hide();
                            }
                        }
                    }
                    else
                    {
                        w.text("L'esame che si\nvuole modificare\nè già passato");

                        w.Show();
                    }
                }

            }
            else if ((listBox4.SelectedIndex % 2 == 0) && Info_Esame.Items.Count != 0)
            {
                button4.Show();
                modifica = true;
                label15.Text = "Sei in modalità modifica";
                label15.Show();
                int i = -1;
                int count = -1;
                foreach (string[] x in lista_esami)
                {
                    i++;
                    if (x[0].Equals(Info_Esame.Items[0]))
                    {
                        count = i;
                    }
                }

                if (count != -1)
                {
                    comboBox5.Text = lista_esami[count][1].Substring(8);

                    comboBox6.Text = lista_esami[count][5].Substring(11);

                    textBox3.Text = lista_esami[count][6].Substring(7);

                    int[] data = getData(count);
                    int d = data[0];
                    int m = data[1];
                    int y = data[2];

                    numericUpDown2.Value = m;
                    numericUpDown1.Value = d;

                    int h = 0;
                    int min = 0;
                    string res = "";

                    int contatore = 0;
                    res = "";
                    foreach (char l in lista_esami[count][7].ToCharArray())
                    {
                        if (':' != l)
                        {
                            res = res + l;
                        }
                        if (':' == l && contatore == 1)
                        {
                            Int32.TryParse(res, out h);
                            res = "";
                            contatore++;
                        }

                        if (':' == l && contatore == 0)
                        {
                            Int32.TryParse(res, out min);
                            res = "";
                            contatore++;
                        }
                    }

                    numericUpDown4.Value = h;
                    numericUpDown3.Value = min;

                    data = getData(count);
                    d = data[0];
                    m = data[1];
                    y = data[2];

                    numericUpDown5.Value = m;
                    numericUpDown6.Value = d;
                    if (lista_esami[count].Length == 10)
                        textBox2.Text = lista_esami[count][8];
                    else
                        textBox2.Text = "";

                    modifica_Count = count;
                }
            }
            else
            {
                if (Info_Esame.SelectedIndex % 2 != 0)
                    w.text("Non hai selezionato\nnessun esame");
                else
                    w.text("Non hai ancora\ninserito esami");

                w.Show();
            }
        }

        private int[] getData(int count)
        {
            int d = 0;
            int m = 0;
            int y = 0;
            string res = "";

            int contatore = 0;
            res = "";
            foreach (char l in db.lista_esami_totali[count][4].ToCharArray())
            {
                if ('/' != l)
                {
                    res = res + l;
                }
                if ('/' == l && contatore == 1)
                {
                    Int32.TryParse(res, out m);
                    res = "";
                    contatore++;
                }

                if ('/' == l && contatore == 0)
                {
                    Int32.TryParse(res, out d);
                    res = "";
                    contatore++;
                }
            }
            Int32.TryParse(res, out y);
            int[] result = new int[3];
            result[0] = d;
            result[1] = m;
            result[2] = y;

            return result;
        }

        private int[] getData(string date)
        {
            int d = 0;
            int m = 0;
            int y = 0;
            string res = "";

            int contatore = 0;
            res = "";
            foreach (char l in date)
            {
                if ('/' != l)
                {
                    res = res + l;
                }
                if ('/' == l && contatore == 1)
                {
                    Int32.TryParse(res, out m);
                    res = "";
                    contatore++;
                }

                if ('/' == l && contatore == 0)
                {
                    Int32.TryParse(res, out d);
                    res = "";
                    contatore++;
                }
            }
            Int32.TryParse(res, out y);
            int[] result = new int[3];
            result[0] = d;
            result[1] = m;
            result[2] = y;

            return result;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            bool enable = true;
            if (new DateTime(DateTime.Today.Year, (int)numericUpDown2.Value, (int)numericUpDown1.Value).DayOfWeek == DayOfWeek.Saturday || new DateTime(DateTime.Today.Year, (int)numericUpDown2.Value, (int)numericUpDown1.Value).DayOfWeek == DayOfWeek.Sunday)
            {
                w.text("ATTENZIONE!\nIl giorno selezoinato\nè un giorno festivo.\n");
                w.Show();
                enable = false;
            }
            if (textBox3.Text.Equals("") || comboBox6.Text.Equals("") || comboBox5.Text.Equals(""))
            {
                w.text("ATTENZIONE!\nNon sono state inserite\ntutte le informazioni\nfondamentali per l'esame.");
                w.Show();
                enable = false;
            }
            if (enable && unbolding.ToList<DateTime>().IndexOf(new DateTime(DateTime.Today.Year, (int)numericUpDown2.Value, (int)numericUpDown1.Value)) == -1)
            {
                w.text("ATTENZIONE!\nIl giorno selezoinato\nnon è disponibile.\n");
                w.Show();
                enable = false;
            }

            if (enable)
            {
                string id_M;
                List<string> actual_anno_accademico = new List<string>();
                if (modifica)
                {
                    //id_M = db.lista_esami_totali[modifica_Count][db.lista_esami_totali[modifica_Count].Count - 1];

                    //string[] info_M = new string[2];
                    //string[] info_coincidenza = new string[2];
                    //foreach (List<string> y in db.ListMaterie)
                    //{
                    //    if (y[0].Equals(id_M))
                    //    {
                    //        info_M[0] = y[2]; //anno accademico
                    //        info_M[1] = y[3]; //corso di studi
                    //    }
                    //}
                    //List<List<string>> control = db.ListMaterie;

                    //foreach (List<string> x in db.lista_esami_totali)
                    //{
                    //    int error = -1;
                    //    if (!x[0].Equals(db.lista_esami_totali[modifica_Count][0]))
                    //    {//controllo che non sia l'esame che voglio modificare/inserire
                    //        if ((x[4] + " 00:00:00").Equals(new DateTime(DateTime.Now.Year, (int)numericUpDown2.Value, (int)numericUpDown1.Value).ToString()))
                    //        {
                    //            foreach (List<string> y in db.ListMaterie)
                    //            {
                    //                if (y[0].Equals(x[x.Count - 1]))
                    //                {
                    //                    info_coincidenza[0] = y[2]; //anno accademico
                    //                    info_coincidenza[1] = y[3]; //corso di studi
                    //                }
                    //            }
                    //            if (info_M[0].Equals(info_coincidenza[0]) && info_M[1].Equals(info_coincidenza[1]))
                    //            {
                    //                error = 0;
                    //            }
                    //        }
                    //    }
                    //    else if ((x[4] + " 00:00:00").Equals(new DateTime(DateTime.Now.Year, (int)numericUpDown2.Value, (int)numericUpDown1.Value).ToString()))
                    //    {
                    //        error = 1;
                    //    }
                    //    switch (error)
                    //    {
                    //        case 0:
                    //            w.Show();
                    //            w.text("La data inserita\nè già stata occupata");
                    //            return;
                    //        case 1:
                    //            w.Show();
                    //            w.text("La data inserita\nè la stessa di prima");
                    //            return;
                    //    }
                    //}
                    List<string> x = db.lista_esami_totali[modifica_Count];
                    if ((x[4] + " 00:00:00").Equals(new DateTime(DateTime.Now.Year, (int)numericUpDown2.Value, (int)numericUpDown1.Value).ToString()))
                    {
                        w.Show();
                        w.text("La data inserita\nè già stata occupata");
                    }
                    if (unbolding.ToList().IndexOf(new DateTime(DateTime.Now.Year, (int)numericUpDown2.Value, (int)numericUpDown1.Value)) != -1)
                    {

                        List<string> add = new List<string>();

                        //05 / 12 / 2019  10 / 02 / 2020  scritto B2  9:00    6
                        //scritto B2  12 / 2 / 2020   0:9 10 / 2 / 2020   6
                        add.Add(db.lista_esami_totali[modifica_Count][0]);

                        add.Add(comboBox5.Text);

                        add.Add(db.lista_esami_totali[modifica_Count][2]);

                        add.Add(numericUpDown6.Value + "/" + numericUpDown5.Value + "/" + DateTime.Now.Year.ToString());

                        add.Add(numericUpDown1.Value + "/" + numericUpDown2.Value + "/" + DateTime.Now.Year.ToString());

                        add.Add(comboBox6.Text);

                        add.Add(textBox3.Text);

                        add.Add(numericUpDown4.Value + ":" + numericUpDown3.Value);

                        if (!textBox2.Text.Equals(""))
                            add.Add(textBox2.Text);

                        add.Add(db.lista_esami_totali[modifica_Count][db.lista_esami_totali[modifica_Count].Count - 1]);

                        db.modificaEsame(add);
                        lista_esami = db.printExamI(id_utente);
                        modifica = false;
                        listBox4.Items.RemoveAt(listBox4.SelectedIndex + 1);
                        listBox4.Items.RemoveAt(listBox4.SelectedIndex);
                        label15.Text = "";
                        label15.Hide();
                        n.text("Esame modificato con successo");
                        n.Show();
                        label14.Show();
                    }
                    else
                    {
                        w.Show();
                        w.text("La data inserita\nnon è disponibile");
                    }
                }
                else
                {
                    if (unbolding.ToList().IndexOf(new DateTime(DateTime.Now.Year, (int)numericUpDown2.Value, (int)numericUpDown1.Value)) != -1)
                    {

                        List<string> add = new List<string>();

                        add.Add(db.searchIdExam().ToString());

                        add.Add(comboBox5.Text);

                        add.Add(DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());

                        add.Add(numericUpDown6.Value + "/" + numericUpDown5.Value + "/" + DateTime.Now.Year.ToString());

                        add.Add(numericUpDown1.Value + "/" + numericUpDown2.Value + "/" + DateTime.Now.Year.ToString());

                        add.Add(comboBox6.Text);

                        add.Add(textBox3.Text);

                        add.Add(numericUpDown4.Value + ":" + numericUpDown3.Value);

                        if (!textBox2.Text.Equals(""))
                            add.Add(textBox2.Text);

                        add.Add(db.printMaterieI(id_utente)[comboBox5.SelectedIndex]);

                        db.addExam(add);
                        lista_esami = db.printExamI(id_utente);
                        n.text("Esame inserito con successo");
                        n.Show();
                        label14.Show();
                    }
                    else
                    {
                        w.Show();
                        w.text("La data inserita\nnon è disponibile");
                    }
                }
                UpgradeCalendar();
                control_secondo_calendario();
            }
        }


        /// ----- FUNZIONAMENTO TASTO CANCELLA -----

        private void button1_Click_1(object sender, EventArgs e)
        {
            if ((listBox4.SelectedIndex % 2 == 0) && Info_Esame.Items.Count != 0)
            {
                int i = -1;
                int count = -1;

                foreach (string[] x in lista_esami)
                {
                    i++;
                    if (x[0].Equals(Info_Esame.Items[0]))
                    {
                        count = i;
                    }
                }
                List<List<string>> ask1 = db.InfoExamsI(id_utente);
                List<string> ask = db.InfoExamsI(id_utente)[count];
                db.deleteExam(ask);
                listBox4.Items.RemoveAt(listBox4.SelectedIndex + 1);
                listBox4.Items.RemoveAt(listBox4.SelectedIndex);
                Info_Esame.Items.Clear();

                modifica = false;
                label15.Text = "";
                label15.Hide();
                modifica_Count = -1;

                comboBox5.Text = "";

                comboBox6.Text = "";

                textBox3.Text = "";

                numericUpDown2.Value = DateTime.Now.Month;
                numericUpDown1.Value = 1;

                numericUpDown3.Value = 0;
                numericUpDown4.Value = 8;

                numericUpDown5.Value = DateTime.Now.Month;
                numericUpDown6.Value = 1;

                textBox2.Text = "";

                n.text("Esame cancellato con successo");
                UpgradeCalendar();
                n.Show();
            }
            else
            {
                if (Info_Esame.SelectedIndex % 2 != 0)
                    w.text("Non hai selezionato\nnessun esame");
                else
                    w.text("Non hai ancora\ninserito esami");

                w.Show();
            }

        }

        /// ----- AGGIORNAMENTO CALENDARIO -----

        private void UpgradeCalendar()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            Info_Esame.Items.Clear();
            int count = 0;
            lista_esami = db.printExamI(id_utente);
            List<List<string>> exams = db.printExams(id_utente);
            lista_materie = db.printMaterieI(id_utente);

            foreach (List<string> x in exams)
            {
                if (!x[0].Equals(""))
                {
                    listBox1.Items.Add("");
                    count++;
                    listBox1.Items.Add(count.ToString() + " " + x[1]);
                    listBox2.Items.Add("");
                    listBox2.Items.Add(x[4]);

                }
            }

            int d = 0;
            int m = 0;
            int y = 0;
            int i = 0;


            bolding = new DateTime[exams.Count];
            foreach (List<string> c in exams)
            {
                int[] data = getData(c[4]);
                d = data[0];
                m = data[1];
                y = data[2];

                bolding[i] = new DateTime(y, m, d);
                monthCalendar2.BoldedDates = bolding;
                i++;
            }



            /*
            lista_materie = db.printMaterieI(id_utente);
            foreach (List<string> x in db.lista_esami_totali)
            {
                listBox1.Items.Add(x[1]);
                listBox1.Items.Add("");
            }

            int d = 0;
            int m = 0;
            int y = 0;
            string res = "";
            int i = 0;
            bolding = new DateTime[db.lista_esami_totali.Count];
            foreach (List<string> c in db.lista_esami_totali)
            {
                int count = 0;
                res = "";
                foreach (char l in c[4].ToCharArray())
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

                bolding[i] = new DateTime(y, m, d);
                monthCalendar2.BoldedDates = bolding;
                i++;
            }
            unbolding = new DateTime[367];
            int j = 0;
            for (i = 1; i < 32; i++)
            {
                j = 0;
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 1, i)) == -1)
                    unbolding[j + i] = new DateTime(DateTime.Today.Year, 1, i);
                j = j + 31;
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 3, i)) == -1)
                    unbolding[j + i] = new DateTime(DateTime.Today.Year, 3, i);
                j = j + 31;
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 5, i)) == -1)
                    unbolding[j + i] = new DateTime(DateTime.Today.Year, 5, i);
                j = j + 31;
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 7, i)) == -1)
                    unbolding[j + i] = new DateTime(DateTime.Today.Year, 7, i);
                j = j + 31;
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 8, i)) == -1)
                    unbolding[j + i] = new DateTime(DateTime.Today.Year, 8, i);
                j = j + 31;
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 10, i)) == -1)
                    unbolding[j + i] = new DateTime(DateTime.Today.Year, 10, i);
                j = j + 31;
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 12, i)) == -1)
                    unbolding[j + 1] = new DateTime(DateTime.Today.Year, 12, i);
                j = j + 31;
            }
            int tot = j;
            for (i = 1; i < 31; i++)
            {
                j = tot;
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 1, i)) == -1)
                    unbolding[j + i] = new DateTime(DateTime.Today.Year, 6, i);
                j = j + 30;
                unbolding[j + i] = new DateTime(DateTime.Today.Year, 6, i);
                j = j + 30;
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 9, i)) == -1)
                    unbolding[j + i] = new DateTime(DateTime.Today.Year, 9, i);
                j = j + 30;
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 11, i)) == -1)
                    unbolding[j + i] = new DateTime(DateTime.Today.Year, 11, i);
                j = j + 30;
            }
            for (i = 1; i < 30; i++)
            {
                if (bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 2, i)) == -1)
                    unbolding[j + i] = new DateTime(DateTime.Today.Year, 2, i);
            }

            monthCalendar1.BoldedDates = unbolding;
            */
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Hide();
            label14.Show();
            modifica = false;
            label15.Text = "";
            label15.Hide();
            modifica_Count = -1;

            comboBox5.Text = "";

            comboBox6.Text = "";

            textBox3.Text = "";

            numericUpDown2.Value = DateTime.Now.Month;
            numericUpDown1.Value = numericUpDown1.Minimum;

            numericUpDown3.Value = numericUpDown3.Minimum;
            numericUpDown4.Value = numericUpDown4.Minimum;

            numericUpDown5.Value = DateTime.Now.Month;
            numericUpDown6.Value = numericUpDown6.Minimum;

            textBox2.Text = "";
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            switch ((int)numericUpDown5.Value)
            {
                case 1:
                    numericUpDown6.Maximum = 31;
                    break;
                case 2:
                    numericUpDown6.Maximum = 28;
                    break;
                case 3:
                    numericUpDown6.Maximum = 31;
                    break;
                case 4:
                    numericUpDown6.Maximum = 30;
                    break;
                case 5:
                    numericUpDown6.Maximum = 31;
                    break;
                case 6:
                    numericUpDown6.Maximum = 30;
                    break;
                case 7:
                    numericUpDown6.Maximum = 31;
                    break;
                case 8:
                    numericUpDown6.Maximum = 31;
                    break;
                case 9:
                    numericUpDown6.Maximum = 30;
                    break;
                case 10:
                    numericUpDown6.Maximum = 31;
                    break;
                case 11:
                    numericUpDown6.Maximum = 30;
                    break;
                case 12:
                    numericUpDown6.Maximum = 31;
                    break;
            }
        }

        private void numericUpDown2_ValueChanged_1(object sender, EventArgs e)
        {
            switch ((int)numericUpDown2.Value)
            {
                case 1:
                    numericUpDown1.Maximum = 31;
                    break;
                case 2:
                    numericUpDown1.Maximum = 28;
                    break;
                case 3:
                    numericUpDown1.Maximum = 31;
                    break;
                case 4:
                    numericUpDown1.Maximum = 30;
                    break;
                case 5:
                    numericUpDown1.Maximum = 31;
                    break;
                case 6:
                    numericUpDown1.Maximum = 30;
                    break;
                case 7:
                    numericUpDown1.Maximum = 31;
                    break;
                case 8:
                    numericUpDown1.Maximum = 31;
                    break;
                case 9:
                    numericUpDown1.Maximum = 30;
                    break;
                case 10:
                    numericUpDown1.Maximum = 31;
                    break;
                case 11:
                    numericUpDown1.Maximum = 30;
                    break;
                case 12:
                    numericUpDown1.Maximum = 31;
                    break;
            }
            numericUpDown5.Value = numericUpDown2.Value;
        }

        private void control_secondo_calendario()
        {
            string anno = "-1";
            string corso = "";
            string id_materia = "";
            int j = 0;
            int i = 0;

            foreach (List<String> x in db.ListMaterie)
            {
                if (comboBox5.Text.Equals(x[1]))
                {
                    // anno academico = x[3]
                    // tipo corso di studi = x [4]
                    anno = x[2];
                    corso = x[3];
                    id_materia = x[0];
                }
            }
            if (!anno.Equals("-1"))
            {
                unbolding = new DateTime[367];
                int count = 0;
                bolding = new DateTime[db.lista_esami_totali.Count];
                List<DateTime> esamiStessoAnno = new List<DateTime>();
                for (i = 0; i < db.lista_esami_totali.Count; i++)
                {
                    for (j = 0; j < db.ListMaterie.Count; j++)
                    {
                        if (db.lista_esami_totali[i][db.lista_esami_totali[i].Count - 1].Equals(db.ListMaterie[j][0]) && db.ListMaterie[j][2].Equals(anno) && db.ListMaterie[j][3].Equals(corso))
                        {
                            count++;
                            List<string> c = db.lista_esami_totali[i];
                            int d = 0;
                            int m = 0;
                            int y = 0;
                            string res = "";

                            count = 0;
                            res = "";
                            foreach (char l in c[4].ToCharArray())
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

                            bolding[i] = new DateTime(y, m, d);
                        }
                    }
                }

                //for (i = 0; i < db.lista_esami_totali.Count - 1; i++)
                //{
                //    for (j = 0; j < db.ListMaterie.Count; j++)
                //    {
                //        if (db.lista_esami_totali[i][db.lista_esami_totali[i].Count - 1].Equals(db.ListMaterie[j][0]) && db.ListMaterie[j][2].Equals(anno) && db.ListMaterie[j][3].Equals(corso))
                //        {
                //            List<string> c = db.lista_esami_totali[i];
                //            int d = 0;
                //            int m = 0;
                //            int y = 0;
                //            string res = "";

                //            count = 0;
                //            res = "";
                //            foreach (char l in c[4].ToCharArray())
                //            {
                //                if ('/' != l)
                //                {
                //                    res = res + l;
                //                }
                //                if ('/' == l && count == 1)
                //                {
                //                    Int32.TryParse(res, out m);
                //                    res = "";
                //                    count++;
                //                }

                //                if ('/' == l && count == 0)
                //                {
                //                    Int32.TryParse(res, out d);
                //                    res = "";
                //                    count++;
                //                }
                //            }
                //            Int32.TryParse(res, out y);

                //            bolding[i] = new DateTime(y, m, d);

                //        }
                //    }
                //}
                for (i = 1; i < 32; i++)
                {
                    j = -1;
                    if ((DateTime.Now.Month < 1 || (DateTime.Now.Month == 1 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 1, i)) == -1 && new DateTime(DateTime.Today.Year, 1, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 1, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 1, i);
                    j = j + 31;
                    if ((DateTime.Now.Month < 3 || (DateTime.Now.Month == 3 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 3, i)) == -1 && new DateTime(DateTime.Today.Year, 3, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 3, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 3, i);
                    j = j + 31;
                    if ((DateTime.Now.Month < 5 || (DateTime.Now.Month == 5 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 5, i)) == -1 && new DateTime(DateTime.Today.Year, 5, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 5, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 5, i);
                    j = j + 31;
                    if ((DateTime.Now.Month < 7 || (DateTime.Now.Month == 7 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 7, i)) == -1 && new DateTime(DateTime.Today.Year, 7, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 7, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 7, i);
                    j = j + 31;
                    if ((DateTime.Now.Month < 8 || (DateTime.Now.Month == 8 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 8, i)) == -1 && new DateTime(DateTime.Today.Year, 8, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 8, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 8, i);
                    j = j + 31;
                    if ((DateTime.Now.Month < 10 || (DateTime.Now.Month == 10 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 10, i)) == -1 && new DateTime(DateTime.Today.Year, 10, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 10, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 10, i);
                    j = j + 31;
                    if ((DateTime.Now.Month < 12 || (DateTime.Now.Month == 12 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 12, i)) == -1 && new DateTime(DateTime.Today.Year, 12, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 12, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 12, i);
                    j = j + 31;
                }
                int tot = j + 1;
                for (i = 1; i < 31; i++)
                {
                    j = tot;
                    if ((DateTime.Now.Month < 4 || (DateTime.Now.Month == 4 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 4, i)) == -1 && new DateTime(DateTime.Today.Year, 1, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 4, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 4, i);
                    j = j + 30;
                    if ((DateTime.Now.Month < 6 || (DateTime.Now.Month == 6 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 6, i)) == -1 && new DateTime(DateTime.Today.Year, 6, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 6, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 6, i);
                    j = j + 30;
                    if ((DateTime.Now.Month < 9 || (DateTime.Now.Month == 9 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 9, i)) == -1 && new DateTime(DateTime.Today.Year, 9, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 9, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 9, i);
                    j = j + 30;
                    if ((DateTime.Now.Month < 11 || (DateTime.Now.Month == 11 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 11, i)) == -1 && new DateTime(DateTime.Today.Year, 11, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 11, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 11, i);
                    j = j + 30;
                }
                for (i = 1; i < 30; i++)
                {
                    if ((DateTime.Now.Month < 2 || (DateTime.Now.Month == 2 && DateTime.Now.Day < i)) && bolding.ToList().IndexOf(new DateTime(DateTime.Today.Year, 2, i)) == -1 && new DateTime(DateTime.Today.Year, 2, i).DayOfWeek != DayOfWeek.Saturday && new DateTime(DateTime.Today.Year, 2, i).DayOfWeek != DayOfWeek.Sunday)
                        unbolding[j + i] = new DateTime(DateTime.Today.Year, 2, i);
                }

                monthCalendar1.BoldedDates = unbolding;
                label14.Hide();
            }
        }

        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            control_secondo_calendario();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown6.Value = numericUpDown1.Value;
        }



        private void ControlloStudentiPrenotati(string id_esame)
        {

            List<List<string>> gestionePrenotazioni = db.StudentiPrenotati(id_utente);
            foreach (List<string> x in gestionePrenotazioni)
            {
                if (x[0].Equals(id_esame))
                {
                    for (int i = 1; i < x.Count; i++)
                    {
                        Lista_Studenti_P.Items.Add((Lista_Studenti_P.Items.Count + 1).ToString() + " " + x[i]);
                    }
                }
            }
        }

        private void listBox2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                listBox4.Items.Clear();
                listBox1.SelectedIndex = listBox2.SelectedIndex;
                Info_Esame.Items.Clear();
                Lista_Studenti_P.Items.Clear();

                object oggetto = listBox1.SelectedItem;

                string x = oggetto.ToString();

                if (x != null)
                {
                    if (!x.Equals(""))
                    {
                        char[] stringa = x.ToCharArray();

                        int count;
                        if ((float)listBox1.SelectedIndex / 2 > 9)
                        {
                            Int32.TryParse(stringa[0].ToString() + stringa[1].ToString(), out count);

                            ControlloStudentiPrenotati(db.printExams(id_utente)[count - 1][0]);

                            foreach (string stampa in lista_esami[count - 1])
                            {

                                Info_Esame.Items.Add(stampa);
                            }
                        }
                        else
                        {
                            Int32.TryParse(stringa[0].ToString(), out count);

                            ControlloStudentiPrenotati(db.printExams(id_utente)[count - 1][0]);

                            foreach (string stampa in lista_esami[count - 1])
                            {

                                Info_Esame.Items.Add(stampa);
                            }
                        }
                    }
                }
            }

        }
    }
}