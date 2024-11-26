using Microsoft.ML.Data;

namespace HuellaHidrica.Presentation;

public class HuellaHidricaResult
{
   [ColumnName("Score")]
   public float HuellaHidrica { get; set; }
}