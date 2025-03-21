using Lambda.Pregiato.Interface;
using Lambda.Pregiato.Services.ModelService;

public async Task ProcessMessage(ContractMessage contractMessage)
{
    try
    {
        Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Verificando mensagem {contractMessage}...");

        if (string.IsNullOrEmpty(contractMessage.CpfModel) && string.IsNullOrEmpty(contractMessage.ContractIds.ToString()))
        {
            Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Dados do modelo inconsistentes.");
            return; 
        }

        Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Buscando dados do modelo portador do CPF: {contractMessage.CpfModel}.");
        var model = await _modelRepository.GetModelByCriteriaAsync(contractMessage.CpfModel);

        if (model == null)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Modelo {contractMessage.CpfModel} não encontrado na base de dados.");
            return; 
        }

        Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Dados do modelo: {model.Name} | Portador do CPF: {model.CPF}.");

        foreach (var id in contractMessage.ContractIds)
        {
            try
            {
                if (Guid.TryParse(id, out Guid contractId))
                {
                    Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Buscando contrato: {contractId}.");
                    var contract = await _contractRepository.GetContractById(contractId);
                    if (contract != null)
                    {
                        Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Contrato {contractId} encontrado.");
                        string contentString = await _contractService.ConvertBytesToString(contract.Content);
                        byte[] pdfBytes = await _contractService.ExtractBytesFromString(contentString);
                        string pdfBase64 = Convert.ToBase64String(pdfBytes);
                        string nameFile = contract.ContractFilePath;
                        var result = await _autentiqueService.CreateDocumentAsync(nameFile, pdfBase64, model);

                        if (result == null)
                        {
                            Console.WriteLine($"[WARN] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Documento não foi criado para o contrato {contractId}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Erro: Contrato ID {contractId} não encontrado.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Erro ao processar contrato {id}: {ex.Message}");
                
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Erro ao processar mensagem: {ex.Message}");
        
    }
}