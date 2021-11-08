namespace InvoiceApi.Response
{
  public class DataNode<T>
  {
      public string id { get; private set; }
      public string type { get; private set; }
      #nullable enable
      public T? attributes { get; private set; }

      public DataNode(string id, string type, T attributes) {
        this.id = id;
        this.type = type;
        this.attributes = attributes;
      }

      public DataNode(string id, string type) {
        this.id = id;
        this.type = type;
      }
  }
}