using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareSystem
{
    // (a) Generic Repository
    public class Repository<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return items;
        }

        public T GetById(Func<T, bool> condition)
        {
            return items.FirstOrDefault(condition);
        }

        public void Remove(Func<T, bool> condition)
        {
            T item = GetById(condition);

            if (item != null)
            {
                items.Remove(item);
            }
        }
    }

    // (b) Patient
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }
    }

    // (c) Prescription
    public class Prescription
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string MedicationName { get; set; }
        public DateTime DateIssued { get; set; }

        public Prescription(
            int id,
            int patientId,
            string medicationName,
            DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }
    }

    // (d), (e), (f), (g)
    public class HealthSystemApp
    {
        private Repository<Patient> patientRepository =
            new Repository<Patient>();

        private Repository<Prescription> prescriptionRepository =
            new Repository<Prescription>();

        private Dictionary<int, List<Prescription>> prescriptionMap =
            new Dictionary<int, List<Prescription>>();

        // (e) Seed data
        public void SeedData()
        {
            patientRepository.Add(
                new Patient(1, "Ama Mensah", 25, "Female"));

            patientRepository.Add(
                new Patient(2, "Kwame Asante", 32, "Male"));

            patientRepository.Add(
                new Patient(3, "Abena Owusu", 28, "Female"));

            prescriptionRepository.Add(
                new Prescription(
                    101,
                    1,
                    "Paracetamol",
                    DateTime.Now));

            prescriptionRepository.Add(
                new Prescription(
                    102,
                    1,
                    "Amoxicillin",
                    DateTime.Now));

            prescriptionRepository.Add(
                new Prescription(
                    103,
                    2,
                    "Ibuprofen",
                    DateTime.Now));

            prescriptionRepository.Add(
                new Prescription(
                    104,
                    3,
                    "Vitamin C",
                    DateTime.Now));

            prescriptionRepository.Add(
                new Prescription(
                    105,
                    2,
                    "Antibiotics",
                    DateTime.Now));
        }

        // Build prescription map
        public void BuildPrescriptionMap()
        {
            foreach (Prescription prescription in
                     prescriptionRepository.GetAll())
            {
                if (!prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    prescriptionMap[prescription.PatientId] =
                        new List<Prescription>();
                }

                prescriptionMap[prescription.PatientId].Add(
                    prescription);
            }
        }

        // Print all patients
        public void PrintAllPatients()
        {
            Console.WriteLine("===== ALL PATIENTS =====");

            foreach (Patient patient in patientRepository.GetAll())
            {
                Console.WriteLine(
                    $"ID: {patient.Id} | " +
                    $"Name: {patient.Name} | " +
                    $"Age: {patient.Age} | " +
                    $"Gender: {patient.Gender}"
                );
            }
        }

        // (f) Get prescriptions by patient ID
        public List<Prescription> GetPrescriptionsByPatientId(
            int patientId)
        {
            if (prescriptionMap.ContainsKey(patientId))
            {
                return prescriptionMap[patientId];
            }

            return new List<Prescription>();
        }

        // Print prescriptions for a patient
        public void PrintPrescriptionsForPatient(int patientId)
        {
            Patient patient =
                patientRepository.GetById(p => p.Id == patientId);

            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                $"===== PRESCRIPTIONS FOR {patient.Name.ToUpper()} =====");

            List<Prescription> prescriptions =
                GetPrescriptionsByPatientId(patientId);

            if (prescriptions.Count == 0)
            {
                Console.WriteLine("No prescriptions found.");
                return;
            }

            foreach (Prescription prescription in prescriptions)
            {
                Console.WriteLine(
                    $"Prescription ID: {prescription.Id} | " +
                    $"Medication: {prescription.MedicationName} | " +
                    $"Date Issued: {prescription.DateIssued.ToShortDateString()}"
                );
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            HealthSystemApp app = new HealthSystemApp();

            app.SeedData();

            app.BuildPrescriptionMap();

            app.PrintAllPatients();

            Console.WriteLine();

            app.PrintPrescriptionsForPatient(1);
        }
    }
}

