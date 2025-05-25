using SimpleGraphCalculatorApp.Models;
using System.IO;
using System.Text.Json;
using SimpleGraphCalculatorApp.Interfaces;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace SimpleGraphCalculatorApp.Services
{
    public static class SettingsService
    {
        private static readonly string fileDirectory;
        public static readonly string filePath;
        private static readonly IMessageService messageService;
        private static List<FunctionParameters> parametersList;

        static SettingsService()
        {
            // Initialize static fields
            fileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
            filePath = Path.Combine(fileDirectory,"param_settings.json");
            messageService = new MessageService();
            parametersList = new List<FunctionParameters>();
        }

        public static List<FunctionParameters> GetParameterList()
        {  
            if (File.Exists(filePath))
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals | JsonNumberHandling.AllowReadingFromString
                    };

                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        parametersList = JsonSerializer.Deserialize<List<FunctionParameters>>(fs, options);
                    }
                }
                catch (JsonException ex)
                {
                    messageService.ShowMessage($"Error during loading parameter list data from Json file -> {ex.Message}", "Error");
                }
            }

            return parametersList;
        }


        public static bool ParameterExists(FunctionParameters parameters)
        {            
            return parametersList.Exists(p => p.Phase == parameters.Phase && p.Amplitude == parameters.Amplitude
                && p.Frequency == parameters.Frequency && p.RangeStart == parameters.RangeStart && p.RangeEnd == parameters.RangeEnd);
        }

        public static int GetParameterIndex(FunctionParameters parameters)
        {
            int index = parametersList.FindIndex(p => p.Phase == parameters.Phase && p.Amplitude == parameters.Amplitude
                        && p.Frequency == parameters.Frequency && p.RangeStart == parameters.RangeStart && p.RangeEnd == parameters.RangeEnd);
            return index;
        }

        public static void Save(FunctionParameters parameters)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    parametersList = GetParameterList();
                }
                else
                {
                    if (!Directory.Exists(fileDirectory))
                    {
                        Directory.CreateDirectory(fileDirectory);
                    }

                    File.Create(filePath).Close(); // Create file if it does not exist

                    parametersList = new List<FunctionParameters>();
                }

                // Remove existing parameter from list and add again due to apply stack for load mechanism
                if (ParameterExists(parameters))
                {
                    parametersList.RemoveAt(GetParameterIndex(parameters)); 
                }
                                
                parametersList.Add(parameters);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    JsonSerializer.Serialize(fs, parametersList, options);
                    fs.Flush();
                }
            }
            catch (JsonException ex)
            {
                messageService.ShowMessage($"Error during saving parameters data into Json file -> {ex.Message}", "Error");
            }           
        }

        public static FunctionParameters Load()
        {
            var parametersList = new Stack<FunctionParameters>();

            try
            {
                if (!File.Exists(filePath))
                {
                    if (!Directory.Exists(fileDirectory))
                    {
                        Directory.CreateDirectory(fileDirectory);
                    }
                    return new FunctionParameters(); // return default  
                }
                
                var options = new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals | JsonNumberHandling.AllowReadingFromString
                };

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    parametersList = JsonSerializer.Deserialize<Stack<FunctionParameters>>(fs, options);                    
                }              
            }
            catch (JsonException ex)
            {
                messageService.ShowMessage($"Error during loading parameters data from Json file -> {ex.Message}", "Error");                
            }
            
            
            if (parametersList.Count == 0)
            {
                return new FunctionParameters(); // return default if no parameters found in json file
            }

            return parametersList.Pop();
        }
    }
}
