namespace BookingApp.Users.Domain
{
    public interface IAESCryptography
    {
        string Decrypt(string cipherText);

        string Encrypt(string plainText);
    }
}
