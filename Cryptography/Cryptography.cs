using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Library.Cryptography
{
    public class Cryptography
    {
        public class AES
        {
            string z_EncryptionKey;

            public string EncryptionKey
            {
                get
                {
                    if (z_EncryptionKey == null)
                        z_EncryptionKey = "KEYSOFTWAREINC301CA93021";

                    return z_EncryptionKey;
                }
                set { z_EncryptionKey = value; }
            }

            public AES()
            {

            }

            public AES(string EncryptionKey)
                : base()
            {
                this.EncryptionKey = EncryptionKey;
            }

            public string Encrypt(string clearText)
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x29, 0x36, 0x11, 0x6e, 0x23, 0x4d, 0x62, 0x63, 0x78, 0x61, 0x64, 0x66, 0x14 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }

                return clearText;
            }

            public string Decrypt(string cipherText)
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x29, 0x36, 0x11, 0x6e, 0x23, 0x4d, 0x62, 0x63, 0x78, 0x61, 0x64, 0x66, 0x14 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
        }

        public class RSA
        {
            System.Security.Cryptography.RSACryptoServiceProvider provider;

            public RSA()
            {
                provider = new RSACryptoServiceProvider();
                RSAParameters p = provider.ExportParameters(true);
                //provider.ImportParameters(new RSAParameters(){D="aaa"}
            }

            public string Encrypt(string clearText)
            {
                return Convert.ToBase64String(provider.Encrypt(Encoding.ASCII.GetBytes(clearText), true));
            }

            public string Decrypt(string cipherText)
            {
                return Convert.ToString(provider.Decrypt(Convert.FromBase64String(cipherText), true));

            }
        }

        public class KeyCap
        {
            public class Crypto
            {
                private static byte[] BaseSalt = new byte[] { 158, 56, 196, 68, 115, 105, 88, 127, 64, 119, 77, 64, 92, 82, 65, 84, 95, 68, 92, 110, 59, 88, 111 };

                /// <summary>
                /// Returns an encrypted string for 'value' using the supplied key.
                /// </summary>
                /// <param name="value">The value to encrypt</param>
                /// <param name="key">The key that must be used to decrypt the value.</param>
                /// <param name="salt">Salt for the encryption process.</param>
                /// <returns>The encrypted value in base64.</returns>
                public static string Encrypt(string value, string key, string salt)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentNullException("value");
                    if (string.IsNullOrWhiteSpace(key))
                        throw new ArgumentNullException("key");

                    byte[] KeyBytes = Encoding.ASCII.GetBytes(key);
                    byte[] ValueBytes = Encoding.ASCII.GetBytes(value);
                    byte[] SaltBytes;
                    if (string.IsNullOrWhiteSpace(salt))
                        SaltBytes = BaseSalt;
                    else
                        SaltBytes = Encoding.ASCII.GetBytes(salt);

                    byte[] encrypted;

                    PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(KeyBytes, SaltBytes);

                    // Create an RijndaelManaged object with the specified key and IV.
                    using (RijndaelManaged rijAlg = new RijndaelManaged())
                    {
                        rijAlg.Key = SecretKey.GetBytes(32);
                        rijAlg.IV = SecretKey.GetBytes(16);

                        // Create a decrytor to perform the stream transform.
                        ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                        // Create the streams used for encryption.
                        using (MemoryStream msEncrypt = new MemoryStream())
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                                {
                                    //Write all data to the stream.
                                    swEncrypt.Write(value);
                                }
                                encrypted = msEncrypt.ToArray();
                            }
                        }
                    }

                    // Return the encrypted bytes from the memory stream.
                    return Convert.ToBase64String(encrypted);
                }

                private static byte[] Pad = new byte[] { 83, 117, 110, 110, 121, 68, 114, 97, 105, 110, 97, 103, 101, 68, 105, 116, 99, 104 };
                /// <summary>
                /// encrypt the config setting.
                /// </summary>
                /// <param name="value"></param>
                /// <param name="salt"></param>
                /// <returns></returns>
                public static string EncryptConfig(string value, string salt)
                {
                    string SecKey = Encoding.ASCII.GetString(Pad);
                    return Encrypt(value, SecKey, salt);
                }

                /// <summary>
                /// decrypt the config setting
                /// </summary>
                /// <param name="value"></param>
                /// <param name="salt"></param>
                /// <returns></returns>
                public static string DecryptConfig(string value, string salt)
                {
                    string SecKey = Encoding.ASCII.GetString(Pad);
                    return Decrypt(value, SecKey, salt);
                }
                /// <summary>
                /// Returns the decrypted result of the supplid 'value'.
                /// </summary>
                /// <param name="value">The ecrypted string in base64.</param>
                /// <param name="key">The key used when the encrypted value was created</param>
                /// <param name="salt">Salt for the encryption process.</param>
                /// <returns>The descrypted value.</returns>
                /// <exception cref="System.Security.Cryptography.CryptographicException">is thrown if
                /// the value could not be decrypted.</exception>
                /// <example>
                /// <para>Example show wrapping the decryption in a try-catch block.</para>
                /// <para>
                /// <code>
                /// string crypted = KeyCap.Security.Crypto.Encrypt(this.textBox1.Text, "testpassword", "testsalt");
                /// this.label1.Text = crypted;
                /// try
                /// {
                ///    this.label2.Text = KeyCap.Security.Crypto.Decrypt(crypted, "testpassword", "testsalt");
                /// }
                /// catch( System.Security.Cryptography.CryptographicException ex )
                /// {
                ///    this.label2.Text = "Failed to decrypt value.";
                /// }
                /// </code>
                /// </para>
                /// </example>
                public static string Decrypt(string value, string key, string salt)
                {
                    // Check arguments.
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentNullException("value");
                    if (string.IsNullOrWhiteSpace(key))
                        throw new ArgumentNullException("key");

                    byte[] KeyBytes = Encoding.ASCII.GetBytes(key);
                    byte[] ValueBytes = Convert.FromBase64String(value);
                    byte[] SaltBytes;
                    if (string.IsNullOrWhiteSpace(salt))
                        SaltBytes = BaseSalt;
                    else
                        SaltBytes = Encoding.ASCII.GetBytes(salt);

                    string plaintext = null;

                    PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(KeyBytes, SaltBytes);

                    // Create an RijndaelManaged objectwith the specified key and IV.
                    using (RijndaelManaged rijAlg = new RijndaelManaged())
                    {
                        rijAlg.Key = SecretKey.GetBytes(32);
                        rijAlg.IV = SecretKey.GetBytes(16);

                        // Create a decrytor to perform the stream transform.
                        ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                        // Create the streams used for decryption.
                        using (MemoryStream msDecrypt = new MemoryStream(ValueBytes))
                        {
                            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                                {
                                    // Read the decrypted bytes from the decrypting stream
                                    // and place them in a string.
                                    plaintext = srDecrypt.ReadToEnd();
                                }
                            }
                        }
                    }
                    return plaintext;
                }

                public static string EncryptPassword(string password, int salt)
                {
                    string Result = null;

                    if (password == null)
                        throw new ArgumentNullException("password");

                    // Create an 8 element array for the salt. 
                    byte[] SaltKey = new byte[8] { 15, 16, 85, 245, 195, 63, 74, 188 }; // base
                    byte[] SaltBytes = BitConverter.GetBytes(salt);
                    for (int i = 0; i < SaltBytes.Length && i < SaltKey.Length; i++)
                        // add bytes from salt value
                        SaltKey[i] = SaltBytes[i];

                    const string pwd1 = "Quantinf2Warfdok&Tubur";

                    Result = Encrypt(password, pwd1, Convert.ToBase64String(SaltKey));


                    return Result;
                }
            }
        }
    }
}
