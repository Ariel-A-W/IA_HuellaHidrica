using HuellaHidrica.Presentation;
using Microsoft.ML;
using Newtonsoft.Json;

namespace HuellaHidrica.Process;

public class HuellaHidricaPrediccion
{
    public void Process()
    {
        // Crear un contexto de ML.NET
        var mlContext = new MLContext();

        string relativePath = Path.Combine("Data", "cultivo.json");
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Archivo JSON no encontrado.");
        }
        else
        {
            string jsonContent = File.ReadAllText(filePath);

            var objectContent = JsonConvert.DeserializeObject<List<CultivoData>>(jsonContent);

            // Convertir la lista de datos en un IDataView
            var trainData = mlContext.Data.LoadFromEnumerable(objectContent);

            // 3. Configurar el pipeline de entrenamiento
            var pipeline = mlContext.Transforms.Concatenate("Features",
                    nameof(CultivoData.Area),
                    nameof(CultivoData.LluviaAnual),
                    nameof(CultivoData.EficienciaRiego),
                    nameof(CultivoData.RetencionAguaSuelo),
                    nameof(CultivoData.Temperatura))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.Sdca(
                    labelColumnName: "HuellaHidrica", 
                    featureColumnName: "Features"
                ));

            // 4. Entrenar el modelo
            var model = pipeline.Fit(trainData);

            // 5. Realizar predicciones
            var predictionEngine = 
                mlContext.Model.CreatePredictionEngine<CultivoData, HuellaHidricaResult>(model);

            var newSample = new CultivoData
            {
                TipoCultivo = "Trigo",
                Area = 40,
                LluviaAnual = 800,
                EficienciaRiego = 70,
                RetencionAguaSuelo = 60,
                Temperatura = 20
            };

            var prediction = predictionEngine.Predict(newSample);

            Console.WriteLine("Predicción Cálculo Huella Hídrica");
            Console.WriteLine("*********************************\n");
            Console.WriteLine("Datos para el Análisis:");
            Console.WriteLine("***********************");
            Console.WriteLine($"Tipo de Cultivo: {newSample.TipoCultivo}");
            Console.WriteLine($"Área: {newSample.Area:F2} ha");
            Console.WriteLine($"Lluvia Anual: {newSample.LluviaAnual:F2} mm");
            Console.WriteLine($"Eficiencia de Riego: {newSample.EficienciaRiego:F2}%");
            Console.WriteLine($"Retención Agua en el Suelo: {newSample.RetencionAguaSuelo:F2}%");
            Console.WriteLine($"Temperatura C°: {newSample.Area:F2}\n");
            Console.WriteLine("Resultados:");
            Console.WriteLine("***********");
            Console.WriteLine($"Predicción Huella Hídrica: {prediction.HuellaHidrica:0.##} m³/ton");
        }
    }
}
