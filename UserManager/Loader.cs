using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UserManager
{
    /// <summary>
    /// Loads users from database file
    /// </summary>
    class Loader
    {
        private char[] CStringToCharArray(byte[] rawData, int offset)
        {
            int length = 0;
            for (var i = offset; i < rawData.Length; i++) {
                if (rawData[i] == '\0') break;
                length++;
            }
            char[] vs = new char[length];
            Buffer.BlockCopy(vs, offset, vs, 0, length);
            return vs;
        }

        private byte[] CStringToByteArray(byte[] rawData, int offset)
        {
            int length = 0;
            for (var i = offset; i < rawData.Length; i++)
            {
                if (rawData[i] == '\0') break;
                length++;
            }
            byte[] vs = new byte[length];
            Buffer.BlockCopy(vs, offset, vs, 0, length);
            return vs;
        }

        private void WriteUserToStream(ref StreamWriter stream, UserInformation user)
        {
            user.username += '\0';

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream.BaseStream, user);
        }

        public void SaveDatabase(ref UserManager userManager, string filePath)
        {
            StreamWriter data = new StreamWriter(filePath);
            if (userManager != null)
            {
                foreach (var user in userManager)
                {
                    this.WriteUserToStream(ref data, user);
                }
            }
            data.Close();
            data.Dispose();
        }

        public void LoadDatabase(ref UserManager userManager, string filePath)
        {
            string fileString = File.ReadAllText(filePath);
            byte[] fileContents = Encoding.ASCII.GetBytes(fileString);

            int index = 0;
            while (index < fileContents.Length)
            {
                try
                {
                    UserInformation user = new UserInformation();
                    user.uid = BitConverter.ToUInt64(fileContents, index);
                    index += sizeof(UInt64);
                    user.username = new string(this.CStringToCharArray(fileContents, index));
                    index += user.username.Length;
                    Buffer.BlockCopy(fileContents, index, user.macAddress, 0, 6);
                    index += user.macAddress.Length;
                    user.expiryDate = fileContents[index];
                    index += sizeof(UInt64);
                    userManager.Add(user);
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }
            }
        }
    }
}
