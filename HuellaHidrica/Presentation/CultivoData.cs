using Microsoft.ML.Data;

namespace HuellaHidrica.Presentation;

public class CultivoData
{
    [LoadColumn(0)] 
    public string? TipoCultivo { get; set; }
    [LoadColumn(1)] 
    public float Area { get; set; }
    [LoadColumn(2)] 
    public float LluviaAnual { get; set; }
    [LoadColumn(3)] 
    public float EficienciaRiego { get; set; }
    [LoadColumn(4)] 
    public float RetencionAguaSuelo { get; set; }
    [LoadColumn(5)] 
    public float Temperatura { get; set; }

    public float HuellaHidrica { get; set; }
}