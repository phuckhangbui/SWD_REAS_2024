namespace API.Validate
{
    public class ParseValidate
    {
        public Int64 ParseStringToInt (string text)
        {
            Int64 number = 0;
            string newtext = text.Replace(",", "");
            try
            {
                number = Int64.Parse(newtext);
            }catch (FormatException)
            {
                number = -1;
            }
            return number;
        }

    }
}
