using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lambda.Pregiato.DTO;
using Lambda.Pregiato.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

public class AutentiqueService : IAutentiqueService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl = Environment.GetEnvironmentVariable("AUTENTIQUE_API_URL", EnvironmentVariableTarget.Machine);
    private readonly string _token = "27ddb743825980c4aa7d258174a5b0b299f66be108d9f4cfe1172266d74b8066";
                                     //Environment.GetEnvironmentVariable("AUTENTIQUE_TOKEN", EnvironmentVariableTarget.Machine);

    public AutentiqueService()
    {
        _httpClient = new HttpClient();

    }

    public async Task<string> CreateDocumentAsync(string documentName, string documentBase64)
    {
        var client = new RestClient("https://api.autentique.com.br/v2/graphql");
        var request = new RestRequest();
        request.Method = Method.Post;

        // Adiciona o cabeçalho de autenticação
        request.AddHeader("Authorization", $"Bearer {_token}");
        request.AddHeader("Content-Type", "multipart/form-data");

        // Estrutura da query GraphQL
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
                signers = new[] { new { email = "admin@pregiato.com.br", action = "SIGN" } },
                file = (string)null
            }
        });

        // Mapeamento do arquivo
        string map = JsonConvert.SerializeObject(new { file = new[] { "variables.file" } });

        // Adiciona os parâmetros necessários
        request.AddParameter("operations", operations);
        request.AddParameter("map", map);

        // Converte a string Base64 para bytes e adiciona como arquivo
        byte[] pdfBytes = Convert.FromBase64String(documentBase64);
        request.AddFile("file", pdfBytes, $"{documentName}.pdf", "application/pdf");

        // Envia a requisição
         RestResponse response = client.Execute(request);

        // Verifica se houve erro
        if (!response.IsSuccessful)
        {
            Console.WriteLine($"Erro ao criar documento: {response.StatusCode}");
            Console.WriteLine($"Response Content: {response.Content}");
            throw new Exception($"Erro ao criar documento: {response.StatusCode} - {response.Content}");
        }

        return response.Content;
    }


}