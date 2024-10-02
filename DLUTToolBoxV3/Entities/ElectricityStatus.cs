namespace DLUTToolBoxV3.Entities
{
    public class ElectricityStatus
    {
        public string message { get; set; }
        public Resultdata resultData { get; set; }
        public bool success { get; set; }
        public class Resultdata
        {
            public string sydl { get; set; }
        }

    }
}
