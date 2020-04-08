// URL FOR CLOUD FUNCTION
// https://adam-cloud-hw-2.azurewebsites.net/api/Adam-Cloud-HW-2?code=O6oybPczG8vdZaQPYVQXrRiukQop4VZpSqXDdux7h2BFd3o907X1mw==
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Cloud_HW2
{
    public class StackCalculator
    {
        Stack<decimal> stack = new Stack<decimal>();
        StringBuilder result = new StringBuilder();
        public string Calculate(string[] commands)
        {
            foreach (var command in commands)
            {
                var splits = command.Split(' ');
                if (splits.Length > 0)
                {
                    switch (splits[0])
                    {
                        case "EXIT":
                            break;
                        case "PUSH":
                            Push(splits[1]);
                            break;
                        case "POP":
                            Pop();
                            break;
                        case "PRINT":
                            Print();
                            break;
                        case "ADD":
                            Add();
                            break;
                        case "SUBTRACT":
                            Subtract();
                            break;
                        case "MULTIPLY":
                            Multiply();
                            break;
                        case "DIVIDE":
                            Divide();
                            break;
                        default:
                            result.AppendLine("Unknown command");
                            break;
                    }
                }
            }
            return result.ToString();
        }
        void Push(string splits)
        {
            if (stack.Count < 10)
            {
                var number = decimal.Parse(splits);
                stack.Push(number);
            }
            else
            {
                result.AppendLine("Stack is limited to a size of 10");
            }


        }

        void Print()
        {
            if (stack.Count > 0)
            {
                var number = stack.Peek();
                result.AppendLine(number.ToString());
            }
            else
            {
                result.AppendLine("Invalid Operation on Current Stack Size");
            }
        }

        void Pop()
        {
            if (stack.Count > 0)
            {
                stack.Pop();
            }
            else
            {
                result.AppendLine("Invalid Operation on Current Stack Size");
            }
        }
        void Add()
        {
            if (stack.Count > 1)
            {
                var numberOne = stack.Pop();
                var numberTwo = stack.Pop();
                var total = numberOne + numberTwo;
                Push(total.ToString());
            }
            else
            {
                result.AppendLine("Invalid Operation on Current Stack Size");
            }

        }
        void Subtract()
        {
            if (stack.Count > 1)
            {
                var numberOne = stack.Pop();
                var numberTwo = stack.Pop();
                var total = numberTwo - numberOne;
                Push(total.ToString());
            }
            else
            {
                result.AppendLine("Invalid Operation on Current Stack Size");
            }

        }
        void Multiply()
        {
            if (stack.Count > 1)
            {
                var numberOne = stack.Pop();
                var numberTwo = stack.Pop();
                var total = numberOne * numberTwo;
                Push(total.ToString());
            }
            else
            {
                result.AppendLine("Invalid Operation on Current Stack Size");
            }

        }
        void Divide()
        {
            if (stack.Count > 1)
            {
                var numberOne = stack.Pop();
                var numberTwo = stack.Pop();
                var total = numberTwo / numberOne;
                Push(total.ToString());
            }
            else
            {
                result.AppendLine("Invalid Operation on Current Stack Size");
            }

        }
    }
    public static class Cloud_HW2
    {
        [FunctionName("Adam-Cloud-HW-2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(req.Body))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    Console.WriteLine(line);
                    lines.Add(line);
                }
            }
            var calculator = new StackCalculator();
            var response = calculator.Calculate(lines.ToArray());
            return new OkObjectResult(response);
        }
    }
}
