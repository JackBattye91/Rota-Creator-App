using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public class Position
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public bool[] ActiveHours { get; set; }

        public Position(string name = "", int duration = 1)
        {
            Name = name;
            DefaultDuration = duration;
            ActiveHours = new bool[167];
            for(int b = 0; b < 167; b++)
                ActiveHours[b] = true;
        }

        public bool IsActive(DateTime time)
        {
            // convert Day to week from sun-sat TO mon-sun
            int day = time.DateOfWeek == DayOfWeek.Sunday ? 6 : ((int)time.DayOfWeek - 1);

            return ActiveHours[day * 24 + time.Hour];
        }

        public static List<Position> Load(string filePath)
        {
            List<Position> positions = new List<Position>();
            
            try
            {
                if (Properties.Settings.Default.UseEncryption)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Position>));
                    using (FileStream fStream = File.OpenRead(filePath))
                    {
                        using (Rijndael rijAlg = Rijndael.Create())
                        {
                            rijAlg.Key = Convert.FromBase64String(Properties.Settings.Default.EncryptionKey);
                            rijAlg.IV = Convert.FromBase64String(Properties.Settings.Default.EncryptionIV);

                            // Create an decryptor to perform the stream transform.
                            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                            using (CryptoStream csDecryptor = new CryptoStream(fStream, decryptor, CryptoStreamMode.Read))
                            {
                                positions = serializer.Deserialize(csDecryptor) as List<Position>;
                            }
                        }
                    }
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Position>));
                    using (FileStream fStream = File.OpenRead(filePath))
                    {
                        positions = serializer.Deserialize(fStream) as List<Position>;
                    }
                }
            }
            catch (Exception e)
            {
                // UpdateStatusText(e.Message);
            }

            return positions;
        }

        public static bool SaveToFile(List<Position> positions, string filePath)
        {
            if (positions.Count > 0)
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
                                    serializer.Serialize(csEncrypt, positions);
                                }
                            }
                        }
                    }
                    else
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Position>));
                        using (FileStream fStream = File.OpenWrite(filePath))
                        {
                            serializer.Serialize(fStream, positions);
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