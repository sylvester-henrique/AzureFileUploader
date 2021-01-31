# AzureFileUploader

É uma aplicação simples que comprime os arquivos de um determinado diretório e os envia para um container do Azure Storage. Todos os arquivos do diretório especificado serão comprimidos no formato *zip*.

Para enviar os arquivos é gerado o arquivo temporário ```"AzureFileUploaderTemp.zip" ``` no diretório pai do diretório especificado. Esse arquivo será removido depois que o envio for concluído.

### Configuração

É necessário criar uma variável de ambiente para armazenar a *connection string* do Azure Storage. Para definir uma *connection string* execute no cmd:

`$ setx NOME_DA_VARIAVEL "SuaConnectionString"`


**UploadAbsolutePath:** caminho absoluto onde os arquivos para upload se encontram.
**ConnectionStringEnvironmentVariableName:** nome da variável de ambiente que contém a *connection string* da conta do Azure Storage.
**BlobContainer**: nome do *blob container* do Azure
**BlobNamePrefix:** prefixo (opcional) do nome do arquivo que será gerado. Ele será concatenado com a data e hora atual para gerar o nome do arquivo. Ex: ```"prefixo_2021-01-30_10h-34m.zip"```  

#### appsettings.json

```json
{
        "UploadAbsolutePath": "C:\\DiretorioParaUpload",
        "Azure": {
                  "ConnectionStringEnvironmentVariableName": "NOME_DA_VARIAVEL",
                  "BlobContainer": "NomeDoBlobContainer",
                  "BlobNamePrefix": "PrefixoDoNomeDoArquivoGerado"
          }
}
```
###Screenshots

[![](https://raw.githubusercontent.com/SylvesterH13/AzureFileUploader/master/screenshots/screenshot1.png "asdfsdf")](https://raw.githubusercontent.com/SylvesterH13/AzureFileUploader/master/screenshots/screenshot1.png "asdfsdf")

[![](https://github.com/SylvesterH13/AzureFileUploader/blob/master/screenshots/screenshot2.png?raw=true)](https://github.com/SylvesterH13/AzureFileUploader/blob/master/screenshots/screenshot2.png?raw=true)
