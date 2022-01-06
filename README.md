# Gerador de Contratos

## Sobre

Um aplicativo que obtém informações de clientes a partir de uma base Firebird 2.5, e permite a criação de arquivos *.docx modulares de acordo com informações previamente cadastradas.

## Tecnologias

A base original de onde as informações da empresa são obtidas é uma Firebird 2.5 de terceiros, de forma que a estrutura não é controlada pelo nosso aplicativo. Por outro lado, além dos dados necessários para a formulação de um contrato, mais nenhuma informação é obtida, e nenhuma é gravada.

Os dados usados no funcionamento do aplicativo ficam em uma simples base SQLite, que inclui tipos de contrato, suas descrições, clientes/contratos/valores relacionados assim como o caminho do arquivo *.docx gerado.

Os contratos gerados em formato *.docx são criados utilizando-se a biblioteca [DocX (1.8.0)](https://www.nuget.org/packages/DocX/), e são completamente modulares, não restringindo o número de cláusulas inseridas por cliente.

#### Ainda em uso em Trilha Tecnologia.
