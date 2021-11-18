using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progetto_Prenotazione_esami
{
    public class Esame
    {
        private DateTime data_generazione;
        private string nome;
        private int codice;
        private DateTime data;
        private DateTime data_scadenza_prenotazione;
        private string tipo;
        private string aula;
        private string ora;
        private string note;
        private string id_insegnante;

        public Esame() { }

        public Esame( string nome, int codice, DateTime data, DateTime data_scadenza_prenotazione, string tipo, string aula, string ora, string note, string id_insegnante)
        {
            this.nome = nome;
            this.codice = codice;
            this.data = data;
            this.data_scadenza_prenotazione = data_scadenza_prenotazione;
            this.tipo = tipo;
            this.aula = aula;
            this.ora = ora;
            this.note = note;
            this.data_generazione = DateTime.Now;
            this.id_insegnante = id_insegnante;
            addToDB();
        }

        public override string ToString()
        {
            // result= id + nome + D creazione + D scadenza + D Esame + Tipo + Aula + Ora + note
            string result = codice.ToString() + "/t";
            result = nome + "\t";
            result = result + DateTime.Today.Day.ToString() + "/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString() + "\t";
            result = result + data_scadenza_prenotazione.Day.ToString() + "/" + data_scadenza_prenotazione.Month.ToString() + "/" + data_scadenza_prenotazione.Year.ToString() + "\t";
            result = result + data.Day.ToString() + "/" + data.Month.ToString() + "/" + data.Year.ToString() + "\t";
            result = result + tipo + "\t";
            result = result + aula + "\t";
            result = result + ora + "\t";
            result = result + id_insegnante + "\t";
            if(!note.Equals(""))
                result = result + note + "\t";
            result = result + "\n";
            
            return result;
        }

        public void addToDB()
        {
            string path = "..//..//..//ESAMI.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter outputFile = File.CreateText(path))
                {
                    string x = ToString();
                    
                    outputFile.WriteLine(x);
                }

            }
            else
            {
                string[] y = File.ReadAllLines(path);
                using (StreamWriter outputFile = new StreamWriter(path))
                {
                    string x = ToString();
                    foreach (string line in y)
                        outputFile.WriteLine(line);
                    
                    outputFile.WriteLine(x);
                }
            }
        }
    }
}
