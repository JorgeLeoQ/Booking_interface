using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Progetto_Prenotazione_esami
{
    public class DataBase
    {
        private List<string> path = new List<string>();

        private List<List<string>> lista_INSEGNATI = new List<List<string>>();
        private List<List<string>> lista_STUDENTI = new List<List<string>>();
        private List<List<string>> lista_MATERIE = new List<List<string>>();
        private List<List<string>> lista_ESAMI = new List<List<string>>();
        private List<List<string>> lista_PRENOTAZIONI = new List<List<string>>();

        public DataBase()
        {
            path.Add("..//..//..//INSEGNANTI.txt");
            path.Add("..//..//..//STUDENTI.txt");
            path.Add("..//..//..//MATERIE.txt");
            path.Add("..//..//..//ESAMI.txt");
            path.Add("..//..//..//PRENOTAZIONI.txt");

            lista_INSEGNATI = Read_From_File(path[0]);
            lista_STUDENTI = Read_From_File(path[1]);
            lista_MATERIE = Read_From_File(path[2]);
            lista_ESAMI = Read_From_File(path[3]);
            lista_PRENOTAZIONI = Read_From_File(path[4]);
        }

        private List<List<string>> Read_From_File(string path)
        {
            List<List<string>> ret = new List<List<string>>();
            string[] contenuto_DB;

            contenuto_DB = File.ReadAllLines(path);
            int i = 0;

            foreach (string p in contenuto_DB)
            {
                string x = p;
                x = x + "\n";
                ret.Add(new List<string>());
                string result = "";
                foreach (char u in x.ToCharArray())
                {
                    if (u != '\t' && u != '\n')
                    {
                        result = result + u;
                    }
                    else
                    {
                        ret[i].Add(result);
                        result = "";
                    }
                }
                i++;

                //int.TryParse(x, out id);
                //lista_ID.Add(id);
            }
            return ret;
        }

        public int Login_Control(string n_matricola, string password) // -1 HAI sbagliato ----- 0 Insegnante ------- 1 Studente
        {
            foreach (List<String> x in lista_INSEGNATI)
            {
                if (x[0].Equals(n_matricola) && x[3].Equals(password))
                {
                    return 0;
                }
            }
            foreach (List<String> x in lista_STUDENTI)
            {
                    if (x[0].Equals(n_matricola) && x[3].Equals(password))
                {
                    return 1;
                }
            }
            return -1;
        }

        public List<string> idEsamiPrenotati(string id)
        {
            List<string> result = new List<string>();
            foreach(List<string> x in lista_STUDENTI)
            {
                if (x[0].Equals(id))
                {
                    if (x.Count == 6) //NON mi sono ancora iscritto ad esami
                        return result;
                    else
                    {
                        for(int i = 6; i<x.Count; i++)
                        {
                            result.Add(x[i]);
                        }
                    }
                }
            }
            return result;
        }

        public void registrazione_Esame(string id, string id_esame)
        {
            string[] contenuto_DB;

            contenuto_DB = File.ReadAllLines(path[3]);

            int count = -1;
            bool find = false;
            foreach (List<string> x in lista_STUDENTI)
            {
                if (!x[0].Equals(id) && !find)
                {
                    count++;
                }
                else
                {
                    if (!find)
                        count++;
                    find = true;
                }
            }

            
            lista_STUDENTI[count].Add(id_esame);

            //abbiamo rimosso la vecchia versione ora aggiorniamo il db
            //string[] new_contenuto_DB = new string[contenuto_DB.Length];
            string[] new_contenuto_DB = new string[lista_STUDENTI.Count];
            for (int i = 0; i < lista_STUDENTI.Count; i++)
            {
                //contenuto_DB[i] = "";
                for (int j = 0; j < lista_STUDENTI[i].Count; j++)
                {
                    new_contenuto_DB[i] = new_contenuto_DB[i] + lista_STUDENTI[i][j];
                    if (j != lista_STUDENTI[i].Count - 1)
                    {
                        new_contenuto_DB[i] = new_contenuto_DB[i] + "\t";
                    }
                }
            }

            File.Delete(path[1]);
            File.Create(path[1]).Close();
            File.WriteAllLines(path[1], new_contenuto_DB);
        }

        public void disdetta_esame(string id, string id_esame)
        {
            string[] contenuto_DB;

            contenuto_DB = File.ReadAllLines(path[3]);

            int count = -1;
            bool find = false;
            foreach (List<string> x in lista_STUDENTI)
            {
                if (!x[0].Equals(id) && !find)
                {
                    count++;
                }
                else
                {
                    if (!find)
                        count++;
                    find = true;
                }
            }

            List<string> ret = new List<string>(lista_STUDENTI[count]);
            ret.RemoveAt(0);
            ret.RemoveAt(4);
            lista_STUDENTI[count].RemoveAt(ret.IndexOf(id_esame)+2);
            
            //abbiamo rimosso la vecchia versione ora aggiorniamo il db
            string[] new_contenuto_DB = new string[lista_STUDENTI.Count];
            for (int i = 0; i < lista_STUDENTI.Count; i++)
            {
                //contenuto_DB[i] = "";
                for (int j = 0; j < lista_STUDENTI[i].Count; j++)
                {
                    new_contenuto_DB[i] = new_contenuto_DB[i] + lista_STUDENTI[i][j];
                    if (j != lista_STUDENTI[i].Count - 1)
                    {
                        new_contenuto_DB[i] = new_contenuto_DB[i] + "\t";
                    }
                }
            }

            File.Delete(path[1]);
            File.Create(path[1]).Close();
            File.WriteAllLines(path[1], new_contenuto_DB);
        }

        public List<string> ReturnMaterie(string n_matriola, int IoS) // IoS = Insegnante o studente
        {
            List<string> materie = new List<string>();

            //Materie insegnanti
            if (IoS == 0)
            {
                foreach (List<string> x in lista_INSEGNATI)
                {
                    if (n_matriola.Equals(x[0]))
                    {
                        for (int i = 4; i < x.Count; i++)
                        {
                            materie.Add(x[i]);
                        }
                    }
                }
            }

            //Materie studenti

            if (IoS == 1)
            {
                List<string> result = new List<string>();
                List<String> infoCorsoStudi = new List<string>();
                foreach (List<string> x in lista_STUDENTI)
                {
                    if (n_matriola.Equals(x[0]))
                    {
                        infoCorsoStudi.Add(x[4]);
                        infoCorsoStudi.Add(x[5]);
                    }
                }
                foreach(List<string> m in lista_MATERIE)
                {
                    if(m[2].Equals(infoCorsoStudi[0]) && m[3].Equals(infoCorsoStudi[1]))
                    {
                        result.Add(m[0]);
                    }
                }
                return result;
            }

            return materie;
        }

        /// <summary>
        /// //stampa anche le info
        /// </summary>
        /// <returns></returns>
        public string printExam() 
        {
            string result = "";
            foreach (List<string> x in lista_ESAMI)
            {
                result = result + "Codice:\t\t" + x[0] + "\n";
                result = "Esame:\t\t" + x[1] + "\n";
                result = result + "Creato il:\t\t" + x[2] + "\n";
                result = result + "Scadenza:\t" + x[3] + "\n";
                result = result + "Data appello:\t" + x[4] + "\n";
                result = result + "Tipologia:\t" + x[5] + "\n";
                result = result + "Aula:\t\t" + x[6] + "\n";
                result = result + "Ora:\t\t" + x[7] + "\n";
                if (x.Count == 11)
                {
                    result = result + x[8] + "\n\n";
                    result = result + x[9] + "\n\n";
                    result = result + x[10] + "\n\n";
                }
                else
                {
                    result = result + x[9] + "\n\n";
                    result = result + x[10] + "\n\n";
                }
            }
            return result;
        }


        /// <summary>
        /// stampa tutti gli esami di una materia
        /// </summary>
        /// <param name="id_materia"></param>
        /// <returns></returns>
        public string printExam(string id_materia)
        {
            string result = "";
            foreach (List<string> x in lista_ESAMI)
            {
                if (x[0].Equals(id_materia))
                {
                    result = result + "Codice:\t\t" + x[0] + "\n";
                    result = "Esame:\t\t" + x[1] + "\n";
                    result = result + "Creato il:\t\t" + x[2] + "\n";
                    result = result + "Scadenza:\t" + x[3] + "\n";
                    result = result + "Data appello:\t" + x[4] + "\n";
                    result = result + "Tipologia:\t" + x[5] + "\n";
                    result = result + "Aula:\t\t" + x[6] + "\n";
                    result = result + "Ora:\t\t" + x[7] + "\n";
                    if (x.Count == 9)
                        result = result + x[8] + "\n";
                    result = result + "\n";
                }
            }
            return result;
        }

        /// <summary>
        /// stampa le info riguardanti gli esami inseriti da un insegnante
        /// </summary>
        /// <param name="id_Insegnante"></param>
        /// <returns></returns>
        public List<string[]> printExamI(string id_Insegnante)
        {
            List<string> materia_id = new List<string>();

            foreach (List<string> y in lista_INSEGNATI)
            {
                if (y[0].Equals(id_Insegnante))
                {
                    for (int j = 4; j < y.Count; j++)
                    {
                        materia_id.Add(y[j]);
                    }
                }
            }

            List<string[]> result = new List<string[]>();
            int i = 0;
            foreach (List<string> x in lista_ESAMI)
            {
                foreach (string id in materia_id)
                {
                    if (x[x.Count-1].Equals(id) && !x[0].Equals(""))
                    {
                        result.Add(new string[x.Count]);
                        result[i][0] = "Codice:\t\t" + x[0] + "\n";
                        result[i][1] = "Esame:\t\t" + x[1] + "\n";
                        result[i][2] = "Creato il:\t\t" + x[2] + "\n";
                        result[i][3] = "Scadenza:\t" + x[3] + "\n";
                        result[i][4] = "Data appello:\t" + x[4] + "\n";
                        result[i][5] = "Tipologia:\t" + x[5] + "\n";
                        result[i][6] = "Aula:\t\t" + x[6] + "\n";
                        result[i][7] = "Ora:\t\t" + x[7] + "\n";
                        if (x.Count == 10)
                        {
                            result[i][8] = "Note:\t\t" + x[8] + "\n";
                            result[i][9] = "Codice corso:\t" + x[9] + "\n";
                        }
                        else
                        {
                            result[i][8] = "Codice corso:\t" + x[8] + "\n";
                        }
                        i++;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// restituisce una lista contenente i dati della lista di esami inserita da un docente
        /// </summary>
        /// <param name="id_Insegnante"></param>
        /// <returns></returns>
        public List<List<string>> printExams(string id_Insegnante)
        {
            List<string> materia_id = new List<string>();

            foreach (List<string> y in lista_INSEGNATI)
            {
                if (y[0].Equals(id_Insegnante))
                {
                    for (int j = 4; j < y.Count; j++)
                    {
                        materia_id.Add(y[j]);
                    }
                }
            }

            List<List<string>> result = new List<List<String>>();
            int i = 0;
            foreach (List<string> x in lista_ESAMI)
            {
                foreach (string id in materia_id)
                {
                    if (x[x.Count-1].Equals(id))
                    {
                        result.Add(new List<string>());
                        result[i] = x;
                        i++;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// stampa tutti gli id delle materie insegnante da un docente
        /// </summary>
        /// <param name="id_Insegnante"></param>
        /// <returns></returns>
        public List<string> printMaterieI(string id_Insegnante)
        {
            List<string> materia_id = new List<string>();

            foreach (List<string> y in lista_INSEGNATI)
            {
                if (y[0].Equals(id_Insegnante))
                {
                    for (int j = 4; j < y.Count; j++)
                    {
                        materia_id.Add(y[j]);
                    }
                }
            }
            return materia_id;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_Insegnante"></param>
        /// <returns></returns>
        public List<string> printMateries(string id_Insegnante)
        {
            List<string> materia_id = new List<string>();

            foreach (List<string> y in lista_INSEGNATI)
            {
                if (y[0].Equals(id_Insegnante))
                {
                    for (int j = 4; j < y.Count; j++)
                    {
                        materia_id.Add(y[j]);
                    }
                }
            }
            List<string> result = new List<string>();
            foreach(List<string> y in lista_MATERIE)
            {
                for(int i = 0; i<materia_id.Count; i++)
                {
                    if (y[0].Equals(materia_id[i]))
                    {
                        result.Add(y[1]);
                    }
                }
            }
            return result;
        }

        public List<List<string>> lista_esami_totali
        {
            get { return lista_ESAMI; }
        }

        public List<List<string>> ListMaterie
        {
            get { return lista_MATERIE; }
        }

        public List<List<string>> Lista_PRENOTAZIONI
        {
            get
            {
                return lista_PRENOTAZIONI;
            }
        }

        public void addExam(List<string> infoExam)
        {
            List<List<string>> ret = new List<List<string>>();
            string[] contenuto_DB;

            contenuto_DB = File.ReadAllLines(path[3]);
            lista_ESAMI.Add(infoExam);

            string[] new_contenuto_DB = new string[contenuto_DB.Length + 1];
            for (int i = 0; i < contenuto_DB.Length; i++)
            {
                new_contenuto_DB[i] = contenuto_DB[i];
            }

            new_contenuto_DB[new_contenuto_DB.Length - 1] = "";

            for (int i = 0; i < infoExam.Count; i++)
            {
                new_contenuto_DB[new_contenuto_DB.Length - 1] = new_contenuto_DB[new_contenuto_DB.Length - 1] + infoExam[i];
                if (i != infoExam.Count - 1)
                {
                    new_contenuto_DB[new_contenuto_DB.Length - 1] = new_contenuto_DB[new_contenuto_DB.Length - 1] + "\t";
                }
            }
            File.Delete(path[3]);
            File.Create(path[3]).Close();
            File.WriteAllLines(path[3], new_contenuto_DB);

            contenuto_DB = File.ReadAllLines(path[4]);
            new_contenuto_DB = new string[contenuto_DB.Length + 1];

            new_contenuto_DB[new_contenuto_DB.Length - 1] = infoExam[0];

            File.Delete(path[4]);
            File.Create(path[4]).Close();
            File.WriteAllLines(path[4], new_contenuto_DB);
        }

        public void deleteExam(List<string> infoExam)
        {
            List<List<string>> ret = new List<List<string>>();
            string[] contenuto_DB;

            contenuto_DB = File.ReadAllLines(path[3]);

            int count = 0;
            bool find = false;
            foreach (List<string> x in lista_ESAMI)
            {
                if (!x[0].Equals(infoExam[0]) && !find)
                {
                    count++;
                }
                else
                {
                    find = true;
                }
            }

            lista_ESAMI.RemoveAt(count);

            string[] new_contenuto_DB = new string[contenuto_DB.Length - 1];


            for (int i = 0; i < lista_ESAMI.Count; i++)
            {
                contenuto_DB[i] = "";
                for (int j = 0; j < lista_ESAMI[i].Count; j++)
                {
                    new_contenuto_DB[i] = new_contenuto_DB[i] + lista_ESAMI[i][j];
                    if (j != lista_ESAMI[i].Count - 1)
                    {
                        new_contenuto_DB[i] = new_contenuto_DB[i] + "\t";
                    }
                }
            }

            File.Delete(path[3]);
            File.Create(path[3]).Close();
            File.WriteAllLines(path[3], new_contenuto_DB);

            contenuto_DB = File.ReadAllLines(path[4]);

            count = -1;
            find = false;
            foreach (List<string> x in lista_PRENOTAZIONI)
            {
                if (!x[0].Equals(infoExam[0]) && !find)
                {
                    count++;
                }
                else
                {
                    find = true;
                }
            }
            lista_PRENOTAZIONI.RemoveAt(count);
            new_contenuto_DB = new string[contenuto_DB.Length - 1];

            for (int i = 0; i < lista_PRENOTAZIONI.Count; i++)
            {
                new_contenuto_DB[i] = "";
                for (int j = 0; j < lista_PRENOTAZIONI[i].Count; j++)
                {
                    new_contenuto_DB[i] = new_contenuto_DB[i] + lista_PRENOTAZIONI[i][j];
                }
            }

            File.Delete(path[4]);
            File.Create(path[4]).Close();
            File.WriteAllLines(path[4], new_contenuto_DB);

        }

        public void modificaEsame(List<string> infoExam)
        {
            List<List<string>> ret = new List<List<string>>();
            string[] contenuto_DB;

            contenuto_DB = File.ReadAllLines(path[3]);

            int count = -1;
            bool find = false;
            foreach (List<string> x in lista_ESAMI)
            {
                if (!x[0].Equals(infoExam[0]) && !find)
                {
                    count++;
                }
                else
                {
                    if(!find)
                        count++;
                    find = true;
                }
            }

            lista_ESAMI.RemoveAt(count);
            lista_ESAMI.Add(infoExam);

            //abbiamo rimosso la vecchia versione ora aggiorniamo il db
            string[] new_contenuto_DB = new string[contenuto_DB.Length];
            for (int i = 0; i < lista_ESAMI.Count; i++)
            {
                contenuto_DB[i] = "";
                for (int j = 0; j < lista_ESAMI[i].Count; j++)
                {
                    new_contenuto_DB[i] = new_contenuto_DB[i] + lista_ESAMI[i][j];
                    if (j != lista_ESAMI[i].Count - 1)
                    {
                        new_contenuto_DB[i] = new_contenuto_DB[i] + "\t";
                    }
                }
            }

            File.Delete(path[3]);
            File.Create(path[3]).Close();
            File.WriteAllLines(path[3], new_contenuto_DB);
        }

        public List<string> FindExam(string code)
        {
            bool find = false;
            int count = -1;

            foreach (List<string> x in lista_ESAMI)
            {
                if (!x[0].Equals(code) && !find)
                {
                    count++;
                }
                else
                {
                    find = true;
                }
            }
            return lista_ESAMI[count];
        }

        public List<List<string>> InfoExamsI(string id_Insegnante)
        {
            List<string> materia_id = new List<string>();

            foreach (string y in ReturnMaterie(id_Insegnante, 0))
            {
                materia_id.Add(y);
                
            }

            List<List<string>> result = new List<List<string>>();
            int i = 0;
            foreach (List<string> x in lista_ESAMI)
            {
                foreach (string id in materia_id)
                {
                    if (x[x.Count-1].Equals(id))
                    {
                        result.Add(new List<string>());
                        result[i].Add(x[0]);
                        result[i].Add(x[1]);
                        result[i].Add(x[2]);
                        result[i].Add(x[3]);
                        result[i].Add(x[4]);
                        result[i].Add(x[5]);
                        result[i].Add(x[6]);
                        result[i].Add(x[7]);
                        if (x.Count == 10)
                        {
                            result[i].Add(x[8]);
                            result[i].Add(x[9]);
                        }
                        else
                        {
                            result[i].Add(x[8]);
                        }
                        i++;
                    }
                }
            }
            return result;
        }

        public int searchIdExam()
        {
            int c1, c2;
            c2 = -1;
            foreach(List<string> x in lista_ESAMI)
            {
                Int32.TryParse(x[0], out c1);
                if (c1 > c2)
                {
                    c2 = c1;
                }
            }
            return c2 + 1;
        }

        public List<List<string>> StudentiPrenotati(string id_insegnante)
        {
            List<List<string>> result = new List<List<string>>();
            List<string> add = new List<string>();
            foreach (List<string> x in lista_STUDENTI)
            {
                if (x.Count > 6) //Ho prenotato almeno un esame
                    for (int i = 6; i < x.Count; i++) {
                        for (int j = 0; j < printExams(id_insegnante).Count; j++)
                            if (x[i].Equals(printExams(id_insegnante)[j][0]))
                            {                                
                                add.Add(printExams(id_insegnante)[j][0]);
                                add.Add(x[1] + " " + x[2]);
                                result.Add(add);
                                add = new List<string>();
                            }
                    }
            }

            return result;
        }
    }
}