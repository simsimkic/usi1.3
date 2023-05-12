using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Service;

namespace ZdravoCorp.Controllers
{
    public class MedicalRecordController
    {
        private MedicalRecordService _medicalRecordService;

        public MedicalRecordController(MedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        public List<MedicalRecord> GetMedicalRecordsForPatient(int patientId)
        {
            return _medicalRecordService.GetMedicalRecordsForPatient(patientId);
        }

        public String GetAllDiseasesForMedicalRecord(int patientId)
        {
            return _medicalRecordService.GetAllDiseases(patientId);
        }

        public String GetAllDiseases(int patientId)
        {
            return _medicalRecordService.GetAllDiseases(patientId);
        }
    }
}
