using System.Text;

namespace Checkpoint.IdentityServer
{
    public static class Verification
    {
        public static string GenerateVerification()
        {
            //ABC-DEF formatında
            string verification = string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            Random rnd = new Random();
            for (int i = 1; i <= 6; i++)
            {
                var rndNumber = rnd.Next(64, 91);
                stringBuilder.Append((char)rndNumber);
                if (i == 3)
                {
                    stringBuilder.Append("-");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
