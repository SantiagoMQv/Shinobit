using System.IO;
using System;
using UnityEngine;
using Newtonsoft.Json;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        // Usamos Path.Combine para tener en cuenta los distintos OS, añadiendo distintos separadores de ruta
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath)){
            try
            {
                // Cargamos los datos serializados desde el archivo
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // opcionalmente encriptamos los datos
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // deserializamos los datos de json a un objeto C#
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.LogError("Error ocurred when trying to load data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // Usamos Path.Combine para tener en cuenta los distintos OS, añadiendo distintos separadores de ruta
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serializamos los datos a json
            string dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented);

            // opcionalmente encriptamos los datos
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // Escribimos los datos serializados en un archivo
            // using garantiza cerrar la conexión al terminar
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {

            Debug.LogError("Error ocurred when trying to save data to file: " + fullPath + "\n" + e);
        }

    }


    // Implementación simple de una encriptación XOR
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    public bool ExistSaveGameFile()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        return File.Exists(fullPath);
    }
}
