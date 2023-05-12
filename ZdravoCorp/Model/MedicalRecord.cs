using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ZdravoCorp.Enums;
using ZdravoCorp.Repository;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class MedicalRecord : ISerializable
    {
        public int Id { get; set; }
        double height, weight;
        public User Patient { get; set; }
        List<Anamnesis> anamneses; 
        List<String> allergies;
        List<String> diseases;

        public double Weight
        { get { return weight; } set { weight = value; } }

        public double Height { get { return height; } set {  height = value; } }

        public List<String> Allergies
        {
            get { return allergies; }
            set { allergies = value; }
        }

        public List<String> Diseases
        {
            get { return diseases; }
            set { diseases = value; }
        }

        public List<Anamnesis> Anamneses
        {
            get { return anamneses; }
            set
            {
                anamneses = value;
            }
        }


        public MedicalRecord()
        {
        }

        public MedicalRecord(int id, List<Anamnesis> anamneses, List<string> allergies, List<string> diseases, double weight, double height)
        {
            Id = id;
            this.height = height;
            this.weight = weight;
            this.anamneses = anamneses;
            this.allergies = allergies;
            this.diseases = diseases;

        }

        public override string ToString()
        {
            return "MedicalRecord [" +
                   "Id: "+Id.ToString()+"\n" +
                   "Height: "+height+ "\n" +
                   "Weight: " +weight+ "\n" +
                   "Anamneses: " +anamneses+ "\n" +
                   "Allergies: " +allergies+ "\n" +
                   "Diseases: " +diseases
                   +"]";
        }


        public string[] ToCSV()
        {
            string anamnesesCSV = string.Join(",", Anamneses.Select(a => a.Report));
            string allergiesCSV = string.Join(",", Allergies);
            string diseasesCSV = string.Join(",", Diseases);
            string[] csvValues =
            {
                Id.ToString(),
                height.ToString(),
                weight.ToString(),
                anamnesesCSV,
                allergiesCSV,
                diseasesCSV,
                Patient.Id.ToString(),
            };
            return csvValues;
        }



        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            height = double.Parse(values[1]);
            weight = double.Parse(values[2]);
            string anamnesesString = values[3];


            Anamnesis anamnesis=new Anamnesis(anamnesesString);
            List<Anamnesis> listOfAnamneses= new List<Anamnesis>();
            listOfAnamneses.Add(anamnesis);
            Anamneses=listOfAnamneses;


            string allergieString = values[4];

            List<String> allergieList = new List<String>();
            foreach (String allergie in allergieString.Split(','))
            {
                allergieList.Add(allergie);
            }

            Allergies=allergieList;

            string diseaseString = values[5];
            List<String> diseaseList = new List<String>();
            foreach (String disease in diseaseString.Split(','))
            {
                diseaseList.Add(disease);
            }

            Diseases=diseaseList;

            int patientId = int.Parse(values[6]);
            UserRepository repo = new UserRepository();
            repo.Load();
            User patient = repo.GetById(patientId);


            Patient = patient;
        }
    }
}
