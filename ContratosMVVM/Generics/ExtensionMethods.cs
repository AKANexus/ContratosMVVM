using System.Globalization;

namespace ContratosMVVM.Generics
{
    public static class ExtensionMethods
    {
        static readonly TextInfo PtBRTextInfo = new CultureInfo("pt-BR", false).TextInfo;

        public static string ToTitleCase(this string input)
        {

            return PtBRTextInfo.ToTitleCase((input ?? "").ToLower());
        }
    }
}
