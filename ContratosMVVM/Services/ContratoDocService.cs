using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using Xceed.Document.NET;
using Xceed.Words;
using Xceed.Words.NET;


namespace ContratosMVVM.Services
{
    public class ContratoDocService
    {
        private const string _razãoSocial = "TRILHA TECNOLOGIA LTDA";
        private const string _cnpj = "14.839.018.0001-96";
        private const string _ie = "146.869.818.113";
        private const string _representante = "Ricardo Leite Machi";
        private const string _cpf = "157.608.748-45";
        private CLIENTE _cliente;
        private List<CONTRATO> _contratos;
        private DocX _documento;
        public void IniciaNovoArquivo(CLIENTE cliente, List<CONTRATO> contratos)
        {
            _cliente = cliente;
            _contratos = contratos;
        }

        public void FazAPorraToda()
        {
            //Instancia o documento
            var document = DocX.Create($@"Contratos\{_cliente.RazãoSocial.Replace(@"\", "").Replace(@"/", "")}.docx");
            document.MarginLeft = 60;
            document.MarginRight = 60;

            document.MarginTop = 30;
            document.MarginBottom = 30;

            //Avisa que serão usados cabeçalhos e rodapés
            document.AddHeaders();
            document.AddFooters();

            document.SetDefaultFont(new Font("Calibri"), 11D);

            //Define que a primeira página também irá usar o header odd
            //Caso contrário, é necessário setar headers.first
            document.DifferentFirstPage = false;

            //Define as imagens a serem usadas no documento
            var logoImage = document.AddImage(@"Images/logo.png");
            var assinaturaImage = document.AddImage(@"Images/assinatura.png");
            var rubricaImage = document.AddImage(@"Images/rubrica.png");
            var logoPicture = logoImage.CreatePicture(45, 187);
            var assinaturaPicture = assinaturaImage.CreatePicture(18, (float)172.6);
            var rubricaPicture = rubricaImage.CreatePicture(33, (float)36.6);

            document.Footers.Odd.Paragraphs[0].AppendPicture(rubricaPicture).AppendLine().Alignment = Alignment.right;

            document.Footers.Odd.InsertParagraph("Rua Frei Vital de Frescarolo, 535 - JD JOÃO XXIII, São Paulo, SP - CEP: 05569-030 - Tel.: (11) 4304-7778")
                    .Bold().FontSize(10.5D)
                    .AppendLine($"São Paulo, {DateTime.Today.ToLongDateString()}").Bold().FontSize(10.5D).Alignment =
                Alignment.center;

            //Cria a tabela no Header
            var t = document.Headers.Odd.Paragraphs[0].InsertTableBeforeSelf(1, 2);

            //Define as bordas como transparentes
            var blankBorder = new Border(BorderStyle.Tcbs_none, 0, 0f, Color.White);
            foreach (var borderType in Enum.GetValues<TableBorderType>())
            {
                t.SetBorder(borderType, blankBorder);
            }

            //Define o conteúdo das células da tabela
            t.Rows[0].Cells[0].Paragraphs[0].InsertPicture(logoPicture);
            t.Rows[0].Cells[1].Paragraphs[0].Append(_razãoSocial)
                .FontSize(8D).Bold()
                .AppendLine($"CNPJ: {_cnpj}")
                .FontSize(8D).Bold()
                .AppendLine($"I.E. {_ie}")
                .FontSize(8D).Bold()
                .AppendLine("Tel.: 11 4304-7778")
                .FontSize(8D)
                .AppendLine("www.trilhatecnologia.com")
                .FontSize(8D);

            document.InsertParagraph();
            document.InsertParagraph("CONTRATO UNIFICADO DE SERVIÇO").FontSize(16D).Bold().AppendLine().Alignment = Alignment.center;


            var t1 = document.InsertParagraph().InsertTableBeforeSelf(2, 1);

            t1.Rows[0].Cells[0].Paragraphs[0].Append("DORAVANTE DENOMINADA: CONTRATANTE").Bold().Alignment = Alignment.center;
            t1.Rows[1].Cells[0].Paragraphs[0].Append("RAZÃO SOCIAL: ").Bold().Append($"{_cliente.RazãoSocial.ToUpper()} ")
                .AppendLine("CNPJ: ").Bold().Append($"{_cliente.CNPJCPF}")
                .AppendLine("ENDEREÇO: ").Bold().Append($"{_cliente.Endereço.ToUpper()}")
                .AppendLine("CEP: ").Bold().Append($"{_cliente.CEP} ").Append("BAIRRO: ").Bold().Append($"{_cliente.Bairro.ToUpper()} ").Append("CIDADE: ").Bold().Append($"{_cliente.Cidade.ToUpper()}")
                .AppendLine("REPRESENTADA POR: ").Bold().Append($"{(_cliente.Representante??"").ToUpper()} ").Append("CPF: ").Bold().Append($"{_cliente.CPFDoRepresentante}");

            var t2 = document.InsertParagraph().InsertTableBeforeSelf(2, 1);

            t2.Rows[0].Cells[0].Paragraphs[0].Append("DORAVANTE DENOMINADA: CONTRATADA").Bold().Alignment = Alignment.center;
            t2.Rows[1].Cells[0].Paragraphs[0].Append("RAZÃO SOCIAL: ").Bold().Append($"{_razãoSocial} ".ToUpper()).Append("CNPJ: ").Bold().Append($"{_cnpj}")
                .AppendLine("ENDEREÇO: ").Bold().Append($"RUA FREI VITAL DE FRESCAROLO, 535".ToUpper())
                .AppendLine("CEP: ").Bold().Append($"05569-030 ").Append("BAIRRO: ").Bold().Append($"JD. JOÃO XXIII ".ToUpper()).Append("CIDADE: ").Bold().Append($"SÃO PAULO".ToUpper())
                .AppendLine("REPRESENTADA POR: ").Bold().Append($"{_representante} ".ToUpper()).Append("CPF: ").Bold().Append($"{_cpf}");

            var t3 = document.InsertParagraph().InsertTableBeforeSelf(1, 1);

            t3.Rows[0].Cells[0].Paragraphs[0].Append("Valor Mensal do Contrato: ").Bold().Append($"{_contratos?.Sum(x => x.ValorTotalDoContrato) ?? 0:C2}")
                .AppendLine()
                .AppendLine("A quebra do contrato no período de vigência será ").Bold().Append("com").Bold().UnderlineStyle(UnderlineStyle.singleLine).Append(" multa").Bold();


            var t4 = document.InsertParagraph().InsertTableBeforeSelf(((_contratos?.Count) ?? 0) + 1, 5);

            t4.SetWidthsPercentage(new float[]{8,58,10,14,10}, 468);

            t4.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            t4.Rows[0].Cells[0].Paragraphs[0].Append("Itens").Bold().Alignment = Alignment.center;

            t4.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            t4.Rows[0].Cells[1].Paragraphs[0].Append("Descrição dos Serviços").Bold().Alignment = Alignment.center;

            t4.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;
            t4.Rows[0].Cells[2].Paragraphs[0].Append("Qtd.").Bold().Alignment = Alignment.center;

            t4.Rows[0].Cells[3].VerticalAlignment = VerticalAlignment.Center;
            t4.Rows[0].Cells[3].Paragraphs[0].Append("Vlr. Fixo").Bold().AppendLine("Mensal").Bold().Alignment = Alignment.center;

            t4.Rows[0].Cells[4].VerticalAlignment = VerticalAlignment.Center;
            t4.Rows[0].Cells[4].Paragraphs[0].Append("Vigência do").Bold().AppendLine("Contrato").Bold().Alignment = Alignment.center;

            var row = 1;
            foreach (var contrato in _contratos)
            {
                t4.Rows[row].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                t4.Rows[row].Cells[0].Paragraphs[0].Append($"{row:D2}").Bold().Alignment = Alignment.center;

                t4.Rows[row].Cells[1].Paragraphs[0].Append(contrato.ContratoBase.Nome).Bold()
                    .AppendLine($"{contrato.ContratoBase.Descrição}").FontSize(8D);

                t4.Rows[row].Cells[2].VerticalAlignment = VerticalAlignment.Center;
                t4.Rows[row].Cells[2].Paragraphs[0].Append($"{contrato.Quantidade:N0}").Bold().Alignment = Alignment.center;

                t4.Rows[row].Cells[3].VerticalAlignment = VerticalAlignment.Center;
                t4.Rows[row].Cells[3].Paragraphs[0].Append($"{contrato.ValorUnitário:C2}").Bold().Alignment = Alignment.center;

                t4.Rows[row].Cells[4].VerticalAlignment = VerticalAlignment.Center;
                t4.Rows[row].Cells[4].Paragraphs[0].Append($"{contrato.Vigência:N0}").Bold().AppendLine("Meses").Bold().Alignment = Alignment.center;

                row++;
            }

            document.InsertSectionPageBreak();
            Formatting listTitle = new()
            {
                Bold = true,
                CapsStyle = CapsStyle.caps,
                Size = 12D

            };
            var listTitle1 = document.AddList("DO PRAZO DE VIGÊNCIA", 0, ListItemType.Numbered, 1, formatting: listTitle);
            
            var list1 = document.AddList("A CONTRATANTE deverá comunicar a CONTRATADA, formalmente via e-mail, a sua intenção de não prorrogação do contrato, com pelo menos 30 dias de antecedência da data término do prazo contratual. Caso não haja manifestações por qualquer uma das partes o prazo de comodato será prorrogado automaticamente.", 1, continueNumbering:true);
            document.AddListItem(list1, "A quebra do contrato no período de vigência será com multa, conforme artigo 8.", 1);
            document.AddListItem(list1, $"O prazo da locação é de {_contratos[0].Vigência} meses.", 1);
            
            var listTitle2 = document.AddList("da forma de pagamento", 0, formatting:listTitle, continueNumbering:true);
            var list2 = document.AddList("O valor dos serviços prestados tem vencimento todo dia 10 de cada mês. A CONTRATANTE se obriga a pagar pontualmente até o dia do vencimento, sendo de responsabilidade do CONTRATADA a emissão e o envio do mesmo a CONTRATANTE.", 1, continueNumbering:true);
            document.AddListItem(list2, "Quaisquer parcelas pagas com atraso sofrerão correção por índice oficial, ou na falta deste, por índice razoável e semelhante a ser escolhido pela CONTRATADA e o nome da empresa será protestado e negativado nos órgãos de proteção ao crédito.", 1);
            document.AddListItem(list2, "A CONTRATADA poderá emitir boletos de cobrança e NF-e de qualquer uma das empresas de seu grupo, tais como:", 1);

            var listTitle3 = document.AddList("dos termos e responsabilidades", 0, formatting: listTitle, continueNumbering:true);
            //var list3 = document.AddList("A CONTRATANTE compreende e concorda que a CONTRATADA solucionará os problemas e corrigirá os erros do sistema na medida em que a CONTRATANTE forneça e relate informações como documentação e relatórios acerca dos erros e problemas ocorridos. A ausência ou insuficiência de informações até mesmo de um suporte do fabricante tanto de software como de hardware podem dificultar ou até impossibilitar os trabalhos da CONTRATADA.", 2, continueNumbering:true);
            var list3 = document.AddList("A CONTRATANTE deverá fornecer nome, endereço, cargo/função, telefone para contato e outros dados necessários dos responsáveis pelos contatos com os técnicos da CONTRATADA.", 1, continueNumbering:true);
            document.AddListItem(list3, "Sempre que necessário a CONTRATANTE se obrigará a ceder suas instalações e equipamentos pessoais para facilitar o acesso aos trabalhos da CONTRATADA, desta forma, auxiliando à execução dos serviços de Assistência Técnica de Manutenção.", 1);
            document.AddListItem(list3, "Além da remuneração nas condições de pagamento descrita, a CONTRATANTE será responsável pelos impostos, taxas, emolumentos e tributos em geral devidos por força do presente contrato.", 1);
            document.AddListItem(list3, "Enquanto o contrato estiver ativo, ou 36 meses após o encerramento de contrato a CONTRATANTE não poderá contratar quaisquer um dos nossos colaboradores direta ou indiretamente para prestar os mesmos serviços descritos. Caso seja descumprido, será estipulada a multa de 3 (três) mensalidades vigentes a partir da data de infração.", 1);

            var listTitle4 = document.AddList("dos REAJUSTES", 0, formatting: listTitle, continueNumbering:true);
            var list4 = document.AddList("O valor estipulado neste contrato será reajustado com base na variação acumulada no IGPM (Índice Geral de Preços do Mercado), variação a ser aplicada em qualquer época de vigência deste contrato, atendida sempre a menor periodicidade que venha a ser admitida em lei e que no momento é de 1 (um) ano, a contar do mês da assinatura deste contrato. Na hipótese de suspensão, extinção ou vedação do uso do IGPM como índice de atualização de preços, fica eleito o índice que oficialmente vier a substituí-lo, na hipótese da não determinação deste, aquele que melhor reflita a variação ponderada dos custos da CONTRATADA desde que publicamente divulgado como índice substitutivo a vigorar entre as partes.", 1, continueNumbering:true);
            document.AddListItem(list4, "O pagamento do serviço contratado deverá ser efetuado pela CONTRATANTE no estabelecimento bancário indicado pela CONTRATADA, nos respectivos documentos de cobrança, na ausência, no endereço da CONTRATADA.", 1);
            document.AddListItem(list4, "Em caso de inadimplência das parcelas por parte da CONTRATANTE por prazo superior a 05 (cinco) dias corridos, o serviço será bloqueado até a regularização completa das pendencias financeiras, desta forma a CONTRATADA ficará desobrigada a fornecer quaisquer serviços a CONTRATANTE enquanto perdurar a pendência financeira.", 1);
            document.AddListItem(list4, "Na falta do exato e pontual pagamento do serviço ou de quaisquer importâncias devidas pela CONTRATANTE, incidirão multa de 5% (cinco por cento) e juros de mercado consignados do documento de cobrança ou informados pela CONTRATADA. O atraso superior a 30 (trinta) dias faculta à CONTRATADA a rescisão deste contrato. Nesta hipótese, a CONTRATADA irá retirar o(s) equipamento(s) em comodato(s), cabendo a CONTRATANTE pagar os aluguéis vencidos até a data da retirada, com acréscimos da mora diária e moratória de 20% (vinte por cento) sobre o débito apurado.", 1);
            document.AddListItem(list4, "No valor mensal estão inclusos apenas reparos, equipamentos e peças dos serviços contratados descritos no contrato.", 1);

            var listTitle5 = document.AddList("da ASSISTÊNCIA TÉCNICA", 0, formatting: listTitle, continueNumbering: true);
            var list5 = document.AddList("Durante a vigência deste contrato, a CONTRATADA, por si ou por terceiros, por ela credenciada, será responsável pela manutenção ou reparo do(s) equipamento(s) ou sistema(s), sem quaisquer ônus para a CONTRATANTE.", 1, continueNumbering: true);
            document.AddListItem(list5, "Os serviços de manutenção ou assistência remota serão efetuados durante o horário comercial, no prazo de até 48 horas para o atendimento, sendo certo que se houver necessidade de sua realização fora desse horário comercial, as despesas serão cobradas de acordo com a tabela de preço vigente de atendimento da CONTRATADA.", 1);
            document.AddListItem(list5, "Atendimentos solicitados aos sábados, domingos e feriados serão avaliados pontualmente a necessidade do problema relatado, e caso o atendimento seja realizado, as despesas serão cobradas de acordo com a tabela de preço vigente.", 1);
            //document.AddListItem(list5, "A CONTRATADA ficará desobrigada de prestar os serviços descritos neste contrato, ficando obrigada a CONTRATANTE orçar à parte ou contactar outra empresa para realizar o atendimento.", 1);
            document.AddListItem(list5, "Caso haja dano ou defeito causado por negligência, imprudência, ou inabilitação do manuseio do(s) equipamento(s) ou sistema(s) pela CONTRATANTE; intervenção de pessoas não autorizadas pela CONTRATADA, na tentativa de reparo do(s) equipamento(s) ou sistema(s); uso de materiais ou de suprimentos em desacordo com as especificações técnicas; uso de energia elétrica inadequada, raios, atos de força maior e queda do(s) equipamento(s), a CONTRATANTE irá arcar com os ônus que vierem a surgir.", 1);

            var listTitle6 = document.AddList("DO SERVIÇO DE COMODATO", 0, formatting: listTitle, continueNumbering: true);
            var list6 = document.AddList("É expressamente vedado à CONTRATANTE mudar o(s) equipamento(s) para outro local sem a prévia concordância da CONTRATADA. Ocorrendo esta ação, a CONTRATANTE arcará com todas as despesas de remoção ou reinstalação(ões) inclusive as despesas com materiais e mão de obra necessária.", 1, continueNumbering: true);
            document.AddListItem(list6, "A CONTRATANTE deverá manter o(s) equipamento(s) ou sistema(s) em perfeito estado e funcionamento, como se próprio fosse, respondendo por danos que vierem a sofrer como incêndios, quedas, uso indevido, ligação em 220v ou em desacordo com as especificações, roubo, furto e qualquer evento ainda que por culpa de terceiros, obrigando-se a indenizar imediatamente à CONTRATADA pelos prejuízos resultantes; desde já prefixados no valor do(s) equipamento(s) ou sistema(s) conforme a tabela do fabricante, ou na falta da mesma, o valor à vista do(s) equipamento(s) ou sistema(s) igual(is) ou similar(es).", 1);
            document.AddListItem(list6, "Ocorrem por conta do CONTRATANTE as despesas de transportes e, se for o caso, a devolução do(s) equipamento(s) para o estabelecimento da CONTRATADA, no término ou rescisão deste contrato.", 1);
            document.AddListItem(list6, "A CONTRATANTE deverá respeitar o direito de propriedade da CONTRATADA sobre o(s) equipamento(s) locado(s), comunicando de imediato qualquer embaraço, arresto, turbação de posse. Em qualquer dos casos, dar a conhecer à parte interessada, ‘initio litis’, o direito de propriedade da CONTRATADA sobre o(s) equipamento(s).", 1);
            document.AddListItem(list6, "A CONTRATADA recomenda cobertura secundária de risco relativo aos bens locados (como seguro total que inclua os equipamentos). Quaisquer custos provenientes ao seguro ocorrerão por conta da CONTRATANTE.", 1);


            var listTitle7 = document.AddList("das DISPOSIÇÕES GERAIS", 0, formatting: listTitle, continueNumbering:true);
            var list7 = document.AddList("A CONTRATADA instalará o(s) sistema(s) ou equipamento(s) no local indicado pela CONTRATANTE ou de melhor posicionamento, deixando-o(s) em perfeitas condições de funcionamento, ficando a cargo do CONTRATANTE preparar previamente as instalações adequadas para este fim, atendendo às especificações técnicas solicitadas pela CONTRATADA.", 1, continueNumbering:true);
            document.AddListItem(list7, "As obrigações decorrentes deste contrato deverão ser integralmente respeitadas até o termo final de sua vigência, ainda que a CONTRATANTE por sua conta e risco deixe de usar o(s) sistema(s) ou equipamento(s) locado(s).", 1);
            document.AddListItem(list7, "A tolerância da CONTRATADA no exigir o integral cumprimento dos termos e condições deste contrato, ou exercer qualquer prerrogativa dele decorrente, não constituirá novação ou renúncia, nem afetará os seus direitos que poderão ser exercidos integralmente a todo e qualquer tempo.", 1);
            document.AddListItem(list7, "A CONTRATADA poderá transmitir a terceira parte ou a totalidade dos seus direitos e obrigações decorrentes deste contrato. Ou seja, ficando solidariamente responsável, perante a CONTRATANTE, pelo cumprimento integral das obrigações assumidas.", 1);
            document.AddListItem(list7, "A CONTRATANTE deverá dar total acesso à CONTRATADA para manutenção e vistoria em seus equipamentos ou sistemas.", 1);

            var listTitle8 = document.AddList( "da MULTA CONTRATUAL", 0, formatting: listTitle, continueNumbering:true);
            var list8 = document.AddList("A rescisão antecipada por parte da CONTRATANTE acarretará a incidência da multa contratual de 1/3 (um terço) do restante do contrato.", 1, continueNumbering:true);
            document.AddListItem(list8, "O contrato será rescindido pela CONTRATADA, de pleno direito, independentemente de qualquer aviso ou notificação judicial ou extrajudicial, nos casos de incorreções nas informações cadastrais prestadas pela CONTRATANTE, atraso no pagamento da mensalidade (artigo 2, seção a), intervenção de pessoas não autorizadas a fazer manutenção ou reparo do(s) equipamento(s), bem como nos casos de falência, concordata ou insolvência da CONTRATANTE e outras infrações contratuais.", 1);

            var listTitle9 = document.AddList("do TÉRMINO DO CONTRATO", 0, formatting: listTitle, continueNumbering:true);
            var list9 = document.AddList("O presente contrato pode ser encerrado a qualquer momento por quaisquer das partes, desde que, a parte interessada no encerramento comunique à outra parte com antecedência de 30 (trinta) dias, e não existam pendências de pagamento por parte da CONTRATANTE. A solicitação deverá ser comunicada formalmente via e-mail.", 1, continueNumbering:true);
            document.AddListItem(list9, "O presente contrato pode também ser rescindido caso uma das partes venha a falir, por determinação de sentença judicial ou requerida concordata, ficando a fadada ou concordatária obrigada desde já, a reparar os prejuízos e indenizar por perdas, danos e lucros cessantes.", 1);
            document.AddListItem(list9, "O cancelamento do contrato será definitivo após os pagamentos das pendências e a retirada dos equipamentos e/ou desinstalação do sistema.", 1);

            var listTitle10 = document.AddList("do FORO", 0, formatting: listTitle, continueNumbering:true);
            var list10 = document.AddList("As partes elegem como foro do contrato a cidade de São Paulo, estado de SP, com exclusão de outro, por mais privilegiado que seja, para dirimir quaisquer dúvidas proveniente deste Instrumento.", 1, continueNumbering:true);
            
            var listTitle11 = document.AddList("das ASSINATURAS DAS PARTES E TESTEMUNHAS", 0, formatting: listTitle, continueNumbering:true);
            var list11 = document.AddList("E, por estarem justos e acertados, assinam o presente instrumento (assinatura conforme documento de identidade) em duas vias de igual teor e forma, na presença de duas testemunhas, para que produzam seus devidos e legais efeitos.", 1, continueNumbering:true);

            document.InsertList(listTitle1);
            document.InsertParagraph();
            document.InsertList(list1);
            document.InsertParagraph();
            document.InsertList(listTitle2);
            document.InsertParagraph();
            document.InsertList(list2);
            document.InsertParagraph();
            var tEmpresas = document.InsertParagraph().InsertTableBeforeSelf(1, 1);
            document.InsertList(listTitle3);
            document.InsertParagraph();
            document.InsertList(list3);
            document.InsertParagraph();
            document.InsertList(listTitle4);
            document.InsertParagraph();
            document.InsertList(list4);
            document.InsertParagraph();
            document.InsertList(listTitle5);
            document.InsertParagraph();
            document.InsertList(list5);
            document.InsertParagraph();
            document.InsertList(listTitle6);
            document.InsertParagraph();
            document.InsertList(list6);
            document.InsertParagraph();
            document.InsertList(listTitle7);
            document.InsertParagraph();
            document.InsertList(list7);
            document.InsertParagraph();
            document.InsertList(listTitle8);
            document.InsertParagraph();
            document.InsertList(list8);
            document.InsertParagraph();
            document.InsertList(listTitle9);
            document.InsertParagraph();
            document.InsertList(list9);
            document.InsertParagraph();
            document.InsertList(listTitle10);
            document.InsertParagraph();
            document.InsertList(list10);
            document.InsertParagraph();
            document.InsertList(listTitle11);
            document.InsertParagraph();
            document.InsertList(list11);
            document.InsertParagraph();
            document.InsertParagraph().Append($"São Paulo, {DateTime.Today.ToLongDateString()}").AppendLine().AppendLine().Alignment = Alignment.center;

            foreach (var documentParagraph in document.Paragraphs)
            {
                if (documentParagraph.IsListItem) documentParagraph.Alignment = Alignment.both;
            }

            tEmpresas.Rows[0].Cells[0].Paragraphs[0]
                .Append("AMBISOF DISTRIBUIDORA TECNOLOGICA LTDA  - CNPJ: 22.141.365/0001-79").Bold().FontSize(12D)
                .AppendLine()
                .AppendLine("TRILHA MULTICOISAS LTDA - CNPJ: 30.737.989/0001-81").Bold().FontSize(12D)
                .AppendLine()
                .AppendLine("DIGITRON DISTRIBUIDORA TECNOLOGICAS LTDA - CNPJ: 10.708.829/0001-05").Bold().FontSize(12D)
                .AppendLine()
                .AppendLine("TRILHA TECNOLOGIA LTDA - CNPJ: 14.839.018/0001-96").Bold().FontSize(12D);


            
            var tAssinaturas = document.InsertParagraph().InsertTableBeforeSelf(2, 2);

            foreach (var borderType in Enum.GetValues<TableBorderType>())
            {
                tAssinaturas.SetBorder(borderType, blankBorder);
            }

            tAssinaturas.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            tAssinaturas.Rows[0].Cells[0].Paragraphs[0].AppendLine("___________________________").AppendLine($"{_cliente.Representante}").Bold().AppendLine($"{_cliente.RazãoSocial}").Alignment = Alignment.center;

            tAssinaturas.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            tAssinaturas.Rows[0].Cells[1].Paragraphs[0].AppendPicture(assinaturaPicture).AppendLine($"{_representante}").Bold().AppendLine($"{_razãoSocial}").Alignment = Alignment.center;

            tAssinaturas.Rows[1].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            tAssinaturas.Rows[1].Cells[0].Paragraphs[0].AppendLine("___________________________").AppendLine($"1ª Testemunha").Italic().Alignment = Alignment.center;

            tAssinaturas.Rows[1].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            tAssinaturas.Rows[1].Cells[1].Paragraphs[0].AppendLine("___________________________").AppendLine($"2ª Testemunha").Italic().Alignment = Alignment.center;

            //document.InsertParagraph($"São Paulo, {DateTime.Today.ToLongDateString()}").Alignment = Alignment.center;
            document.Save();

            //Process.Start($@"Contratos\{_cliente.RazãoSocial}.docx");
        }
    }
}
