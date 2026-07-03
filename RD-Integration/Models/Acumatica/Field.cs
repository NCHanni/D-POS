namespace RD_INTEGRATION.Models
{
    public class Field {
        public Field()
        {
            this.value = "";
        }

        public Field(string value)
        {
            this.value = value;
        }

        public string value { get; set; }
    }
}
