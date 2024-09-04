/* *******************************************************************
* Colegio Técnico Antônio Teixeira Fernandes (Univap)
* Curso Técnico em Informática - Data de Entrega: 10/09/2024
* Autores do Projeto: Leonardo Martinelli de Oliveira Lima
*                     Natan Soares Telles
* Turma: 2H
* Projeto bimestral 3
* Observação: 
* 
* textBox1 - inserir o valor da compra
* textBox2 - inserir a quantidade de parcelas
* textBox3 - inserir data da compra
* textBox4 - parcelas da compra
* textBox5 - inserir data do pagamento da parcela
* label1 - texto valor da compra
* label2 - texto numero de parcelas
* label3 - texto data da compra
* label4 - texto valor total da compra
* label5 - texto R$ para o textBox1
* label6 - texto data de pagamento 
* label7 - valor com juros da parcela atrasada
* label8 - valor acumulado dos juros
* button1 - calcular parcelas
* button2 - realizar pagamento da parcela
* button3 - obter data no momento do clique
* 
* ************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto3b
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //variáveis globias
        float valorTotal;
        int numeroParcelas;
        DateTime dataCompra;
        float valorParcela;
        int parcelas_pagas;
        float juros_acumulados;


/*----------------------------------------------------------------------------------------------------------------*/
        private void button1_Click(object sender, EventArgs e) //botão para preencher as parcelas de acordo com entrada do usuário
        {
            float.TryParse(textBox1.Text, out valorTotal);
            int.TryParse(textBox2.Text, out numeroParcelas);
            DateTime.TryParse(textBox3.Text, out dataCompra);

            valorParcela = valorTotal / numeroParcelas;
            textBox4.Text = ""; //zera o textBox para futuras simulações
            label7.Text = ""; //zera o label de juros para futuras simulações


            //preenchendo textBox com as parcelas
            for (int i = 1; i <= numeroParcelas; i++) //laço de acordo com o nº de parcelas
            {
                DateTime dataVencimento = dataCompra.AddMonths(i);

                while (dataVencimento.DayOfWeek == DayOfWeek.Saturday || dataVencimento.DayOfWeek == DayOfWeek.Sunday)
                {
                    dataVencimento = dataVencimento.AddDays(1); //desconsidera finais de semana
                }

                //exibição
                string linha = i.ToString() + ". " + dataVencimento.ToString("dd/MM/yyyy") + " - Valor: " + valorParcela.ToString("C2");
                textBox4.AppendText(linha + Environment.NewLine);
            }

            label4.Text = "Valor Total: " + valorTotal.ToString("C2"); //atualizar Valor Total
            parcelas_pagas = 0; //seta como padrão que nenhuma parcela foi paga
        }

/*----------------------------------------------------------------------------------------------------------------*/

        private void button2_Click(object sender, EventArgs e) //botao para pagar parcela
        {           
            DateTime dataDoPagamento; //variáveis
            DateTime.TryParse(textBox5.Text, out dataDoPagamento);
            DateTime dataParcela;

            
            //verificar se a data do pagamento é válida e pagar parcela

            DateTime.TryParse(textBox3.Text, out dataParcela); //data inserida pelo usuario para pagar
            dataParcela = dataParcela.AddMonths(parcelas_pagas + 1); //obtém parcela atual para ser paga
            while ((int)dataParcela.DayOfWeek == 6 || (int)dataParcela.DayOfWeek == 0)
            {
                dataParcela = dataParcela.AddDays(1);
            }

            int.TryParse(textBox2.Text, out numeroParcelas);
            if (parcelas_pagas >= numeroParcelas)
            {
                MessageBox.Show("Todas as parcelas foram pagas!");
                return;
            }
            else
            {
                if (dataDoPagamento < dataParcela) //data inválida para pagar 
                {
                    MessageBox.Show("Data inválida! Não é possível pagar parcela antes da data da compra;");
                }
                else if (dataDoPagamento == dataParcela) //parcela paga na data correta
                {
                    parcelas_pagas++;
                    MessageBox.Show("Pagamento efetuado!" + Environment.NewLine + "Parcelas pagas: " + parcelas_pagas.ToString() + "/" + numeroParcelas.ToString());
                    //removendo do total
                    valorTotal -= valorParcela;
                    label4.Text = "Valor Total: " + valorTotal.ToString("C2");
                    label7.Text = ""; //zera o label de aviso de parcela atrasada
                }
                else if (dataDoPagamento > dataParcela) //parcela paga com atraso
                {
                    parcelas_pagas++;
                    float parcelaComJuros = valorParcela * 1.03F;
                    valorTotal = (valorTotal - valorParcela + parcelaComJuros) - valorParcela;
                    valorParcela = int.Parse(textBox1.Text) / int.Parse(textBox2.Text);
                    juros_acumulados += (parcelaComJuros - valorParcela);

                    MessageBox.Show("Pagamento realizado com atraso!" + Environment.NewLine + "Parcelas pagas: " + parcelas_pagas.ToString() + "/" + numeroParcelas.ToString());
                    label4.Text = "Valor Total: " + valorTotal.ToString("C2");
                    label7.Text = "OBS: Parcela paga com atraso. \nValor reajustado com juros: " + parcelaComJuros.ToString("C2");
                    label8.Text = "Juros acumulados: " + juros_acumulados.ToString("C2");
                }
            }     

            //remover do textBox4 a parcela paga
            textBox4.Text = "";
            for (int i = 1 + parcelas_pagas; i <= numeroParcelas; i++) //preenche novamente o textBox4, pulando as linhas das parcelas pagas
            {
                DateTime dataVencimento = dataCompra.AddMonths(i);

                while (dataVencimento.DayOfWeek == DayOfWeek.Saturday || dataVencimento.DayOfWeek == DayOfWeek.Sunday)
                {
                    dataVencimento = dataVencimento.AddDays(1); //mesma lógica dos finais de semana
                }

                //exibição
                string linha = i.ToString() + ". " + dataVencimento.ToString("dd/MM/yyyy") + " - Valor: " + valorParcela.ToString("C2");
                textBox4.AppendText(linha + Environment.NewLine);
            }

        }
/*----------------------------------------------------------------------------------------------------------------*/
        //botao que obtem data atual e passa para a data da compra
        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text = DateTime.Now.ToString("dd/MM/yy");
        }
    }
}
