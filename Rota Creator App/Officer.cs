using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public class Officer
    {
        public int ID { get; protected set; }
        public string Name 
        { 
            get { return Name; }
            set
            {
                if (value != "")
                    Name = value;
            }
        }
        public string Team { get; set; }
        public List<Position> WorkablePositions { get; protected set; }

        public Officer()
        {
            ID = new Random().Next();
            WorkablePositions = new List<Position>();
        }

        public bool CanWorkPosition(Position pos)
        {
            foreach(Position workPos in WorkablePositions)
            {
                if (workPos == pos)
                    return true;
            }

            return false;
        }

        public bool Equals(Officer officer)
        {
            if (ID == officer.ID)
                return true;

            return false;
        }

        static List<Officer> Load()
        {
            List<Officer> officers = new List<Officer>();

            try
            {
                if (Properties.Settings.Default.UseEncryption)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Officer>));
                    using (FileStream fStream = File.OpenRead(Properties.Settings.Default.OfficersFileName))
                    {
                        using (Rijndael rijAlg = Rijndael.Create())
                        {
                            rijAlg.Key = Convert.FromBase64String(Properties.Settings.Default.EncryptionKey);
                            rijAlg.IV = Convert.FromBase64String(Properties.Settings.Default.EncryptionIV);

                            // Create an decryptor to perform the stream transform.
                            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                            using (CryptoStream csDecryptor = new CryptoStream(fStream, decryptor, CryptoStreamMode.Read))
                            {
                                Officers = serializer.Deserialize(csDecryptor) as List<Officer>;
                            }
                        }
                    }
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Officer>));
                    using (FileStream fStream = File.OpenRead(Properties.Settings.Default.OfficersFileName))
                    {
                        officers = serializer.Deserialize(fStream) as List<Officer>;
                    }
                }
            }
            catch (Exception e)
            {
                // UpdateStatusText(e.Message);
            }

            return officers;
        }

        public static bool SaveToFile(List<Officer> officers, string filePath)
        {
            if (officers.Count > 0)
            {
                try
                {
                    File.Delete(filePath);

                    if (Properties.Settings.Default.UseEncryption)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Position>));
                        using (FileStream fStream = File.OpenWrite(filePath))
                        {
                            using (Rijndael rijAlg = Rijndael.Create())
                            {
                                rijAlg.Key = Convert.FromBase64String(Properties.Settings.Default.EncryptionKey);
                                rijAlg.IV = Convert.FromBase64String(Properties.Settings.Default.EncryptionIV);

                                // Create an encryptor to perform the stream transform.
                                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                                using (CryptoStream csEncrypt = new CryptoStream(fStream, encryptor, CryptoStreamMode.Write))
                                {
                                    serializer.Serialize(csEncrypt, officers);
                                }
                            }
                        }
                    }
                    else
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Position>));
                        using (FileStream fStream = File.OpenWrite(filePath))
                        {
                            serializer.Serialize(fStream, officers);
                        }
                    }
                }
                catch (Exception e)
                {
                    //UpdateStatusText(e.Message);
                }
            }
        }

    }
}