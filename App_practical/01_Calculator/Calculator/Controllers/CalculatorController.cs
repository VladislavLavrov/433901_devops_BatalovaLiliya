using Calculator.Data;
using Calculator.Services;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Calculator.Controllers
{
    public enum Operation { Add = 1, Subtract = 2, Multiply = 3, Divide = 4 }

    public class CalculatorController : Controller
    {
        private readonly CalculatorContext _context;
        private readonly KafkaProducerService<Null, string> _producer;

        public CalculatorController(CalculatorContext context, KafkaProducerService<Null, string> producer)
        {
            _context = context;
            _producer = producer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(double num1, double num2, Operation operation)
        {
            double result = 0;
            switch (operation)
            {
                case Operation.Add: result = num1 + num2; break;
                case Operation.Subtract: result = num1 - num2; break;
                case Operation.Multiply: result = num1 * num2; break;
                case Operation.Divide: result = num2 != 0 ? num1 / num2 : 0; break;
            }
            ViewBag.Result = result;

            var dataInputVariant = new DataInputVariant
            {
                Operand_1 = num1.ToString(),
                Operand_2 = num2.ToString(),
                Type_operation = operation.ToString(),
                Result = result.ToString()
            };

            await SendDataToKafka(dataInputVariant);

            return View("Index");
        }

        [HttpPost]
        public IActionResult Callback([FromBody] DataInputVariant inputData)
        {
            _context.DataInputVariants.Add(inputData);
            _context.SaveChanges();
            return Ok();
        }

        private async Task SendDataToKafka(DataInputVariant dataInputVariant)
        {
            var json = JsonSerializer.Serialize(dataInputVariant);
            await _producer.ProduceAsync("lavrov", new Message<Null, string> { Value = json });
        }
    }
}