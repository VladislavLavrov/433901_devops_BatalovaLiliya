
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calculator.Data
{
    public class DataInputVariant
    {
        [Key]
        [Column("Id")]
        public int ID_DataInputVariant { get; set; }

        [Column("Operand_1")]
        public double Operand_1 { get; set; }

        [Column("Operand_2")]
        public double Operand_2 { get; set; }

        [Column("Operation")]
        public string? Type_operation { get; set; }

        [Column("Result")]
        public double Result { get; set; }

        [Column("ExecutionDate")]
        public DateTime? ExecutionDate { get; set; }
    }
}
