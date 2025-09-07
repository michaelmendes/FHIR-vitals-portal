public class FhirService
    {
        private readonly FhirClient _client;

        public FhirService(string fhirBaseUrl)
        {
            _client = new FhirClient(fhirBaseUrl);
        }

        public async Task<Patient?> GetPatientByFamilyName(string familyName)
        {
            var bundle = await _client.SearchAsync<Patient>(new string[] { $"family={familyName}" });
            return bundle.Entry.Select(e => e.Resource).OfType<Patient>().FirstOrDefault();
        }

        public async Task<List<Observation>> GetLatestVitals(string patientId, int count = 10)
        {
            var bundle = await _client.SearchAsync<Observation>(
                new[] { $"patient={patientId}", "category=vital-signs", $"_count={count}", "_sort=-date" });
            return bundle.Entry.Select(e => e.Resource).OfType<Observation>().ToList();
        }
    }