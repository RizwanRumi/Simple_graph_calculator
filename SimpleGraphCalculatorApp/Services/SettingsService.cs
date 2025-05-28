using SimpleGraphCalculatorApp.Models;
using System.IO;
using SimpleGraphCalculatorApp.Interfaces;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace SimpleGraphCalculatorApp.Services
{
    public static class SettingsService
    {
        // Public properties to allow setting from tests or other parts of the application
        public static string FileDirectory { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
        public static string FilePath { get; set; } = Path.Combine(FileDirectory, "param_settings.json");

        public static IMessageService messageService;
        private static List<FunctionParameters> parametersList;

        // Add a method to inject IMessageService for Mock test
        public static void SetMessageService(IMessageService service)
        {
            messageService = service;
        }

        static SettingsService()
        {
            // Initialize static fields
            messageService = new MessageService();
            parametersList = new List<FunctionParameters>();
        }

        public static List<FunctionParameters> GetParameterList()
        {           
            try
            { 
                // Check if file is empty first
                var fileInfo = new FileInfo(FilePath);
                if (fileInfo.Length == 0)
                {
                    return new List<FunctionParameters>();
                }

                // File.ReadAllText handles file sharing internally
                string jsonContent = File.ReadAllText(FilePath);

                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    return new List<FunctionParameters>();
                }

                parametersList = JsonConvert.DeserializeObject<List<FunctionParameters>>(jsonContent);                
            }
            catch (JsonException ex)
            {
                messageService.ShowMessage($"Error during loading parameter list data from Json file -> {ex.Message}", "Error");
            }
            catch (IOException)
            {
                messageService.ShowMessage("Error accessing file: The system cannot find the file specified.", "Error");                
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

        public static void SaveParameters(FunctionParameters parameters)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    parametersList = GetParameterList();
                }
                else
                {
                    if (!Directory.Exists(FileDirectory))
                    {
                        Directory.CreateDirectory(FileDirectory);
                    }

                    parametersList = new List<FunctionParameters>();
                }

                // Remove existing parameter from list and add again due to apply stack for load mechanism
                if (ParameterExists(parameters))
                {
                    parametersList.RemoveAt(GetParameterIndex(parameters)); 
                }
                                
                parametersList.Add(parameters);
                
                string jsonContent = JsonConvert.SerializeObject(parametersList, Formatting.Indented);
                File.WriteAllText(FilePath, jsonContent);
            }
            catch (JsonException ex)
            {
                messageService.ShowMessage($"Error during saving parameters data into Json file -> {ex.Message}", "Error");
            }           
        }

        public static FunctionParameters LoadParameters()
        {
            var parametersList = new Stack<FunctionParameters>();

            try
            {
                var fileInfo = new FileInfo(FilePath);
                if (fileInfo.Length == 0)
                {
                    return new FunctionParameters();
                    
                }

                string jsonContent = File.ReadAllText(FilePath);

                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    return new FunctionParameters();
                }

                parametersList = JsonConvert.DeserializeObject<Stack<FunctionParameters>>(jsonContent);

            }
            catch (JsonException ex)
            {
                messageService.ShowMessage($"Error during loading parameters data from Json file -> {ex.Message}", "Error");                
            }
            catch (IOException)
            {
                messageService.ShowMessage("Error accessing file: The system cannot find the file specified.", "Error");
            }


            if (parametersList.Count == 0)
            {
                return new FunctionParameters(); // return default if no parameters found in json file
            }

            return parametersList.Pop();
        }
    }
}
