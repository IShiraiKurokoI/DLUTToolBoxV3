namespace DLUTToolBoxV3.Ultilities
{
    public class DES
    {
        public static string GetRSA(string Uid,string Password,string lt)
        {
            using (ScriptEngine engine = new ScriptEngine("jscript"))
            {
                ParsedScript parsed = engine.Parse(Properties.Resources.StrEnc);
                return (string)parsed.CallMethod("GetRSA", Uid+Password+lt);
            }
        }
    }
}
