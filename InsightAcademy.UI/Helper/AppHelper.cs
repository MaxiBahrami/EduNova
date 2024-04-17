namespace InsightAcademy.UI.Helper
{
    public static class AppHelper
    {
        public static string HideText(string text)
        {
            if (text.Length <= 4)
                return text; // If text is 4 characters or less, don't hide anything
            else
                return text.Substring(0, 4) + new string('*', text.Length - 4); // Replace remaining characters with *
        }
    }
}
