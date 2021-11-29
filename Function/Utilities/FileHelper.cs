using Function.Interfaces;
using System;
using System.IO;
using System.Reflection;

namespace Function.Utilities
{
    internal class FileHelper : IFileHelper
    {
        public string GetToxicPlantAnimalFileLocation()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                                throw new InvalidOperationException("File not found")
                , "Data", "ToxicPlants.csv");
        }
    }
}
