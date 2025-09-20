using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                { 
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null) 
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Descrição: {t.description} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Pôr do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Mín: {t.temp_min} \n" +
                                         $"Velocidade do Vento: {t.speed} m/s \n" +
                                         $"Visibilidade: {t.visibility} m";

                        lbl_res.Text = dados_previsao;
                    }
                    else 
                    {
                        lbl_res.Text = "Cidade não encontrada.";
                    }
                }
                else 
                {
                    lbl_res.Text = "Preencha o nome da cidade.";
                }
            }
            catch (HttpRequestException HttpEx)
            {
                await DisplayAlert("Erro de rede", HttpEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
