using Lambda.Pregiato.Interface;
using Lambda.Pregiato.Models;
using Newtonsoft.Json;
using RestSharp;

public class AutentiqueService : IAutentiqueService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl = Environment.GetEnvironmentVariable("AUTENTIQUE_API_URL") ?? "https://api.autentique.com.br/v2/graphql";
    private readonly string _token = "4ea6a0455f8e2e02b2f299be01d1a0949b24ceaf8d8bf7e7c5b56cff133c1f71" ?? Environment.GetEnvironmentVariable("AUTENTIQUE_TOKEN");
    private readonly Logger _logger; 

    public AutentiqueService()
    {
        _httpClient = new HttpClient();
        _logger = new Logger(); 
    }

    public async Task<string> CreateDocumentAsync(string documentName, string documentBase64, Model model)
    {
        var client = new RestClient(_apiUrl);
        var request = new RestRequest();
        request.Method = Method.Post;

        request.AddHeader("Authorization", $"Bearer {_token}");
        request.AddHeader("Content-Type", "multipart/form-data");

        try
        {
            string operations = JsonConvert.SerializeObject(new
            {
                query = @"mutation CreateDocumentMutation($document: DocumentInput!, $signers: [SignerInput!]!, $file: Upload!) {
                    createDocument(document: $document, signers: $signers, file: $file) {
                        id
                        name
                        refusable
                        sortable
                        created_at
                        signatures {
                            public_id
                            name
                            email
                            created_at
                            action { name }
                            link { short_link }
                            user { id name email }
                        }
                    }
                }",
                variables = new
                {
                    document = new { name = documentName },
                    signers = new[] { new { email = model.Email, action = "SIGN" } },
                    file = (string)null
                }
            });

            string map = JsonConvert.SerializeObject(new { file = new[] { "variables.file" } });

            request.AddParameter("operations", operations);
            request.AddParameter("map", map);

            byte[] pdfBytes = Convert.FromBase64String(documentBase64);
            request.AddFile("file", pdfBytes, $"{documentName}.pdf", "application/pdf");

            _logger.LogInfo($"Criando documento {documentName} para o modelo {model.Name}.");

            RestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                _logger.LogError($"Erro ao criar documento no Autentique: {response.StatusCode}");
                _logger.LogError($"Response Content: {response.Content}");
                throw new Exception($"Erro ao criar documento: {response.StatusCode} - {response.Content}");
            }

            _logger.LogInfo($"Documento {documentName} criado com sucesso para o modelo {model.Name}.");
            return $"Contrato de {documentName} gerado para {model.Name} com sucesso.";
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao criar documento {documentName}.", ex);
            throw;
        }
    }
}