namespace InvoiceApi.Formatters
{
  public class DataNode<T>
  {
      public string id { get; set; }
      public string type { get; set; }
      public T attributes { get; set; }

      public DataNode(int id, string type, T attributes) {
        this.id = id.ToString();
        this.type = type;
        this.attributes = attributes;
      }
  }
}